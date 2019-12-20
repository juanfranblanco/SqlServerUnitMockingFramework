using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using UnitTest.Controllers;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void AssertSelectStatement(string columnNames, string columnValues, string arraySeparator, string selectStatement)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
              connection.Open();
             AssertSelectStatementController assertSelectStatementController =
                new AssertSelectStatementController(connection, columnNames, columnValues, arraySeparator, selectStatement);
             assertSelectStatementController.DoAssertion();
             string expectations = assertSelectStatementController.ListExpectations();

             SqlCommand command = new SqlCommand("PRINT '" + expectations.Replace("'", "''") + "'", connection);
             SqlContext.Pipe.ExecuteAndSend(command);
             connection.Close();
            
        }
    }
};
