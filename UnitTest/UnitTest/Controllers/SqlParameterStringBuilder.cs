using System.Collections.Generic;
using System.Text;

public class SqlParameterStringBuilder : SqlBuilderBase
{
    private List<Parameter> parameters;

    public SqlParameterStringBuilder(List<Parameter> parameters)
    {
        this.parameters = parameters;
    }

    public string BuildSqlString()
    {
        bool prefixComma = false;
        StringBuilder builder = new StringBuilder();
        foreach(Parameter field in parameters)
        {
            if(field.Name != "")
            {
                builder.Append(BuildParamaterSqlString(field, prefixComma));
                prefixComma = true;
            }
        }
        return builder.ToString();
    }

    private string BuildParamaterSqlString(Parameter parameter, bool prefixComma)
    {
        StringBuilder builder = new StringBuilder();
        if(prefixComma)
        {
            builder.AppendLine(",");
        }
        builder.AppendFormat("{0} {1}",
                             parameter.Name,
                             GetSqlStringType(parameter)
            );
        return builder.ToString();
    }
}