using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WebService2Test.Helpers
{
    public class DBHelper
    {
        public SqlConnection conn = null;

        public DBHelper()
        {
            string serverIP = ConfigurationManager.AppSettings.Get("dbServerIp");
            string dbName = ConfigurationManager.AppSettings.Get("dbName");
            string userName = ConfigurationManager.AppSettings.Get("dbUserName");
            string password = ConfigurationManager.AppSettings.Get("dbPassword");
            //password = System.Text.Encoding.GetEncoding("iso-8859-9").GetString(Convert.FromBase64String(password));

            string connectionString = "server=" + serverIP + ";uid=" + userName + ";pwd=" + password + ";database=" + dbName + ";Integrated Security=True";
            //string connectionString = "server=" + serverIP + ";uid=" + userName + ";pwd=" + password + ";database=" + dbName;

            try
            {
                if (conn == null || conn.State != System.Data.ConnectionState.Open)
                {
                    conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader getDataReader(string query)
        {
            SqlDataReader dr = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = conn;
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    dr = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = query;
                throw ex;
            }
            return dr;
        }

        public DataSet getDataSet(string query)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ex.HelpLink = query;
                throw ex;
            }
            return ds;
        }

        public void closeConnection()
        {
            if (conn != null && conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public int executeNonQuery(string query)
        {
            int result = -1;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = conn;
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = query;
                throw ex;
            }
            return result;
        }
    }
}
