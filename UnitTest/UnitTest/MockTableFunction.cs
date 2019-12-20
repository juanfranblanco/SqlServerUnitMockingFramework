using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using UnitTest.Controllers;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void MockTableFunction(string functionName, string testName, string parameterNames, string expectedParameterValues, string arraySeparator, string tableReturnColumnNames, string tableReturnColumnValues)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlMockTableFunctionBuilder sqlMockTableFunctionBuilder =
                new SqlMockTableFunctionBuilder(connection, functionName, testName, parameterNames,
                                                 expectedParameterValues, arraySeparator, tableReturnColumnNames, tableReturnColumnValues);

            string mockedFunction = sqlMockTableFunctionBuilder.BuildMockFunction();

            SqlCommand command = new SqlCommand(mockedFunction, connection);
            SqlContext.Pipe.ExecuteAndSend(command);
        }

    }
};
