using System;
using System.Collections;
using System.Collections.Generic;

public class SqlMockValueStringBuilderBase : SqlBuilderBase
{
    public static Hashtable BuildParameterValues(string names, string values, string arraySeparator)
    {
        string[] namesArray = names.Split(new string[] { arraySeparator }, StringSplitOptions.RemoveEmptyEntries);
        string[] valuesArray = values.Split(new string[] { arraySeparator }, StringSplitOptions.RemoveEmptyEntries);

        if(namesArray.Length != valuesArray.Length)
            throw new Exception("Names length does not match values length");

        Hashtable nameValueCollection = new Hashtable();
        for(int i=0; i<namesArray.Length; i++)
        {
            nameValueCollection.Add(namesArray[i], valuesArray[i]);
        }
        return nameValueCollection;
    }

    public static string GetSqlValue(ColumnBase columnBase, string value)
    {
        string typeName = columnBase.TypeName.ToUpper();

        if(typeName.EndsWith("CHAR") || typeName.Equals("DATETIME"))
        {
            return String.Format("'{0}'", value);
        }
        return value;
    }
}