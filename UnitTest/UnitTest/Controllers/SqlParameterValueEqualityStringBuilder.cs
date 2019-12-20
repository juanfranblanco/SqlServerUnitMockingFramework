using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class SqlParameterValueEqualityStringBuilder:SqlMockValueStringBuilderBase
{
    private List<Parameter> parameters;
    private Hashtable parameterValuesHashtable;

    public SqlParameterValueEqualityStringBuilder(List<Parameter> parameters, string parameterNames, string parameterValues, string parameterSeparator)
    {
        this.parameters = parameters;
        parameterValuesHashtable = BuildParameterValues(parameterNames, parameterValues, parameterSeparator);
    }

    public string BuildSqlParameterValueEqualityStatement()
    {
        StringBuilder builder = new StringBuilder();
        bool isFirst = true;
        
        foreach(Parameter parameter in parameters)
        {
            if(parameter.Name != "")
            {
                if(parameterValuesHashtable.ContainsKey(parameter.Name))
                {
                    if(!isFirst) builder.Append(" AND ");

                    builder.Append(BuildSqlParameterValueEqualityStatement(parameter, (string)parameterValuesHashtable[parameter.Name]));
                    isFirst = false;
                }
                else
                {
                    throw new Exception("Parameter name incorrectly or not found:" + parameter.Name);

                }
            }
        }
        return builder.ToString();
    }

    public const string SQL_PARAMETER_EQUALITY_STATEMENT = "({0} = {1})";

    public string BuildSqlParameterValueEqualityStatement(Parameter parameter, string value)
    {
        return String.Format(SQL_PARAMETER_EQUALITY_STATEMENT, parameter.Name, GetSqlValue(parameter, value));
    }
}