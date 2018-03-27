using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WSSAT.DataTypes;

namespace WSSAT.BusinessLayer
{
    public class RestParser
    {
        public RestParser(ref RESTApi restDesc)
        {
            restDesc.NormalizedURL = ParseURLParameters(ref restDesc, restDesc.Url.AbsoluteUri);
            restDesc.NormalizedPostData = ParsePOSTParameters(ref restDesc, restDesc.PostData);
        }

        private string ParsePOSTParameters(ref RESTApi restDesc, string postData)
        {
            string result = postData;
            if (!string.IsNullOrEmpty(postData))
            {
                MatchCollection coll = Regex.Matches(result, @"\$([^$]*)\$");

                if (coll != null && coll.Count > 0)
                {
                    restDesc.PostParameters = new List<WSParameter>();

                    int tmp = 0;
                    int i = 0;
                    foreach (Match m in coll)
                    {
                        WSParameter prm = new WSParameter();
                        prm.Index = i;
                        prm.Name = "POST Data";
                        prm.TypeName = GetTypeName(m.Value.Trim().Replace("$", "").ToLower());

                        string newVal = "{" + i.ToString() + "}";

                        result = result.Substring(0, m.Index - tmp) + newVal + result.Substring(m.Index + m.Length - tmp);

                        i++;
                        tmp += m.Value.Length - newVal.Length;

                        restDesc.PostParameters.Add(prm);
                    }
                }
            }
            return result;
        }

        private string ParseURLParameters(ref RESTApi restDesc, string url)
        {
            string result = url;
            if (!string.IsNullOrEmpty(url))
            {
                MatchCollection coll = Regex.Matches(result, @"\$([^$]*)\$");

                if (coll != null && coll.Count > 0)
                {
                    restDesc.UrlParameters = new List<WSParameter>();

                    int tmp = 0;
                    int i = 0;
                    foreach (Match m in coll)
                    {
                        WSParameter prm = new WSParameter();
                        prm.Index = i;
                        prm.Name = "Url";
                        prm.TypeName = GetTypeName(m.Value.Trim().Replace("$", "").ToLower());

                        string newVal = "{" + i.ToString() + "}";

                        result = result.Substring(0, m.Index - tmp) + newVal + result.Substring(m.Index + m.Length - tmp);

                        i++;
                        tmp += m.Value.Length - newVal.Length;

                        restDesc.UrlParameters.Add(prm);
                    }
                }
            }
            return result;
        }

        private string GetTypeName(string val)
        {
            if (val.Equals("int") || val.Equals("string") || val.Equals("double") ||
                val.Equals("decimal") || val.Equals("boolean") || val.Equals("bool"))
                return val;
            return "string";
        }
    }
}
