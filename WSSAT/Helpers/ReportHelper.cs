using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WSSAT.DataTypes;

namespace WSSAT.Helpers
{
    public class ReportHelper
    {
        public static void CreateHTMLReport(ReportObject reportObject, string templateFilePath, string pathToSave, 
            bool alsoCreateAnXMLReport, string xmlFilePath)
        {
            StreamReader reader = new StreamReader(templateFilePath);
            StringBuilder htmlContent = new StringBuilder(reader.ReadToEnd());
            reader.Close();
            
            htmlContent = htmlContent.Replace("{ScanDuration}", reportObject.Duration.ToString("N2"));
            htmlContent = htmlContent.Replace("{ScanStartDate}", reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss"));
            htmlContent = htmlContent.Replace("{TotalRequestCount}", reportObject.TotalRequestCount.ToString());
            string xmlReportPath = "-";
            if (alsoCreateAnXMLReport)
            {
                xmlReportPath = "<a href='Report.xml' target='_blank'>Report.xml</a>";
            }
            htmlContent = htmlContent.Replace("{XMLReportPath}", xmlReportPath);

            double totalStaticVulnCount = 0;
            double totalDynamicVulnCount = 0;
            double totalInfoVulnCount = 0;
            double totalVulnCount = 0;

            StringBuilder htmlVulns = new StringBuilder();
            List<Param> allVulns = new List<Param>();

            XmlDocument reportXMLDoc = null;
            reportXMLDoc = new XmlDocument();
            XmlNode docNode = reportXMLDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            reportXMLDoc.AppendChild(docNode);

            XmlNode root = reportXMLDoc.CreateElement("WSSATReport");
            root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "ScanDuration", reportObject.Duration.ToString("N2")));
            root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "ScanStartDate", reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss")));
            root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "TotalRequestCount", reportObject.TotalRequestCount.ToString()));

            foreach (WSDescriberForReport wsDesc in reportObject.WsDescs)
            {
                htmlVulns.Append(GetHtmlRowForWsdl(wsDesc, ref allVulns));
                if (alsoCreateAnXMLReport)
                {
                    root.AppendChild(GetXMLForWS(wsDesc, ref reportXMLDoc));
                }

                totalDynamicVulnCount += wsDesc.Vulns.Count;
                totalStaticVulnCount += wsDesc.StaticVulns.Count;
                totalInfoVulnCount += wsDesc.InfoVulns.Count;
            }

            totalVulnCount = totalStaticVulnCount + totalDynamicVulnCount
                + totalInfoVulnCount;

            htmlContent = htmlContent.Replace("{TotalVulnCount}", totalVulnCount.ToString());
            htmlContent = htmlContent.Replace("{Content}", htmlVulns.ToString());

            if (alsoCreateAnXMLReport)
            {
                root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "TotalVulnCount", totalVulnCount.ToString()));
                root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "TotalDynamicVulnCount", totalDynamicVulnCount.ToString()));
                root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "TotalStaticVulnCount", totalStaticVulnCount.ToString()));
                root.Attributes.Append(GetXMLAttribute(reportXMLDoc, "TotalInfoVulnCount", totalInfoVulnCount.ToString()));

                reportXMLDoc.AppendChild(root);
                reportXMLDoc.Save(xmlFilePath);
            }

            string chart1Data = "['Static (" + totalStaticVulnCount + ")', " + totalStaticVulnCount + 
                "], ['Dynamic (" + totalDynamicVulnCount + ")', " + totalDynamicVulnCount +
                "], ['Information (" + totalInfoVulnCount + ")', " + totalInfoVulnCount + "]";

            string chart2Data = string.Empty;
            int criticalVulnCount = 0, highVulnCount = 0, mediumVulnCount = 0, lowVulnCount = 0, infoVulnCount = 0;
            foreach (Param param in allVulns)
            {
                chart2Data += "['" + param.Name + " (" + param.Value + ")', " + param.Value + "],";
                switch (param.Severity)
                {
                    case "Critical": criticalVulnCount += int.Parse(param.Value); break;
                    case "High": highVulnCount += int.Parse(param.Value); break;
                    case "Medium": mediumVulnCount += int.Parse(param.Value); break;
                    case "Low": lowVulnCount += int.Parse(param.Value); break;
                    case "Info": infoVulnCount += int.Parse(param.Value); break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(chart2Data))
            {
                chart2Data = chart2Data.Remove(chart2Data.Length - 1); // remove last ","
            }

            string chart3Data = "['Critical (" + criticalVulnCount + ")', " + criticalVulnCount + 
                "],['High (" + highVulnCount + ")', " + highVulnCount + 
                "], ['Medium (" + mediumVulnCount + ")', " + mediumVulnCount +
                "], ['Low (" + lowVulnCount + ")', " + lowVulnCount +
                "], ['Info (" + infoVulnCount + ")', " + infoVulnCount + "]";

            htmlContent = htmlContent.Replace("{ChartData1}", chart1Data);
            htmlContent = htmlContent.Replace("{ChartData2}", chart2Data);
            htmlContent = htmlContent.Replace("{ChartData3}", chart3Data);

            File.WriteAllText(pathToSave, htmlContent.ToString());
        }

        private static XmlNode GetXMLForWS(WSDescriberForReport wsDesc, ref XmlDocument doc)
        {
            XmlNode wsNode = doc.CreateElement("WebService");

            string url = string.Empty;
            string postData = string.Empty;
            string method = string.Empty;
            string contentType = string.Empty;
            if (wsDesc.WsDesc != null)
            {
                url = wsDesc.WsDesc.WSDLAddress;
            }
            else
            {
                url = wsDesc.RestAPI.Url.AbsoluteUri;
                if (!string.IsNullOrEmpty(wsDesc.RestAPI.PostData)) postData = wsDesc.RestAPI.PostData;
                method = wsDesc.RestAPI.Method;
                contentType = wsDesc.RestAPI.ContentType;
            }

            wsNode.Attributes.Append(GetXMLAttribute(doc, "Url", url));
            wsNode.Attributes.Append(GetXMLAttribute(doc, "VulnCount", (wsDesc.StaticVulns.Count + wsDesc.Vulns.Count + wsDesc.InfoVulns.Count).ToString()));
            if (!string.IsNullOrEmpty(postData)) wsNode.Attributes.Append(GetXMLAttribute(doc, "PostData", postData));
            if (!string.IsNullOrEmpty(method)) wsNode.Attributes.Append(GetXMLAttribute(doc, "Method", method));
            if (!string.IsNullOrEmpty(contentType)) wsNode.Attributes.Append(GetXMLAttribute(doc, "ContentType", contentType));

            XmlNode vulnerabilitiesNode = doc.CreateElement("Vulnerabilities");

            XmlNode staticVulnerabilitiesNode = doc.CreateElement("Static");
            foreach (StaticVulnerabilityForReport vuln in wsDesc.StaticVulns)
            {
                XmlNode vulnNode = doc.CreateElement("Vulnerability");
                vulnNode.AppendChild(GetXMLNode(doc, "Title", vuln.Vuln.title));
                vulnNode.AppendChild(GetXMLNode(doc, "Severity", vuln.Vuln.severity));
                vulnNode.AppendChild(GetXMLNode(doc, "Line", System.Web.HttpUtility.HtmlEncode(vuln.XMLLine)));
                vulnNode.AppendChild(GetXMLNode(doc, "Description", System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description)));
                vulnNode.AppendChild(GetXMLNode(doc, "Link", vuln.Vuln.link));

                staticVulnerabilitiesNode.AppendChild(vulnNode);
            }

            vulnerabilitiesNode.AppendChild(staticVulnerabilitiesNode);

            XmlNode dynamicVulnerabilitiesNode = doc.CreateElement("Dynamic");
            foreach (VulnerabilityForReport vuln in wsDesc.Vulns)
            {
                XmlNode vulnNode = doc.CreateElement("Vulnerability");

                vulnNode.AppendChild(GetXMLNode(doc, "Title", vuln.Vuln.title));
                vulnNode.AppendChild(GetXMLNode(doc, "Severity", vuln.Vuln.severity));

                if (wsDesc.WsDesc != null)
                {
                    vulnNode.AppendChild(GetXMLNode(doc, "MethodName", System.Web.HttpUtility.HtmlEncode(vuln.VulnerableMethodName)));
                    vulnNode.AppendChild(GetXMLNode(doc, "ParameterName", System.Web.HttpUtility.HtmlEncode(vuln.VulnerableParamName)));
                }
                else
                {
                    vulnNode.AppendChild(GetXMLNode(doc, "UrlOrPostData", System.Web.HttpUtility.HtmlEncode(vuln.VulnerableMethodName)));
                }

                vulnNode.AppendChild(GetXMLCDataNode(doc, "Payload", vuln.Payload));
                vulnNode.AppendChild(GetXMLNode(doc, "StatusCode", System.Web.HttpUtility.HtmlEncode(vuln.StatusCode)));
                vulnNode.AppendChild(GetXMLCDataNode(doc, "Response", System.Web.HttpUtility.HtmlEncode(vuln.Response)));
                vulnNode.AppendChild(GetXMLNode(doc, "Description", System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description)));
                vulnNode.AppendChild(GetXMLNode(doc, "Link", vuln.Vuln.link));

                dynamicVulnerabilitiesNode.AppendChild(vulnNode);
            }

            vulnerabilitiesNode.AppendChild(dynamicVulnerabilitiesNode);

            XmlNode infoVulnerabilitiesNode = doc.CreateElement("Informational");
            foreach (DisclosureVulnerabilityForReport vuln in wsDesc.InfoVulns)
            {
                XmlNode vulnNode = doc.CreateElement("Vulnerability");

                vulnNode.AppendChild(GetXMLNode(doc, "Title", vuln.Vuln.title));
                vulnNode.AppendChild(GetXMLNode(doc, "Severity", vuln.Vuln.severity));
                vulnNode.AppendChild(GetXMLNode(doc, "Value", System.Web.HttpUtility.HtmlEncode(vuln.Value)));
                vulnNode.AppendChild(GetXMLNode(doc, "Description", System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description)));
                vulnNode.AppendChild(GetXMLNode(doc, "Link", vuln.Vuln.link));

                infoVulnerabilitiesNode.AppendChild(vulnNode);
            }

            vulnerabilitiesNode.AppendChild(infoVulnerabilitiesNode);

            wsNode.AppendChild(vulnerabilitiesNode);
            return wsNode;
        }

        private static string GetHtmlRowForWsdl(WSDescriberForReport wsDesc, ref List<Param> allVulns)
        {
            StringBuilder result = new StringBuilder();

            result.Append("<table style='border-style:solid;width:100%' id='tbl4' class='wsdl'>");
            if (wsDesc.WsDesc != null)
            {
                result.Append("<tr class='accordion'><td><b>WSDL Address: " + wsDesc.WsDesc.WSDLAddress + "</b>&nbsp;&nbsp;Vulnerability Count:" + (wsDesc.StaticVulns.Count + wsDesc.Vulns.Count + wsDesc.InfoVulns.Count).ToString() + "</td><td><div class='arrow'></div></td></tr>");
            }
            else
            {
                string postData = string.Empty;
                if (!string.IsNullOrEmpty(wsDesc.RestAPI.PostData)) postData = "&nbsp;&nbsp;Post Data:" + wsDesc.RestAPI.PostData;
                result.Append("<tr class='accordion'><td><b>API Address: " + wsDesc.RestAPI.Url.AbsoluteUri + "</b>" + postData + "&nbsp;&nbsp;Vulnerability Count:" + (wsDesc.StaticVulns.Count + wsDesc.Vulns.Count + wsDesc.InfoVulns.Count).ToString() + "</td><td><div class='arrow'></div></td></tr>");
            }

            foreach (StaticVulnerabilityForReport vuln in wsDesc.StaticVulns)
            {
                result.Append("<tr><td colspan='2'><ul>");
                result.Append("<li style='text-align:center;color:maroon;font-size:14px;'>" + vuln.Vuln.title + " - Severity:<b>" + vuln.Vuln.severity + "</b> - <i>Scan Type: Static</i></li>");
                result.Append("<li>Line:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.XMLLine) + "</pre></b></li>");
                result.Append("<li><b>Description:</b><br /><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description) + "</pre><br /><a href='" + vuln.Vuln.link + "' target='_blank'>Vulnerability Details</a></li>");
                result.Append("</ul></td></tr>");
                result.Append("<tr><td><hr /></td></tr>");

                AddToAllVuln("Stat" + vuln.Vuln.id, ref allVulns, vuln.Vuln.title, vuln.Vuln.severity);
            }

            foreach (VulnerabilityForReport vuln in wsDesc.Vulns)
            {
                result.Append("<tr><td colspan='2'><ul>");
                result.Append("<li style='text-align:center;color:maroon;font-size:14px;'>" + vuln.Vuln.title + " - Severity:<b>" + vuln.Vuln.severity + "</b> - <i>Scan Type: Dynamic</i></li>");
                if (wsDesc.WsDesc != null)
                {
                    result.Append("<li>Method Name:<b>" + System.Web.HttpUtility.HtmlEncode(vuln.VulnerableMethodName) + "</b></li>");
                    result.Append("<li>Parameter Name:<b>" + System.Web.HttpUtility.HtmlEncode(vuln.VulnerableParamName) + "</b></li>");
                }
                else
                {
                    result.Append("<li>URL/Post Data:<b>" + System.Web.HttpUtility.HtmlEncode(vuln.VulnerableMethodName) + "</b></li>");
                }
                result.Append("<li>Payload:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Payload) + "</pre></b></li>");
                result.Append("<li>Status Code:<br /><b><pre>" + vuln.StatusCode + "</pre></b></li>");
                result.Append("<li>Response:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Response) + "</pre></b></li>");
                result.Append("<li><b>Description:</b><br /><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description) + "</pre><br /><a href='" + vuln.Vuln.link + "' target='_blank'>Vulnerability Details</a></li>");
                result.Append("</ul></td></tr>");
                result.Append("<tr><td><hr /></td></tr>");

                AddToAllVuln("Dyn" + vuln.Vuln.id, ref allVulns, vuln.Vuln.title, vuln.Vuln.severity);
            }

            foreach (DisclosureVulnerabilityForReport vuln in wsDesc.InfoVulns)
            {
                result.Append("<tr><td colspan='2'><ul>");
                result.Append("<li style='text-align:center;color:maroon;font-size:14px;'>" + vuln.Vuln.title + " - Severity:<b>" + vuln.Vuln.severity + "</b> - <i>Scan Type: Information Disclosure</i></li>");
                result.Append("<li>Value:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Value) + "</pre></b></li>");
                result.Append("<li><b>Description:</b><br /><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description) + "</pre><br /><a href='" + vuln.Vuln.link + "' target='_blank'>Vulnerability Details</a></li>");
                result.Append("</ul></td></tr>");
                result.Append("<tr><td><hr /></td></tr>");

                AddToAllVuln("Info" + vuln.Vuln.id, ref allVulns, vuln.Vuln.title, vuln.Vuln.severity);
            }

            result.Append("</table>");

            return result.ToString();
        }

        private static void AddToAllVuln(string vulnId, ref List<Param> allVulns, string vulnName, string severity)
        {
            //Param existingVuln = allVulns.Where(v => v.Id.Equals(vulnId)).FirstOrDefault();
            if (allVulns.Where(v => v.Id.Equals(vulnId)).FirstOrDefault() != null)
            {
                allVulns.Where(v => v.Id.Equals(vulnId)).FirstOrDefault().Value = (int.Parse(allVulns.Where(v => v.Id.Equals(vulnId)).FirstOrDefault().Value) + 1).ToString();
            }
            else
            {
                allVulns.Add(new Param() { Id = vulnId, Name = vulnName, Severity = severity, Value = "1"});
            }
        }

        private static XmlElement GetXMLNode(XmlDocument doc, string name, string val)
        {
            XmlElement node = doc.CreateElement(name);
            //node.Value = val;
            node.AppendChild(doc.CreateTextNode(val));
            return node;
        }

        private static XmlElement GetXMLCDataNode(XmlDocument doc, string name, string val)
        {
            XmlElement node = doc.CreateElement(name);
            //node.Value = val;
            node.AppendChild(doc.CreateCDataSection(val));
            return node;
        }

        private static XmlAttribute GetXMLAttribute(XmlDocument doc, string name, string value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value;
            return att;
        }
    }
}
