using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace UnitTest.DataAccess
{
    public class UnitTestFailureDataAccess
    {
        public static void InsertUnitTestFailure(SqlConnection connection, string message)
        {
            using(SqlCommand sqlCommand = new SqlCommand())
            {
                //connection.Open();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = "tsu_failure";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlmessageParameter = new SqlParameter("@message", message);
                sqlCommand.Parameters.Add(sqlmessageParameter);
                sqlCommand.ExecuteNonQuery();
                //connection.Close();

            }
        }
    }
}
