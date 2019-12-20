using System.Collections.Generic;
using System.Data.SqlClient;
using UnitTest.DataAccess;

namespace UnitTest.Controllers
{
    public abstract class SqlMockScalarFunctionBuilderBase
    {
        protected SqlConnection connection;
        protected string functionName;
        protected string testName;
        protected string parameterNames;
        protected string expectedParameterValues;
        protected string parameterSepararor;
        protected string returnValue;
        protected List<Parameter> parameters;
        protected SqlParameterStringBuilder sqlParameterStringBuilder;

        public SqlMockScalarFunctionBuilderBase(SqlConnection connection, string functionName, string testName, string parameterNames, string expectedParameterValues, string parameterSepararor, string returnValue)
        {
            this.connection = connection;
            this.functionName = functionName;
            this.testName = testName;
            this.parameterNames = parameterNames;
            this.expectedParameterValues = expectedParameterValues;
            this.parameterSepararor = parameterSepararor;
            this.returnValue = returnValue;
            Init();
        }

        public const string SQLFUNCTION =
            @"ALTER FUNCTION {0}
(
      --parameters
      {1}
)
RETURNS {2} --return type
AS BEGIN
		DECLARE @errorMessage nvarchar(250);
		DECLARE @testName nvarchar(250);

		SET @testName = '{3}'; -- testName

         --Assertions
           {4}
        -- return
        {5} ;
END
";

        protected virtual void Init()
        {

            parameters = GetParameters();
            sqlParameterStringBuilder = new SqlParameterStringBuilder(parameters);
        }

        public List<Parameter> GetParameters()
        {            
            ParametersDataAccess parameterDataAccess = new ParametersDataAccess(connection);
            return parameterDataAccess.GetParameters(functionName);
        }

        public abstract string BuildMockFunction();
    }
}