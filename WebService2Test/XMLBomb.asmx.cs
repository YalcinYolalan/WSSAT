using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for XMLBomb
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XMLBomb : System.Web.Services.WebService
    {

        [WebMethod]
        public string LoadXML(string xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationType = ValidationType.None;
            settings.MaxCharactersFromEntities = 999999999999999999;

            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);
            while (reader.Read()) { }

            return string.Empty;
        }
    }
}
