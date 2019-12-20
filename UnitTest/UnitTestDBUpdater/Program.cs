using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace UnitTestDBUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string message = args[0];
                string testName = args[1];
                string dbName = args[2];

                string sqlConnectionString = "Data Source=" + Environment.MachineName + ";Initial Catalog=" + dbName + ";Integrated Security=True;";
                SqlConnection updateCon = new SqlConnection(sqlConnectionString);
                using(SqlCommand sqlCommand = new SqlCommand())
                {

                    updateCon.Open();
                    sqlCommand.Connection = updateCon;
                    sqlCommand.CommandText = "tsu__private_addFailureExt";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlErrorParameter = new SqlParameter("@errorMessage", message);
                    SqlParameter sqlTestName = new SqlParameter("@test", testName);
                    sqlCommand.Parameters.Add(sqlErrorParameter);
                    sqlCommand.Parameters.Add(sqlTestName);
                    sqlCommand.ExecuteNonQuery();
                    updateCon.Close();

                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();
                if(!System.Diagnostics.EventLog.SourceExists("UnitTestLog"))
                    System.Diagnostics.EventLog.CreateEventSource(
                       "UnitTestLog", "Application");

                eventLog.Source = "UnitTestLog";
                eventLog.WriteEntry(ex.ToString());
            }
        }
    }
}
