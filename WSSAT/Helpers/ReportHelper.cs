using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSSAT.DataTypes;

namespace WSSAT.Helpers
{
    public class ReportHelper
    {
        public static void CreateHTMLReport(ReportObject reportObject, string templateFilePath, string pathToSave)
        {
            StreamReader reader = new StreamReader(templateFilePath);
            StringBuilder htmlContent = new StringBuilder(reader.ReadToEnd());
            reader.Close();
            
            htmlContent = htmlContent.Replace("{ScanDuration}", reportObject.Duration.ToString("N2"));
            htmlContent = htmlContent.Replace("{ScanStartDate}", reportObject.ScanStartDate.ToString("dd.MM.yyyy HH:mm:ss"));
            htmlContent = htmlContent.Replace("{TotalRequestCount}", reportObject.TotalRequestCount.ToString());

            double totalStaticVulnCount = 0;
            double totalDynamicVulnCount = 0;
            double totalVulnCount = 0;

            StringBuilder htmlVulns = new StringBuilder();
            List<Param> allVulns = new List<Param>();
            
            foreach (WSDescriberForReport wsDesc in reportObject.WsDescs)
            {
                htmlVulns.Append(GetHtmlRowForWsdl(wsDesc, ref allVulns));
                totalDynamicVulnCount += wsDesc.Vulns.Count;
                totalStaticVulnCount += wsDesc.StaticVulns.Count;
            }

            totalVulnCount = totalStaticVulnCount + totalDynamicVulnCount;

            htmlContent = htmlContent.Replace("{TotalVulnCount}", totalVulnCount.ToString());
            htmlContent = htmlContent.Replace("{Content}", htmlVulns.ToString());

            string chart1Data = "['Static (" + totalStaticVulnCount + ")', " + totalStaticVulnCount + "], ['Dynamic (" + totalDynamicVulnCount + ")', " + totalDynamicVulnCount + "]";

            string chart2Data = string.Empty;
            int criticalVulnCount = 0, highVulnCount = 0, mediumVulnCount = 0, lowVulnCount = 0;
            foreach (Param param in allVulns)
            {
                chart2Data += "['" + param.Name + " (" + param.Value + ")', " + param.Value + "],";
                switch (param.Severity)
                {
                    case "Critical": criticalVulnCount += int.Parse(param.Value); break;
                    case "High": highVulnCount += int.Parse(param.Value); break;
                    case "Medium": mediumVulnCount += int.Parse(param.Value); break;
                    case "Low": lowVulnCount += int.Parse(param.Value); break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(chart2Data))
            {
                chart2Data = chart2Data.Remove(chart2Data.Length - 1); // remove last ","
            }

            string chart3Data = "['Critical (" + criticalVulnCount + ")', " + criticalVulnCount + "],['High (" + highVulnCount + ")', " + highVulnCount + "], ['Medium (" + mediumVulnCount + ")', " + mediumVulnCount + "], ['Low (" + lowVulnCount + ")', " + lowVulnCount + "]";

            htmlContent = htmlContent.Replace("{ChartData1}", chart1Data);
            htmlContent = htmlContent.Replace("{ChartData2}", chart2Data);
            htmlContent = htmlContent.Replace("{ChartData3}", chart3Data);

            File.WriteAllText(pathToSave, htmlContent.ToString());
        }

        private static string GetHtmlRowForWsdl(WSDescriberForReport wsDesc, ref List<Param> allVulns)
        {
            StringBuilder result = new StringBuilder();

            result.Append("<table style='border-style:solid;width:100%' id='tbl4' class='wsdl'>");
            result.Append("<tr class='accordion'><td><b>WSDL Address: " + wsDesc.WsDesc.WSDLAddress + "</b>&nbsp;&nbsp;Vulnerability Count:" + (wsDesc.StaticVulns.Count + wsDesc.Vulns.Count).ToString() + "</td><td><div class='arrow'></div></td></tr>");

            foreach (StaticVulnerabilityForReport vuln in wsDesc.StaticVulns)
            {
                result.Append("<tr><td colspan='2'><ul>");
                result.Append("<li>" + vuln.Vuln.title + " - Severity:<b>" + vuln.Vuln.severity + "</b> - <i>Scan Type: Static</i></li>");
                result.Append("<li>Line:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.XMLLine) + "</pre></b></li>");
                result.Append("<li><b>Description:</b><br /><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description) + "</pre><br /><a href='" + vuln.Vuln.link + "' target='_blank'>Vulnerability Details</a></li>");
                result.Append("</ul></td></tr>");
                result.Append("<tr><td><hr /></td></tr>");

                AddToAllVuln("Stat" + vuln.Vuln.id, ref allVulns, vuln.Vuln.title, vuln.Vuln.severity);
            }

            foreach (VulnerabilityForReport vuln in wsDesc.Vulns)
            {
                result.Append("<tr><td colspan='2'><ul>");
                result.Append("<li>" + vuln.Vuln.title + " - Severity:<b>" + vuln.Vuln.severity + "</b> - <i>Scan Type: Dynamic</i></li>");
                result.Append("<li>Method Name:<b>" + vuln.VulnerableMethodName + "</b></li>");
                result.Append("<li>Parameter Name:<b>" + vuln.VulnerableParamName + "</b></li>");
                result.Append("<li>Payload:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Payload) + "</pre></b></li>");
                result.Append("<li>Status Code:<br /><b><pre>" + vuln.StatusCode + "</pre></b></li>");
                result.Append("<li>Response:<br /><b><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Response) + "</pre></b></li>");
                result.Append("<li><b>Description:</b><br /><pre>" + System.Web.HttpUtility.HtmlEncode(vuln.Vuln.description) + "</pre><br /><a href='" + vuln.Vuln.link + "' target='_blank'>Vulnerability Details</a></li>");
                result.Append("</ul></td></tr>");
                result.Append("<tr><td><hr /></td></tr>");

                AddToAllVuln("Dyn" + vuln.Vuln.id, ref allVulns, vuln.Vuln.title, vuln.Vuln.severity);
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
    }
}
