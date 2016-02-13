using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebService2Test.DataTypes;
using System.Web.Services.Protocols;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for WeakXMLTest2
    /// </summary>
    [WebServiceAttribute(Namespace = "http://www.wstest.com/WebServices")]
    [WebServiceBindingAttribute("AYS20Aug2002", "http://www.wstest.com/WebServices/ExternalWSDL")]

    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1, Namespace = "http://www.wstest.com/WebServices")]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WeakXMLTest2 : System.Web.Services.WebService
    {

        [WebMethodAttribute()]
        [SoapDocumentMethodAttribute(Binding = "AYS20Aug2002")]
        public Books HelloWorld(List<CompType> types, Books books)
        {
            return books;
        }
    }
}
