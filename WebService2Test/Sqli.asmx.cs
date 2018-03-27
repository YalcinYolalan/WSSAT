using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebService2Test.Helpers;

namespace WebService2Test
{
    /// <summary>
    /// Summary description for Sqli
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Sqli : System.Web.Services.WebService
    {

        [WebMethod]
        public string SQLInjectMe1(string id, int x)
        {
            string result = string.Empty;

            string query = "SELECT * FROM Users WHERE Id=" + id + "";
            DBHelper db = null;
            try{
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

            } catch (Exception ex){
                result = ex.ToString();
            } finally {
                if (db != null) db.closeConnection();
            }
            return result;
        }


    }
}
