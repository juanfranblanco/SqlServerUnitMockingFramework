using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using UnitTest.Controllers;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void MultiMockScalarFunction(string functionName, string testName, string parameterNames, string expectedParameterValues, string parameterSepararor, string multivalueSeparator, string returnValue)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlMultiMockScalarFunctionBuilder sqlMockScalarFunctionBuilder =
                new SqlMultiMockScalarFunctionBuilder(connection, functionName, testName, parameterNames, expectedParameterValues,
                                                      parameterSepararor, multivalueSeparator, returnValue);
            string mockedFunction = sqlMockScalarFunctionBuilder.BuildMockFunction();
            SqlCommand command = new SqlCommand(mockedFunction, connection);
            SqlContext.Pipe.ExecuteAndSend(command);
        }
    }
};
