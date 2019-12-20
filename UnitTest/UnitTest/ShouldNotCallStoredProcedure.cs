using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using UnitTest.Controllers;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ShouldNotCallStoredProcedure(string procedureName)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlShouldNotCallStoredProcedureBuilder sqlShouldNotCallStoredProcedureBuilder =
                new SqlShouldNotCallStoredProcedureBuilder(connection, procedureName);
            string mockedProcedure = sqlShouldNotCallStoredProcedureBuilder.BuildStoredProcedure();
            SqlCommand command = new SqlCommand(mockedProcedure, connection);
            SqlContext.Pipe.ExecuteAndSend(command);
        }
    }
};
