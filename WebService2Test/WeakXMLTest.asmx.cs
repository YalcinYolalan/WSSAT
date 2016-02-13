using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebService2Test.DataTypes;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for WeakXMLTest
    /// </summary>
    [WebService(Namespace = "http://www.wstest.com/WeakXMLTest2")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WeakXMLTest : System.Web.Services.WebService
    {

        [WebMethod]
        public List<CompType> MethodComplexType(List<CompType> types, string[] tmp, Books books)
        {
            return types;
        }
    }
}
