using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Web;
using WSSAT.DataTypes;
using System.Text.RegularExpressions;

namespace WSSAT.BusinessLayer
{
    public class WebServiceToInvoke
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public string TargetNameSpace { get; private set; }
        public List<Param> Params = new List<Param>();
        public XDocument ResponseSOAP = XDocument.Parse("<root/>");
        public string ResultString = String.Empty;
        public int StatusCode = 0;

        public WebServiceToInvoke()
        {
            Url = String.Empty;
            Method = String.Empty;
            TargetNameSpace = String.Empty;
        }
        public WebServiceToInvoke(string baseUrl)
        {
            Url = baseUrl;
            Method = String.Empty;
            TargetNameSpace = String.Empty;
        }

        public void AddParameter(string name, string value)
        {
            Params.Add(new Param() { Name = name, Value = value });
        }

        public void InvokeMethod(string methodName, string targetNameSpace, WSDescriber wsDesc, ref List<Param> respHeader)
        {
            InvokeMethod(methodName, true, targetNameSpace, wsDesc, ref respHeader);
        }

        public void CleanLastInvoke()
        {
            ResponseSOAP = null;
            //ResultXML = null;
            ResultString = Method = String.Empty;
            TargetNameSpace = string.Empty;
            StatusCode = 0;
            Params = new List<Param>();
        }

        private void AssertCanInvoke(string methodName = "")
        {
            if (Url == String.Empty)
                throw new ArgumentNullException("You tried to invoke a webservice without specifying the WebService's URL.");
            if ((methodName == "") && (Method == String.Empty))
                throw new ArgumentNullException("You tried to invoke a webservice without specifying the WebMethod.");
        }

        private void ExtractResult(string methodName, string targetNameSpace)
        {
            XmlNamespaceManager namespMan = new XmlNamespaceManager(new NameTable());
            namespMan.AddNamespace("foo", targetNameSpace);

            XElement webMethodResult = ResponseSOAP.XPathSelectElement("//foo:" + methodName + "Result", namespMan);
            if (webMethodResult.FirstNode != null && webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
            {
                ResultString = Utils.UnescapeString(Utils.RemoveNamespaces(XDocument.Parse(webMethodResult.FirstNode.ToString())).ToString());
            }
            else
            {
                if (webMethodResult.FirstNode != null)
                {
                    ResultString = Utils.UnescapeString(webMethodResult.FirstNode.ToString());
                }
            }
        }

        private void InvokeMethod(string methodName, bool encode, string targetNameSpace, WSDescriber wsDesc, ref List<Param> respHeader)
        {
            AssertCanInvoke(methodName);
            string soapStr =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                   xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <{0} xmlns=""http://tempuri.org/"">
                      {1}
                    </{0}>
                  </soap:Body>
                </soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            string strSlash = string.Empty;
            if (!targetNameSpace.EndsWith("/")) strSlash = "/";
            req.Headers.Add("SOAPAction", "\"" + targetNameSpace + strSlash + methodName + "\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            if (wsDesc != null && !string.IsNullOrEmpty(wsDesc.Username))
            {
                SetBasicAuthHeader(req, wsDesc.Username, wsDesc.Password);
            }

            using (Stream stm = req.GetRequestStream())
            {
                string postValues = "";
                foreach (Param param in Params)
                {
                    if (encode) postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.HtmlEncode(param.Name), HttpUtility.HtmlEncode(param.Value));
                    else postValues += string.Format("<{0}>{1}</{0}>", param.Name, param.Value);
                }

                soapStr = string.Format(soapStr, methodName, postValues);
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                SetHeader(resp, ref respHeader);

                StatusCode = (int)resp.StatusCode;
                using (StreamReader responseReader = new StreamReader(resp.GetResponseStream()))
                {
                    string result = responseReader.ReadToEnd();
                    ResponseSOAP = XDocument.Parse(result);

                    ExtractResult(methodName, targetNameSpace);
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    HttpWebResponse resp = (HttpWebResponse)wex.Response;

                    SetHeader(resp, ref respHeader);

                    StatusCode = (int)(resp).StatusCode;
                    ResultString = new StreamReader(wex.Response.GetResponseStream())
                              .ReadToEnd();
                }
                else
                {
                    if (wex.Status == WebExceptionStatus.Timeout)
                    {
                        ResultString = "The operation has timed out.";
                        StatusCode = 408;
                    }
                }
                ThrowIfSoapFault(ResultString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
            }
        }

        private void SetHeader(HttpWebResponse resp, ref List<Param> respHeader)
        {
            if (resp != null && resp.Headers != null && resp.Headers.Count > 0)
            {
                for (int i = 0; i < resp.Headers.Count; ++i)
                {
                    if (respHeader.Where(rh => rh.Name.ToLowerInvariant().Equals(resp.Headers.Keys[i].ToLowerInvariant()) &&
                        rh.Value.ToLowerInvariant().Equals(resp.Headers[i].ToLowerInvariant())).Count() == 0)
                    {
                        respHeader.Add(new Param() { Name = resp.Headers.Keys[i].ToLowerInvariant(), Value = resp.Headers[i].ToLowerInvariant() });
                    }
                }
            }
        }

        private static void ThrowIfSoapFault(string responseFailure)
        {
            XmlDocument doc = ExtractDocumentFromResponse(responseFailure);
            if (doc == null)
            {
                return;
            }

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace(
                 "soap",
                 "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace(
                 "m",
                 "http://schemas.microsoft.com/exchange/services/2006/messages");

            XmlNodeList faultStringNodes = doc.SelectNodes(
                      "//faultstring[1]", namespaceManager);
            XmlNodeList exceptionTypeNodes = doc.SelectNodes(
                      "//m:ExceptionType[1]", namespaceManager);
            if ((faultStringNodes.Count == 0) && (exceptionTypeNodes.Count == 0))
            {
                return;
            }

            string innerMsg = string.Empty;

            if (!string.IsNullOrEmpty(faultStringNodes[0].InnerText))
            {
                innerMsg = faultStringNodes[0].InnerText;
            }
            else if (!string.IsNullOrEmpty(exceptionTypeNodes[0].InnerText))
            {
                innerMsg = exceptionTypeNodes[0].InnerText;
            }
            throw new System.Web.Services.Protocols.SoapException(innerMsg, XmlQualifiedName.Empty);
        }

        internal static XmlDocument ExtractDocumentFromResponse(string response)
        {
            Match match = Regex.Match(response, @"<\S*:Envelope");
            if (match.Success)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Substring(match.Index));
                return doc;
            }
            return null;
        }

        public void SetBasicAuthHeader(HttpWebRequest request, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        internal void PreInvoke()
        {
            CleanLastInvoke();
        }

        internal void PosInvoke()
        {
        }
    }
}
