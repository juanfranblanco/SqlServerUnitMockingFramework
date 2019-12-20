using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Transactions;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(SystemDataAccess = SystemDataAccessKind.Read, DataAccess = DataAccessKind.Read)]
    public static SqlInt32 LogTsuFailure(string testName, string message)
    {
        SqlConnection connection = new SqlConnection("context connection=true");
        string dbName = String.Empty;

        using(SqlCommand sqlCommand = new SqlCommand())
        {
            connection.Open();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = "select db_name()";
            sqlCommand.CommandType = CommandType.Text;
            dbName = (string)sqlCommand.ExecuteScalar();
            connection.Close();
        }
            string sqlConnectionString = "Data Source=" + Environment.MachineName + ";Initial Catalog=" + dbName + ";Integrated Security=True;Pooling=True;Enlist=false";
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

        return 1;
    }
};

