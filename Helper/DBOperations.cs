
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using GAIN.Controllers;
using MySql.Data.MySqlClient;


namespace GAIN.Helper

{
    public class DBOperations
    {
        string conn;
        public DBOperations()
        {
            conn = clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]);
            conn = conn.Substring(conn.IndexOf("server"));
            conn = conn.Replace("'", "");
        }

        public string CallSaveInitiativesSP(string strSPName, string jsonInitiatives, int initYear)
        {
            string strMessage = "";
            using (MySqlConnection mySql = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand(strSPName, mySql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = strSPName;
                    cmd.Parameters.Add(new MySqlParameter("initiatives", jsonInitiatives));
                    cmd.Parameters.Add(new MySqlParameter("inYear", initYear));
                    mySql.Open();
                    cmd.ExecuteNonQuery();
                    mySql.Close();
                }
            }

            return strMessage;
        }

    }
}