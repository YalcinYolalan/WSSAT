using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Xml.Linq;
using WSSAT.BusinessLayer;
using WSSAT.DataTypes;
using WSSAT.Helpers;

namespace WSSAT
{
    public partial class MainForm : Form
    {
        List<WSDescriber> services = null;
        string scanDirectory = null;
        public static Vulnerabilities vulnerabilities = null;
        public static StaticVulnerabilities staticVulnerabilities = null;
        public static DisclosureVulnerabilities disclosureVulnerabilities = null;

        public static string CustomSoapHeaderTags = string.Empty;
        public static string CustomSoapBodyTags = string.Empty;

        public static string CustomRequestHeader = string.Empty;
        public static string UserAgentHeader = string.Empty;

        public RESTApi RestAPIDesc;

        public MainForm()
        {
            InitializeComponent();

            UserAgentHeader = ConfigurationManager.AppSettings.Get("userAgent");
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                services = new List<WSDescriber>();
                string line;

                StreamReader file = null;
                try
                {
                    file = new StreamReader(openFileDialog1.FileName);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (!line.StartsWith("#"))
                            {
                                WSDescriber wsDesc = new WSDescriber();
                                string[] arr = line.Split('|');
                                wsDesc.WSDLAddress = arr[0].Trim();

                                wsDesc.WSUri = new Uri(arr[0].Trim());

                                if (arr.Length > 1)
                                {
                                    wsDesc.BasicAuthentication = new BasicAuthentication();

                                    wsDesc.BasicAuthentication.Username = arr[1].Trim();
                                    wsDesc.BasicAuthentication.Password = arr[2].Trim();
                                }

                                services.Add(wsDesc);
                            }
                        }
                    }
                }
                catch
                {
                }
                finally {
                    if (file != null)
                    {
                        file.Close();
                    }
                }
                lblSelectedFileName.Text = openFileDialog1.SafeFileName;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (services != null && services.Count > 0)
            {
                lvResult.Items.Clear();

                scanDirectory = DirectoryHelper.GetScanDirectoryName();
                DirectoryHelper.CreateScanDirectory(scanDirectory);

                ReportObject reportObject = new ReportObject();
                reportObject.ScanStartDate = DateTime.Now;
                reportObject.WsDescs = new List<WSDescriberForReport>();
                reportObject.TotalRequestCount = 0;

                Log("Scan Started: " + reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true, false);

                foreach (WSDescriber wsDesc in services)
                {
                    WSDescriberForReport WSItemVulnerabilities = new WSDescriberForReport();

                    WSItemVulnerabilities.WsDesc = wsDesc;
                    WSItemVulnerabilities.Vulns = new List<VulnerabilityForReport>();
                    WSItemVulnerabilities.StaticVulns = new List<StaticVulnerabilityForReport>();
                    WSItemVulnerabilities.InfoVulns = new List<DisclosureVulnerabilityForReport>();

                    Log("WSDL Address: " + wsDesc.WSDLAddress, FontStyle.Bold, true, false);
                    Log("Parsing WSDL...", FontStyle.Regular, true, false);

                    List<Param> respHeader = new List<Param>();

                    bool untrustedSSLSecureChannel = false;
                    Parser parser = null;
                    try
                    {
                        parser = new Parser(wsDesc, ref untrustedSSLSecureChannel, ref respHeader, CustomRequestHeader);
                    }
                    catch (Exception parseEx)
                    {
                        Log("WSDL Parsing Exception: " + parseEx.Message, FontStyle.Regular, true, false);
                    }

                    if (chkStaticScan.Checked && parser != null)
                    {
                        Log("Static Analysis Started", FontStyle.Regular, true, false);

                        StaticVulnerabilityScanner svs = new StaticVulnerabilityScanner();

                        foreach (StaticVulnerabilitiesStaticVulnerability staticVuln in staticVulnerabilities.StaticVulnerability)
                        {
                            Log("   Testing: " + staticVuln.title, FontStyle.Regular, chkDebug.Checked, false);

                            string staticScanRes = svs.ScanIt(staticVuln, parser.rawWSDL);

                            if (!string.IsNullOrEmpty(staticScanRes))
                            {
                                Log("   " + staticVuln.title + " Vulnerability Found: " + staticScanRes, FontStyle.Bold, true, false);
                                StaticVulnerabilityForReport vulnRep = new StaticVulnerabilityForReport();
                                vulnRep.Vuln = staticVuln;
                                vulnRep.XMLLine = staticScanRes;

                                WSItemVulnerabilities.StaticVulns.Add(vulnRep);
                            }
                        }

                        Log("Static Analysis Finished", FontStyle.Regular, true, false);
                    }

                    if (chkDynamicScan.Checked && parser != null)
                    {
                        Log("Getting Methods...", FontStyle.Regular, true, false);
                        List<WSOperation> operations = parser.GetOperations();

                        WebServiceToInvoke wsInvoker = new WebServiceToInvoke(wsDesc.WSDLAddress.Replace("?WSDL", ""));

                        if (!wsDesc.WSUri.Scheme.Equals("https"))
                        {
                            Log(" Vulnerability Found - SSL Not Used, Uri Schema is " + wsDesc.WSUri.Scheme, FontStyle.Bold, true, false);
                            AddSSLRelatedVulnerability(WSItemVulnerabilities, 0);
                        }
                        else
                        {
                            if (untrustedSSLSecureChannel)
                            {
                                Log(" Vulnerability Found - Could not establish trust relationship for the SSL/TLS secure channel.", FontStyle.Bold, true, false);
                                AddSSLRelatedVulnerability(WSItemVulnerabilities, -1);
                            }
                        }                        

                        DynamicVulnerabilityScanner dynScn = new DynamicVulnerabilityScanner(this);

                        foreach (WSOperation operation in operations)
                        {
                            Log("Method: " + operation.MethodName, FontStyle.Regular, chkDebug.Checked, false);

                            foreach (VulnerabilitiesVulnerability vuln in vulnerabilities.Vulnerability)
                            {
                                if (vuln.type == 1 || vuln.type == 3) // 1: soap specific , 3: common for soap & rest
                                {
                                    if (vuln.id != 0 && vuln.id != 7 && vuln.id != 9 && vuln.id != 10) // 0 for insecure transport - ssl not used , 7 for verbose soap fault message , 9 for HTTP Options , 10 for XST
                                    {
                                        wsInvoker.PreInvoke();

                                        Log("   Testing: " + vuln.title, FontStyle.Regular, chkDebug.Checked, false);
                                        Log("   Parameter Count: " + operation.Parameters.Count, FontStyle.Regular, chkDebug.Checked, false);

                                        try
                                        {
                                            dynScn.ScanVulnerabilities(wsInvoker, operation, vuln, parser.TargetNameSpace, wsDesc, WSItemVulnerabilities, reportObject,
                                                chkDebug.Checked, ref respHeader, CustomSoapHeaderTags.Trim(), CustomSoapBodyTags.Trim(), CustomRequestHeader.Trim());
                                        }
                                        catch (System.Web.Services.Protocols.SoapException soapEx)
                                        {
                                            dynScn.SetSoapFaultException(operation, soapEx, WSItemVulnerabilities, chkDebug.Checked);
                                        }
                                        catch (Exception ex)
                                        {
                                            Log("   Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                                        }
                                    }
                                }
                            }
                        }

                        try
                        {
                            VulnerabilitiesVulnerability optionsVuln = vulnerabilities.Vulnerability.Where(v => v.id == 9).FirstOrDefault();
                            if (optionsVuln != null)
                            {
                                dynScn.CheckHTTPOptionsVulns(wsDesc, optionsVuln, WSItemVulnerabilities, reportObject,
                                                chkDebug.Checked, ref respHeader, CustomRequestHeader);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log("   CheckHTTPOptionsVulns - Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                        }

                        try
                        {
                            VulnerabilitiesVulnerability xstVuln = vulnerabilities.Vulnerability.Where(v => v.id == 10).FirstOrDefault();
                            if (xstVuln != null)
                            {
                                dynScn.CheckXSTVulns(wsDesc, xstVuln, WSItemVulnerabilities, reportObject,
                                                chkDebug.Checked, ref respHeader, CustomRequestHeader);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log("   CheckXSTVulns - Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                        }
                    }

                    if (chkInfoDisclosure.Checked)
                    {
                        Log("Information Disclosure Analysis Started", FontStyle.Regular, true, false);

                        InformationDisclosureVulnerabilityScanner idvs = new InformationDisclosureVulnerabilityScanner(this);

                        foreach (InformationDisclosureVulnerability infoVuln in disclosureVulnerabilities.Vulnerability)
                        {
                            Log("   Searching Response Header: " + infoVuln.title, FontStyle.Regular, chkDebug.Checked, false);

                            string infoScanRes = idvs.ScanIt(infoVuln, respHeader);

                            if (!string.IsNullOrEmpty(infoScanRes))
                            {
                                Log("   " + infoVuln.title + " Information Disclosure Found: " + infoScanRes, FontStyle.Bold, true, false);
                                DisclosureVulnerabilityForReport vulnRep = new DisclosureVulnerabilityForReport();
                                vulnRep.Vuln = infoVuln;
                                vulnRep.Value = infoScanRes;

                                WSItemVulnerabilities.InfoVulns.Add(vulnRep);
                            }
                        }

                        Log("Information Disclosure Analysis Finished", FontStyle.Regular, true, false);
                    }

                    reportObject.WsDescs.Add(WSItemVulnerabilities);
                }

                reportObject.ScanEndDate = DateTime.Now;

                Log("Scan Finished: " + reportObject.ScanEndDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true, false);

                string reportFilePath = scanDirectory + @"\Report\Report.html";
                string xmlFilePath = scanDirectory + @"\Report\Report.xml";

                string reportTemplatePath = System.AppDomain.CurrentDomain.BaseDirectory + 
                    @"\ReportTemplates\HTMLReportTemplate.html";

                ReportHelper.CreateHTMLReport(reportObject, 
                    reportTemplatePath,
                    reportFilePath, chkXMLReport.Checked, xmlFilePath);

                //if (chkXMLReport.Checked)
                //{
                //    Process.Start("cmd.exe /c notepad.exe " + xmlFilePath);
                //}

                Process.Start(reportFilePath);
            }
            else
            {
                MessageBox.Show("Please Select WSDL List File!!!");
            }
        }

        public void ScanRESTApi()
        {
            if (RestAPIDesc != null)
            {
                lvResult.Items.Clear();

                scanDirectory = DirectoryHelper.GetScanDirectoryName();
                DirectoryHelper.CreateScanDirectory(scanDirectory);

                ReportObject reportObject = new ReportObject();
                reportObject.ScanStartDate = DateTime.Now;
                reportObject.WsDescs = new List<WSDescriberForReport>();
                reportObject.TotalRequestCount = 0;

                Log("Scan Started: " + reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true, false);

                WSDescriberForReport WSItemVulnerabilities = new WSDescriberForReport();

                WSItemVulnerabilities.RestAPI = RestAPIDesc;
                WSItemVulnerabilities.StaticVulns = new List<StaticVulnerabilityForReport>();
                WSItemVulnerabilities.Vulns = new List<VulnerabilityForReport>();
                WSItemVulnerabilities.InfoVulns = new List<DisclosureVulnerabilityForReport>();

                Log("API Address: " + RestAPIDesc.Url.AbsoluteUri, FontStyle.Bold, true, false);
                Log("Parsing API...", FontStyle.Regular, true, false);

                List<Param> respHeader = new List<Param>();

                bool untrustedSSLSecureChannel = false;
                RestParser restParser = new RestParser(ref RestAPIDesc);

                if (chkDynamicScan.Checked)
                {
                    RestHTTPHelper HttpHelper = new RestHTTPHelper(ref RestAPIDesc, ref untrustedSSLSecureChannel, ref respHeader, CustomRequestHeader);

                    if (!RestAPIDesc.Url.Scheme.Equals("https"))
                    {
                        Log(" Vulnerability Found - SSL Not Used, Uri Schema is " + RestAPIDesc.Url.Scheme, FontStyle.Bold, true, false);
                        AddSSLRelatedVulnerability(WSItemVulnerabilities, 0);
                    }
                    else
                    {
                        if (untrustedSSLSecureChannel)
                        {
                            Log(" Vulnerability Found - Could not establish trust relationship for the SSL/TLS secure channel.", FontStyle.Bold, true, false);
                            AddSSLRelatedVulnerability(WSItemVulnerabilities, -1);
                        }
                    }

                    int paramCount = 0;
                    paramCount = RestAPIDesc.UrlParameters != null ? RestAPIDesc.UrlParameters.Count : 0;
                    paramCount += RestAPIDesc.PostParameters != null ? RestAPIDesc.PostParameters.Count : 0;

                    RestDynamicVulnerabilityScanner restDynScn = new RestDynamicVulnerabilityScanner(this);

                    foreach (VulnerabilitiesVulnerability vuln in vulnerabilities.Vulnerability)
                    {
                        if (vuln.type == 2 || vuln.type == 3) // 2: rest specific , 3: common for soap & rest
                        {
                            if (vuln.id != 0 && vuln.id != 9 && vuln.id != 10) // 0 for insecure transport - ssl not used , 9 for HTTP Options , 10 for XST
                            {
                                Log("   Testing: " + vuln.title, FontStyle.Regular, chkDebug.Checked, false);
                                Log("   Parameter Count: " + (paramCount), FontStyle.Regular, chkDebug.Checked, false);

                                try
                                {
                                    restDynScn.ScanVulnerabilities(vuln, RestAPIDesc, WSItemVulnerabilities, reportObject,
                                    chkDebug.Checked, ref respHeader, HttpHelper, CustomRequestHeader);
                                }
                                catch (Exception ex)
                                {
                                    Log("   Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                                }
                            }
                        }
                    }

                    try
                    {
                        VulnerabilitiesVulnerability optionsVuln = vulnerabilities.Vulnerability.Where(v => v.id == 9).FirstOrDefault();
                        if (optionsVuln != null)
                        {
                            restDynScn.CheckHTTPOptionsVulns(RestAPIDesc, optionsVuln, WSItemVulnerabilities, reportObject,
                                                   chkDebug.Checked, ref respHeader, HttpHelper, CustomRequestHeader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("   CheckHTTPOptionsVulns - Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                    }

                    try
                    {
                        VulnerabilitiesVulnerability xstVuln = vulnerabilities.Vulnerability.Where(v => v.id == 10).FirstOrDefault();
                        if (xstVuln != null)
                        {
                            restDynScn.CheckXSTVulns(RestAPIDesc, xstVuln, WSItemVulnerabilities, reportObject,
                                                   chkDebug.Checked, ref respHeader, HttpHelper, CustomRequestHeader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("   CheckXSTVulns - Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked, true);
                    }
                }

                if (chkInfoDisclosure.Checked)
                {
                    Log("Information Disclosure Analysis Started", FontStyle.Regular, true, false);

                    InformationDisclosureVulnerabilityScanner idvs = new InformationDisclosureVulnerabilityScanner(this);

                    foreach (InformationDisclosureVulnerability infoVuln in disclosureVulnerabilities.Vulnerability)
                    {
                        Log("   Searching Response Header: " + infoVuln.title, FontStyle.Regular, chkDebug.Checked, false);

                        string infoScanRes = idvs.ScanIt(infoVuln, respHeader);

                        if (!string.IsNullOrEmpty(infoScanRes))
                        {
                            Log("   " + infoVuln.title + " Information Disclosure Found: " + infoScanRes, FontStyle.Bold, true, false);
                            DisclosureVulnerabilityForReport vulnRep = new DisclosureVulnerabilityForReport();
                            vulnRep.Vuln = infoVuln;
                            vulnRep.Value = infoScanRes;

                            WSItemVulnerabilities.InfoVulns.Add(vulnRep);
                        }
                    }

                    Log("Information Disclosure Analysis Finished", FontStyle.Regular, true, false);
                }

                reportObject.WsDescs.Add(WSItemVulnerabilities);

                reportObject.ScanEndDate = DateTime.Now;

                Log("Scan Finished: " + reportObject.ScanEndDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true, false);

                string reportFilePath = scanDirectory + @"\Report\Report.html";
                string xmlFilePath = scanDirectory + @"\Report\Report.xml";

                //string reportTemplatePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\..\..\ReportTemplates\HTMLReportTemplate.html";

                string reportTemplatePath = System.AppDomain.CurrentDomain.BaseDirectory +
                        @"\ReportTemplates\HTMLReportTemplate.html";

                ReportHelper.CreateHTMLReport(reportObject,
                    reportTemplatePath,
                    reportFilePath, chkXMLReport.Checked, xmlFilePath);

                Process.Start(reportFilePath);
                //if (chkXMLReport.Checked) Process.Start(xmlFilePath);
            }
            else
            {
                MessageBox.Show("Please Enter API Info!!!");
            }
        }

        private void AddSSLRelatedVulnerability(WSDescriberForReport WSItemVulnerabilities, int vulnId)
        {
            VulnerabilityForReport sslVuln = new VulnerabilityForReport();
            sslVuln.Vuln = vulnerabilities.Vulnerability.Where(v => v.id == vulnId).FirstOrDefault();
            sslVuln.VulnerableMethodName = "";
            sslVuln.VulnerableParamName = "";
            sslVuln.Payload = "";
            sslVuln.Response = "";
            sslVuln.StatusCode = "";

            WSItemVulnerabilities.Vulns.Add(sslVuln);
        }

        public void Log(string str, FontStyle fontStyle, bool displayInListView, bool isError)
        {
            Logger.Log(scanDirectory + @"\Logs\", str, isError);

            if (displayInListView)
            {
                ListViewItem lvItem = new ListViewItem(str);
                lvItem.Font = new System.Drawing.Font(lvResult.Font, fontStyle);
                lvResult.Items.Add(lvItem);

                lvResult.Refresh();
                lvResult.Items[lvResult.Items.Count - 1].EnsureVisible();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            vulnerabilities = new Vulnerabilities();
            System.Xml.Serialization.XmlSerializer reader = new
               System.Xml.Serialization.XmlSerializer(vulnerabilities.GetType());

            System.IO.StreamReader file =
               new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\..\..\XML\Vulnerabilities.xml");

            vulnerabilities = (Vulnerabilities)reader.Deserialize(file);

            staticVulnerabilities = new StaticVulnerabilities();
            reader = new
               System.Xml.Serialization.XmlSerializer(staticVulnerabilities.GetType());

            file =
               new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\..\..\XML\StaticVulnerabilities.xml");

            staticVulnerabilities = (StaticVulnerabilities)reader.Deserialize(file);

            disclosureVulnerabilities = new DisclosureVulnerabilities();
            reader = new
               System.Xml.Serialization.XmlSerializer(disclosureVulnerabilities.GetType());

            file =
               new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\..\..\XML\DisclosureVulnerabilities.xml");

            disclosureVulnerabilities = (DisclosureVulnerabilities)reader.Deserialize(file);

            file.Close();
        }

        private void btnCustomSoapTags_Click(object sender, EventArgs e)
        {
            ShowCustomSoapTagsForm();
        }

        private void addCustomSoapTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomSoapTagsForm();
        }

        private void ShowCustomSoapTagsForm()
        {
            CustomSoapTagEntry frm = new CustomSoapTagEntry(CustomSoapHeaderTags, CustomSoapBodyTags);
            frm.ShowDialog(this);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }

        private void ShowAboutForm()
        {
            AboutBox about = new AboutBox();
            about.ShowDialog(this);
        }

        private void btnScanREST_Click(object sender, EventArgs e)
        {
            ShowRestFormScan();
        }

        private void scanRestServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRestFormScan();
        }

        private void ShowRestFormScan()
        {
            RESTInfoEntry frm = new RESTInfoEntry(this, RestAPIDesc);
            frm.ShowDialog(this);
        }

        private void addCustomRequestHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomRequestHeaderForm();
        }

        private void ShowCustomRequestHeaderForm()
        {
            CustomRequestHeaderEntry frm = new CustomRequestHeaderEntry(CustomRequestHeader, UserAgentHeader);
            frm.ShowDialog(this);
        }
    }
}