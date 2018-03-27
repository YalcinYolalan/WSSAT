using System;
using System.Collections.Generic;

namespace WSSAT.DataTypes
{
    public class RESTApi
    {
        public Uri Url { set; get; }
        public string Method { set; get; }
        public string PostData { set; get; }
        public string NormalizedPostData { set; get; }
        public string NormalizedURL { set; get; }
        public List<WSParameter> UrlParameters { set; get; }
        public List<WSParameter> PostParameters { set; get; }
        public string ContentType { set; get; }
        public BasicAuthentication BasicAuthentication { get; set; }
    }
}
