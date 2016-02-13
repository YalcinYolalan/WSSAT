using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for XPATH
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XPATH : System.Web.Services.WebService
    {

        [WebMethod]
        public string LoginMeFromXMLFile(string username, string pwd)
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <Employees>
                           <Employee ID=""1"">
                              <FirstName>Arnold</FirstName>
                              <LastName>Baker</LastName>
                              <UserName>ABaker</UserName>
                              <Password>SoSecret</Password>
                              <Type>Admin</Type>
                           </Employee>
                           <Employee ID=""2"">
                              <FirstName>Peter</FirstName>
                              <LastName>Pan</LastName>
                              <UserName>PPan</UserName>
                              <Password>NotTelling</Password>
                              <Type>User</Type>
                           </Employee>
                        </Employees>";


            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            string xpathExpr = "//Employee[UserName/text()='" + username + "' and Password/text()='" + pwd + "']";

            XmlNode node = document.SelectSingleNode(xpathExpr);

            if (node != null)
            {
                return "Login Success";
            }
            else
            {
                return "Username or password is incorrect";
            }
        }
    }
}
