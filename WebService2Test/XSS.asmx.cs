using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for XSS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XSS : System.Web.Services.WebService
    {

        [WebMethod]
        public string ShowMessage(string txt)
        {
            ShowMessageOnBrowser(txt);
            return txt;
        }

        private void ShowMessageOnBrowser(string txt)
        {
            // show message on browser
        }

        [WebMethod]
        public string XSSTest2(string txt, bool a)
        {
            if (a)
            {
                return txt;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
