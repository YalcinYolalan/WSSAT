using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WSSAT.DataTypes;

namespace WSSAT.BusinessLayer
{
    public class RestHTTPHelper
    {
        public RestHTTPHelper() { }

        public RestHTTPHelper(ref RESTApi restDesc, ref bool untrustedSSLSecureChannel, ref List<Param> respHeader, string customRequestHeader)
        {
            HttpWebRequest wr = GetHttpWebReqWithDefaultParams(restDesc, true, customRequestHeader);

            HttpWebResponse wres = null;
            try
            {
                wres = (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.TrustFailure)
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    wr = GetHttpWebReqWithDefaultParams(restDesc, true, customRequestHeader);
                    wres = (HttpWebResponse)wr.GetResponse();

                    untrustedSSLSecureChannel = true;
                }
            }

            if (wres != null)
            {
                for (int i = 0; i < wres.Headers.Count; ++i)
                {
                    respHeader.Add(new Param() { Name = wres.Headers.Keys[i].ToLowerInvariant(), Value = wres.Headers[i].ToLowerInvariant() });
                }

                StreamReader streamReader = new StreamReader(wres.GetResponseStream());

                string response = streamReader.ReadToEnd();
            }
        }

        public HttpWebResponseWrapper GetHttpWebResponse(RESTApi restDesc, string url, string postData, bool useAuthIfExists, 
            ref List<Param> respHeader, string customRequestHeader)
        {
            HttpWebResponseWrapper resp = null;

            HttpWebRequest wr = GetHttpWebReq(restDesc, url, postData, useAuthIfExists, customRequestHeader);
            HttpWebResponse wres = null;
            try
            {
                wres = (HttpWebResponse)wr.GetResponse();
            }
            catch (Exception wex)
            {
                throw wex;
            }

            if (wres != null)
            {
                SetHeader(wres, ref respHeader);

                StreamReader streamReader = new StreamReader(wres.GetResponseStream());

                resp = new HttpWebResponseWrapper();
                resp.WebResponse = wres;
                resp.ResponseBody = streamReader.ReadToEnd();
            }

            return resp;
        }

        public HttpWebResponseWrapper GetHttpWebResponseWithDefaultParams(RESTApi restDesc, bool useAuthIfExists, ref List<Param> respHeader, string customRequestHeader)
        {
            return GetHttpWebResponse(restDesc, GetDefaultValuesForParam(restDesc.NormalizedURL, restDesc.UrlParameters, false),
                GetDefaultValuesForParam(restDesc.NormalizedPostData, restDesc.PostParameters, true), useAuthIfExists, ref respHeader, customRequestHeader);
        }

        private HttpWebRequest GetHttpWebReqWithDefaultParams(RESTApi restDesc, bool useAuthIfExists, string customRequestHeader)
        {
            return GetHttpWebReq(restDesc, GetDefaultValuesForParam(restDesc.NormalizedURL, restDesc.UrlParameters, false), 
                GetDefaultValuesForParam(restDesc.NormalizedPostData, restDesc.PostParameters, true), useAuthIfExists, customRequestHeader);
        }

        private HttpWebRequest GetHttpWebReq(RESTApi restDesc, string url, string postData, bool useAuthIfExists, string customRequestHeader)
        {
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(url);

            wr.ContentType = restDesc.ContentType;
            wr.Method = restDesc.Method;

            wr.UserAgent = MainForm.UserAgentHeader;
            //wr.Connection = "keep-alive";

            wr.Proxy = WebRequest.DefaultWebProxy;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (!string.IsNullOrEmpty(customRequestHeader))
            {
                wr.Headers.Add(customRequestHeader);
            }

            if (useAuthIfExists)
            {
                if (restDesc.BasicAuthentication != null && !string.IsNullOrEmpty(restDesc.BasicAuthentication.Username))
                {
                    wr.Credentials = new NetworkCredential(restDesc.BasicAuthentication.Username, restDesc.BasicAuthentication.Password);
                }
            }

            if (!string.IsNullOrEmpty(postData))
            {
                wr.ContentLength = Encoding.UTF8.GetByteCount(postData);
                using (Stream stream = wr.GetRequestStream())
                {
                    stream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
                    stream.Close();
                }
            }

            return wr;
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

        public string GetDefaultValuesForParam(string data, List<WSParameter> prms, bool isQuoted)
        {
            string result = data;

            if (prms != null && prms.Count > 0)
            {
                for (int i = 0; i < prms.Count; i++)
                {
                    result = result.Replace("{" + i + "}", GetDefaultValueForSingleParam(prms[i].TypeName, isQuoted));
                }
            }

            return result;
        }

        private string GetDefaultValueForSingleParam(string typeName, bool isQuoted)
        {
            switch (typeName)
            {
                case "int":
                    return DefaultValues.IntDefaultVal;
                case "string":
                    if (isQuoted)
                    {
                        return "\"" + DefaultValues.StringDefaultVal + "\"";
                    }
                    else
                    {
                        return DefaultValues.StringDefaultVal;
                    }
                case "double":
                    return DefaultValues.DoubleDefaultVal;
                case "decimal":
                    return DefaultValues.DecimalDefaultVal;
                case "boolean":
                    return DefaultValues.BooleanDefaultVal;
                case "bool":
                    return DefaultValues.BooleanDefaultVal;
                default:
                    return DefaultValues.StringDefaultVal;
            }
        }

        public HttpWebResponseWrapper GetHttpWebResponseForWebServerVuln(string host, BasicAuthentication basicAuthentication, 
            ref List<Param> respHeader, string customRequestHeader, string httpMethodName)
        {
            HttpWebResponseWrapper resp = null;

            HttpWebRequest wr = GetHttpWebReqForWebServerVuln(host, basicAuthentication, customRequestHeader, httpMethodName);
            HttpWebResponse wres = null;
            try
            {
                wres = (HttpWebResponse)wr.GetResponse();
            }
            catch (Exception wex)
            {
                throw wex;
            }

            if (wres != null)
            {
                SetHeader(wres, ref respHeader);

                StreamReader streamReader = new StreamReader(wres.GetResponseStream());

                resp = new HttpWebResponseWrapper();
                resp.WebResponse = wres;
                resp.ResponseBody = streamReader.ReadToEnd();
            }

            return resp;
        }

        private HttpWebRequest GetHttpWebReqForWebServerVuln(string host, BasicAuthentication basicAuthentication, 
            string customRequestHeader, string httpMethodName)
        {
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(host);

            //wr.ContentType = restDesc.ContentType;
            wr.Method = httpMethodName;
            wr.UserAgent = MainForm.UserAgentHeader;
            //wr.Connection = "keep-alive";

            wr.Proxy = WebRequest.DefaultWebProxy;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (!string.IsNullOrEmpty(customRequestHeader))
            {
                wr.Headers.Add(customRequestHeader);
            }

            if (basicAuthentication != null && !string.IsNullOrEmpty(basicAuthentication.Username))
            {
                wr.Credentials = new NetworkCredential(basicAuthentication.Username, basicAuthentication.Password);
            }

            return wr;
        }
    }
}
