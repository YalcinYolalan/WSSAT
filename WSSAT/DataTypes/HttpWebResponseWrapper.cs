using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WSSAT.DataTypes
{
    public class HttpWebResponseWrapper
    {
        public HttpWebResponse WebResponse { set; get; }
        public string ResponseBody { set; get; }
    }
}
