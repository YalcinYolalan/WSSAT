using System;
using System.Collections.Generic;
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

        public MainForm()
        {
            InitializeComponent();
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
                                    wsDesc.Username = arr[1].Trim();
                                    wsDesc.Password = arr[2].Trim();
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
                lblSelectedFileName.Text = openFileDialog1.FileName;
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

                Log("Scan Started: " + reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true);

                foreach (WSDescriber wsDesc in services)
                {
                    WSDescriberForReport WSItemVulnerabilities = new WSDescriberForReport();

                    WSItemVulnerabilities.WsDesc = wsDesc;
                    WSItemVulnerabilities.Vulns = new List<VulnerabilityForReport>();
                    WSItemVulnerabilities.StaticVulns = new List<StaticVulnerabilityForReport>();

                    Log("WSDL Address: " + wsDesc.WSDLAddress, FontStyle.Bold, true);
                    Log("Parsing WSDL...", FontStyle.Regular, true);

                    Parser parser = new Parser(wsDesc);

                    if (chkStaticScan.Checked)
                    {
                        Log("Static Analysis Started", FontStyle.Regular, true);

                        StaticVulnerabilityScanner svs = new StaticVulnerabilityScanner();

                        foreach (StaticVulnerabilitiesStaticVulnerability staticVuln in staticVulnerabilities.StaticVulnerability)
                        {
                            Log("   Testing: " + staticVuln.title, FontStyle.Regular, chkDebug.Checked);

                            string staticScanRes = svs.ScanIt(staticVuln, parser.rawWSDL, WSItemVulnerabilities);

                            if (!string.IsNullOrEmpty(staticScanRes))
                            {
                                Log("   " + staticVuln.title + " Vulnerability Found: " + staticScanRes, FontStyle.Bold, true);
                                StaticVulnerabilityForReport vulnRep = new StaticVulnerabilityForReport();
                                vulnRep.Vuln = staticVuln;
                                vulnRep.XMLLine = staticScanRes;

                                WSItemVulnerabilities.StaticVulns.Add(vulnRep);
                            }
                        }

                        Log("Static Analysis Finished", FontStyle.Regular, true);
                    }

                    if (chkDynamicScan.Checked)
                    {
                        Log("Getting Methods...", FontStyle.Regular, true);
                        List<WSOperation> operations = parser.GetOperations();

                        WebServiceToInvoke wsInvoker = new WebServiceToInvoke(wsDesc.WSDLAddress.Replace("?WSDL", ""));

                        if (!wsDesc.WSUri.Scheme.Equals("https"))
                        {
                            Log(" Vulnerability Found - SSL Not Used, Uri Schema is " + wsDesc.WSUri.Scheme, FontStyle.Bold, true);
                            VulnerabilityForReport sslVuln = new VulnerabilityForReport();
                            sslVuln.Vuln = vulnerabilities.Vulnerability.Where(v => v.id == 0).FirstOrDefault();
                            sslVuln.VulnerableMethodName = "";
                            sslVuln.VulnerableParamName = "";
                            sslVuln.Payload = "";
                            sslVuln.Response = "";
                            sslVuln.StatusCode = "";

                            WSItemVulnerabilities.Vulns.Add(sslVuln);
                        }

                        DynamicVulnerabilityScanner dynScn = new DynamicVulnerabilityScanner(this);

                        foreach (WSOperation operation in operations)
                        {
                            Log("Method: " + operation.MethodName, FontStyle.Regular, chkDebug.Checked);

                            foreach (VulnerabilitiesVulnerability vuln in vulnerabilities.Vulnerability)
                            {
                                if (vuln.id != 0 && vuln.id != 7) // 0 for insecure transport - ssl not used , 7 for verbose soap fault message
                                {
                                    wsInvoker.PreInvoke();

                                    Log("   Testing: " + vuln.title, FontStyle.Regular, chkDebug.Checked);
                                    Log("   Parameter Count: " + operation.Parameters.Count, FontStyle.Regular, chkDebug.Checked);

                                    try
                                    {
                                        dynScn.ScanVulnerabilities(wsInvoker, operation, vuln, parser.TargetNameSpace, wsDesc, WSItemVulnerabilities, reportObject, chkDebug.Checked);
                                    }
                                    catch (System.Web.Services.Protocols.SoapException soapEx)
                                    {
                                        dynScn.SetSoapFaultException(operation, soapEx, WSItemVulnerabilities, chkDebug.Checked);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log("   Exception: " + ex.ToString(), FontStyle.Regular, chkDebug.Checked);
                                    }
                                }
                            }
                        }
                    }
                    reportObject.WsDescs.Add(WSItemVulnerabilities);
                }

                reportObject.ScanEndDate = DateTime.Now;

                Log("Scan Finished: " + reportObject.ScanEndDate.ToString("dd.MM.yyyy HH:mm:ss"), FontStyle.Bold, true);

                string reportFilePath = scanDirectory + @"\Report\Report.html";

                ReportHelper.CreateHTMLReport(reportObject, 
                    System.AppDomain.CurrentDomain.BaseDirectory + @"\ReportTemplates\HTMLReportTemplate.html",
                    reportFilePath);

                Process.Start(reportFilePath);
            }
            else
            {
                MessageBox.Show("Please Select WSDL List File!!!");
            }
        }

        public void Log(string str, FontStyle fontStyle, bool displayInListView)
        {
            Logger.Log(scanDirectory + @"\Logs\", str);

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
            vulnerabilities = new Vulnerabilities();
            System.Xml.Serialization.XmlSerializer reader = new
               System.Xml.Serialization.XmlSerializer(vulnerabilities.GetType());

            System.IO.StreamReader file =
               new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\XML\Vulnerabilities.xml");

            vulnerabilities = (Vulnerabilities)reader.Deserialize(file);

            staticVulnerabilities = new StaticVulnerabilities();
            reader = new
               System.Xml.Serialization.XmlSerializer(staticVulnerabilities.GetType());

            file =
               new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\XML\StaticVulnerabilities.xml");

            staticVulnerabilities = (StaticVulnerabilities)reader.Deserialize(file);

            file.Close();
        }
    }
}