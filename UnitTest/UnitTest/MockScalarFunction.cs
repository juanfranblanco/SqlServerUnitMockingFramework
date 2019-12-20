using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using UnitTest.Controllers;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void MockScalarFunction(string functionName, string testName, string parameterNames, string expectedParameterValues, string parameterSepararor, string returnValue)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlMockScalarFunctionBuilder sqlMockScalarFunctionBuilder =
                new SqlMockScalarFunctionBuilder(connection, functionName, testName, parameterNames,
                                                 expectedParameterValues, parameterSepararor, returnValue);

            string mockedFunction = sqlMockScalarFunctionBuilder.BuildMockFunction();

            SqlCommand command = new SqlCommand(mockedFunction, connection);
            SqlContext.Pipe.ExecuteAndSend(command);
            string commandInfo = "PRINT '" + sqlMockScalarFunctionBuilder.GetInfo().Replace("'", "''") + "'";

            command = new SqlCommand(commandInfo, connection);
            SqlContext.Pipe.ExecuteAndSend(command);
        }

    }


};

