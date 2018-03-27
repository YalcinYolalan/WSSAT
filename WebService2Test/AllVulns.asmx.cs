using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using WebService2Test.DataTypes;
using WebService2Test.Helpers;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for AllVulns
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AllVulns : System.Web.Services.WebService
    {
        [WebMethod]
        public string SFTest()
        {
            throw new SoapException("Test Fault",
                SoapHeaderException.ClientFaultCode);
        }

        [WebMethod]
        public string SQLInjectMe1(string id, int x)
        {
            string result = string.Empty;

            string query = "SELECT * FROM Users WHERE Id=" + id + "";
            DBHelper db = null;
            try
            {
                db = new DBHelper();
                SqlDataReader dr = db.getDataReader(query);

                if (dr != null)
                {
                    //return username
                    if (dr.Read())
                    {
                        result = dr["UserName"].ToString();
                    }
                    dr.Close();
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                if (db != null) db.closeConnection();
            }
            return result;
        }

        [WebMethod]
        public List<CompType> MethodComplexType(List<CompType> types, string[] tmp, Books books)
        {
            return types;
        }

        //[WebMethod]
        //public string LoadXML(string xml)
        //{
        //    XmlReaderSettings settings = new XmlReaderSettings();
        //    settings.DtdProcessing = DtdProcessing.Parse;
        //    settings.ValidationType = ValidationType.None;
        //    settings.MaxCharactersFromEntities = 999999999999999999;

        //    XmlReader reader = XmlReader.Create(new StringReader(xml), settings);
        //    while (reader.Read()) { }

        //    return string.Empty;
        //}

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

        [WebService(Namespace = "http://tempuri.org/")]
        [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
        [System.ComponentModel.ToolboxItem(false)]
        // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        // [System.Web.Script.Services.ScriptService]
        public class XXE : System.Web.Services.WebService
        {

            [WebMethod]
            public string LoadExternalEntity(string xml)
            {
                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    settings.ValidationType = ValidationType.None;
                    //settings.MaxCharactersFromEntities = 999999999999999999;


                    //XmlDocument document = new XmlDocument();
                    //document.LoadXml(xml);

                    XmlReader reader = XmlReader.Create(new StringReader(xml), settings);
                    while (reader.Read()) { }

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
        }
    }
}
