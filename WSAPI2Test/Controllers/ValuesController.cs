using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WSAPI2Test.Helpers;
using WSAPI2Test.Models;

namespace WSAPI2Test.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/5
        [HttpGet]
        public string GetUser(int? id)
        {
            string result = string.Empty;

            string query = "SELECT * FROM Users WHERE Id=" + id;
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

        // GET api/xss?txt=<script>alert</script>
        [HttpGet]
        [Route("api/xss")]
        public string ShowMessage(string txt)
        {
            ShowMessageOnBrowser(txt);
            return txt;
        }

        private void ShowMessageOnBrowser(string txt)
        {
            // show message on browser
        }

        [HttpPost]
        [Route("api/values/save")]
        public string CreateUser(UserModel user)
        {
            // Create User Object
            return "value create: " + user.Name;
        }

        [HttpPost]
        [Route("api/values/saveuser")]
        public string CreateUser(UserModel2 user)
        {
            // Create User Object
            return "value create: " + user.Name + " - age: " + user.Age + " - cars[0]: " + user.Cars[0];
        }

        // PUT api/values/update?Id=1
        [HttpPut]
        [Route("api/values/update")]
        public string UpdateUser(int Id, UserModel user)
        {
            // Update User
            return "value update: " + Id + "-" + user.Name;
        }

        // DELETE api/values/delete?Id=2
        [HttpDelete]
        [Route("api/values/delete")]
        public string DeleteUser(int Id, UserModel user)
        {
            // Delete User
            return "value delete: " + Id + "-" + user.Name;
        }
    }
}
