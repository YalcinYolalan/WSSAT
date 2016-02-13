using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for SOAPFault
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SOAPFault : System.Web.Services.WebService
    {

        [WebMethod]
        public string SFTest()
        {
            throw new SoapException("Test Fault",
                SoapHeaderException.ClientFaultCode); 
        }
    }
}
