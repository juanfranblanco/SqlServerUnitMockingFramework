using System;

public class SqlBuilderBase
{
    public string GetSqlStringType(ColumnBase columnBase)
    {
        return String.Format("{0}{1}", 
                             columnBase.TypeName,
                             GetSqlStringForSizeOrPrecision(columnBase));
    }

    public string GetSqlStringForSizeOrPrecision(ColumnBase columnName)
    {
        if(IsPrecisionScaleRequired(columnName.TypeName))
        {
            return String.Format("({0},{1})", columnName.Precission, columnName.Scale);
        }
        if(IsSizeRequired(columnName.TypeName))
        {
            return String.Format("({0})", columnName.MaxLength);
        }
        return String.Empty;
    }

    public bool IsPrecisionScaleRequired(string sqlType)
    {
        sqlType = sqlType.ToLower();
        return sqlType == "numeric" || sqlType == "decimal";
    }

    public bool IsSizeRequired(string sqlType)
    {
        sqlType = sqlType.ToLower();
        return sqlType == "binary"
               || sqlType == "char"
               || sqlType == "nchar"
               || sqlType == "nvarchar"
               || sqlType == "varbinary"
               || sqlType == "varchar";
    }
}