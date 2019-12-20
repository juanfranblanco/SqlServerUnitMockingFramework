using System;
using System.Collections.Generic;

public class SqlScalarFunctionReturnValueStringBuilder:SqlMockValueStringBuilderBase
{
    private Parameter returnParameter;
    private string returnValue;

    public SqlScalarFunctionReturnValueStringBuilder(List<Parameter> parameters, string returnValue)
    {
        returnParameter = GetReturnParameter(parameters);
        this.returnValue = returnValue;
    }
        
    public string GetReturnType()
    {
        return GetSqlStringType(returnParameter);
    }

    public string GetReturnValue()
    {
       return GetSqlValue(returnParameter, returnValue);
    }

    protected Parameter GetReturnParameter(List<Parameter> parameters)
    {
        foreach (Parameter parameter in parameters)
        {
            if(parameter.Name == "") return parameter; 
        }
        throw new Exception("No return parameter found, this may not be an scalar function");
    }
}