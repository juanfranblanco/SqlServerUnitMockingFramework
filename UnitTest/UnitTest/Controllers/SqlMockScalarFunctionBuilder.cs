using System;
using System.Collections;
using System.Data.SqlClient;
using System.Text;

namespace UnitTest.Controllers
{
    public class SqlMockScalarFunctionBuilder : SqlMockScalarFunctionBuilderBase
    {
        protected SqlParameterValueAssertionStringBuilder sqlParameterValueAssertionStringBuilder;
        protected SqlScalarFunctionReturnValueStringBuilder sqlScalarFunctionReturnValueStringBuilder;
        protected Hashtable parameterValuesHashtable;


        public SqlMockScalarFunctionBuilder(SqlConnection connection, string functionName, string testName, string parameterNames, string expectedParameterValues, string parameterSepararor, string returnValue)
            : base(connection, functionName, testName, parameterNames, expectedParameterValues, parameterSepararor, returnValue)
        {
        }

        protected override void Init()
        {
            base.Init();

            parameterValuesHashtable = SqlMockValueStringBuilderBase.BuildParameterValues(parameterNames,
                                                                                         expectedParameterValues,
                                                                                         parameterSepararor);

            sqlParameterValueAssertionStringBuilder = new SqlParameterValueAssertionStringBuilder(parameters,
                                                                                                  parameterValuesHashtable);

            sqlScalarFunctionReturnValueStringBuilder = new SqlScalarFunctionReturnValueStringBuilder(parameters,
                                                                                                      returnValue);

        }

        public override string BuildMockFunction()
        {
            return string.Format(SQLFUNCTION, functionName, sqlParameterStringBuilder.BuildSqlString(),
                                 sqlScalarFunctionReturnValueStringBuilder.GetReturnType(), testName,
                                 sqlParameterValueAssertionStringBuilder.BuildSqlParameterAssertion(),
                                 "RETURN " + sqlScalarFunctionReturnValueStringBuilder.GetReturnValue());
        }

        public string GetInfo()
        {
            return
                String.Format(
@"Calling the function {0} with the following parameter values: 
     {1} 
and with a ficticious return value of: {2}",
                    functionName, GetValuePairedStringValues(), returnValue);

        }

        public string GetValuePairedStringValues()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            foreach(string key in parameterValuesHashtable.Keys)
            {
                builder.AppendLine(String.Format("{0} = {1}", key, parameterValuesHashtable[key]));
            }

            return builder.ToString();
        }
    }
}
