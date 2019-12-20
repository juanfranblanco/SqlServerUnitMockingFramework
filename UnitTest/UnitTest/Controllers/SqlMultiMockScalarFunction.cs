using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UnitTest.DataAccess;

namespace UnitTest.Controllers
{
    public class SqlMultiMockScalarFunctionBuilder:SqlMockScalarFunctionBuilderBase
    {
        protected string multiMockSeparator;
        protected SqlScalarFunctionReturnValueStringBuilder defaultReturnValueStringBuilder;
        protected  string[] expectedParameterValuesArray;
        protected string[] expectedReturnValuesArray;
        

        public SqlMultiMockScalarFunctionBuilder(SqlConnection connection, string functionName, string testName, string parameterNames, string expectedParameterValues, string parameterSepararor, string multiMockSeparator, string returnValue)
            : base(connection, functionName, testName, parameterNames, expectedParameterValues, parameterSepararor, returnValue)
        {
            this.multiMockSeparator = multiMockSeparator;
            InitArraysAndDefaultReturn();
        }

        protected void InitArraysAndDefaultReturn()
        {
            InitArraysMultiSetValues();
            if(expectedReturnValuesArray == null || expectedReturnValuesArray.Length == 0)
                throw new Exception("Expected return values should have been set");

            defaultReturnValueStringBuilder = new SqlScalarFunctionReturnValueStringBuilder(parameters, expectedReturnValuesArray[0]);
        }

        public override string BuildMockFunction()
        {
            return string.Format(SQLFUNCTION, functionName, sqlParameterStringBuilder.BuildSqlString(), defaultReturnValueStringBuilder.GetReturnType(), testName,
                                 BuildMultiMockSqlParametersEqualityChecksAndReturn(), BuildDefaultReturnAndFailureLog());
        }

        public void InitArraysMultiSetValues()
        {
            expectedParameterValuesArray = expectedParameterValues.Split(
               new string[] { multiMockSeparator }, StringSplitOptions.None);

            expectedReturnValuesArray = returnValue.Split(new string[] { multiMockSeparator },
                                                                   StringSplitOptions.None);

            if(expectedParameterValuesArray.Length != expectedReturnValuesArray.Length)
            {
                throw new Exception("multi set of parameter values does not match the multi set of return values");
            }

        }

        public string BuildMultiMockSqlParametersEqualityChecksAndReturn()
        {
           
            StringBuilder builder = new StringBuilder();
            for(int i=0; i< expectedParameterValuesArray.Length; i++)
            {
                SqlParameterValueEqualityStringBuilder equalityStringBuilder =
                    new SqlParameterValueEqualityStringBuilder(parameters, parameterNames,
                                                               expectedParameterValuesArray[i],
                                                               parameterSepararor);

                SqlScalarFunctionReturnValueStringBuilder returnValueStringBuilder =
                    new SqlScalarFunctionReturnValueStringBuilder(parameters, expectedReturnValuesArray[i]);

                builder.AppendFormat(
                    SQL_MULTI_MOCK_ITEM_RETURN_CHECK,
                    equalityStringBuilder.BuildSqlParameterValueEqualityStatement(),
                    returnValueStringBuilder.GetReturnValue()
                    );
            }
            return builder.ToString();
        }

        public const string SQL_MULTI_MOCK_ITEM_RETURN_CHECK =
         @" 
               IF {0} --Check IF param = value and param2 = value2
               BEGIN
                     RETURN {1} -- return specified value
               END
            ";

        public string BuildDefaultReturnAndFailureLog()
        {
            return String.Format(SQL_MULTI_MOCK_ITEM_NOT_FOUND_LOG_EXCEPTION, defaultReturnValueStringBuilder.GetReturnValue());
        }

        public const string SQL_MULTI_MOCK_ITEM_NOT_FOUND_LOG_EXCEPTION =
            @"
                SET @errorMessage = N'Mock expectation and return has not been for the given parameters';
			    EXEC LogTsuFailure @testName, @errorMessage;
                
                RETURN {0}; --Default value to return, first one of expectation set
            ";
    }
}
