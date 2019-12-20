using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SqlParameterValueAssertionStringBuilder : SqlMockValueStringBuilderBase
{
    public const string SQL_ASSERTION =
       @"
        IF ({0} != {1})
		BEGIN
			SET @errorMessage = N'Expected value for {0} did not match parameter';
			EXEC LogTsuFailure @testName, @errorMessage;
		END
    ";

    private List<Parameter> parameters;
    private Hashtable parameterValuesHashtable;

    public SqlParameterValueAssertionStringBuilder(List<Parameter> parameters, Hashtable parameterValuesHashtable)
    {
        this.parameters = parameters;
        this.parameterValuesHashtable = parameterValuesHashtable;
    }

    public string BuildSqlParameterAssertion()
    {
        StringBuilder builder = new StringBuilder();
        foreach (Parameter parameter in parameters)
        {  
            //Exclude empty name which are the return values for scalar functions
            if(parameter.Name != "")
            {
                if (parameterValuesHashtable.ContainsKey(parameter.Name))
                {
                    builder.Append(BuildSqlParameterAssertion(parameter, (string) parameterValuesHashtable[parameter.Name]));
                }
                else
                {
                    throw new Exception("Parameter name is incorrect or not found:" + parameter.Name);

                }
            }
        }
        return builder.ToString();
    }

    public string BuildSqlParameterAssertion(Parameter parameter, string value)
    {
        return String.Format(SQL_ASSERTION, parameter.Name, GetSqlValue(parameter, value));
    }

   
}