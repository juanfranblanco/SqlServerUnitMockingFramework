using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DataAccess;
using System.Collections;
using System.Data.SqlClient;

namespace UnitTest.Controllers
{
    public class SqlMockTableFunctionBuilder:SqlBuilderBase
    {
        public const string SQLFUNCTION =
@"ALTER FUNCTION {0}
(
      --parameters
      {1}
)
RETURNS @answer TABLE
(
    --table definition
    {2}
)
AS BEGIN
		DECLARE @errorMessage nvarchar(250);
		DECLARE @testName nvarchar(250);

		SET @testName = '{3}'; -- testName

         --Assertions
           {4}
        -- return table
               {5}
	    RETURN
         ;
END
";

        protected SqlConnection connection;
        protected string functionName;
        protected string testName;
        protected string parameterNames;
        protected string expectedParameterValues;
        protected string separator;
        protected string tableReturnColumnValues;
        protected string tableReturnColumnNames;

        protected List<Parameter> parameters;
        protected List<Column> columns;

        protected SqlParameterStringBuilder sqlParameterStringBuilder;
        protected SqlParameterValueAssertionStringBuilder sqlParameterValueAssertionStringBuilder;
        protected Hashtable parameterValuesHashtable;

        public SqlMockTableFunctionBuilder(SqlConnection connection, string functionName, string testName, string parameterNames, string expectedParameterValues, string separator, string tableReturnColumnNames, string tableReturnColumnValues)
        {
            this.connection = connection;
            this.functionName = functionName;
            this.testName = testName;
            this.parameterNames = parameterNames;
            this.expectedParameterValues = expectedParameterValues;
            this.separator = separator;
            this.tableReturnColumnNames = tableReturnColumnNames;
            this.tableReturnColumnValues = tableReturnColumnValues;
            Init();
        }

        protected virtual void Init()
        {
            parameters = GetParameters();
            sqlParameterStringBuilder = new SqlParameterStringBuilder(parameters);
            parameterValuesHashtable = SqlMockValueStringBuilderBase.BuildParameterValues(parameterNames,
                                                                                      expectedParameterValues,
                                                                                      separator);
            
            sqlParameterValueAssertionStringBuilder = new SqlParameterValueAssertionStringBuilder(parameters,
                                                                                                  parameterValuesHashtable);
            columns = GetColumns();
        }

        public List<Parameter> GetParameters()
        {
            ParametersDataAccess parameterDataAccess = new ParametersDataAccess(connection);
            return parameterDataAccess.GetParameters(functionName);
        }

        public List<Column> GetColumns()
        {
            ColumnDataAccess columnDataAccess = new ColumnDataAccess(connection);
            return columnDataAccess.GetColumns(functionName);
        }

        public string BuildMockFunction()
        {
            return string.Format(SQLFUNCTION, functionName, sqlParameterStringBuilder.BuildSqlString(),
                               BuildColumnTableString(), testName,
                               sqlParameterValueAssertionStringBuilder.BuildSqlParameterAssertion(),
                               BuildReturnTableValues());
        }


        public string BuildReturnTableValues()
        {
            Hashtable nameValueCollection = 
                SqlMockValueStringBuilderBase.BuildParameterValues(tableReturnColumnNames, tableReturnColumnValues, separator);

            List<string> columnNamesInsertInto = new List<string>();
            List<string> columnValuesInsertInto = new List<string>();

            foreach(Column column in columns)
            {
                if(!column.Identity)
                {
                    if(nameValueCollection.Contains(column.Name))
                    {
                        columnNamesInsertInto.Add(column.Name);
                        columnValuesInsertInto.Add(SqlMockValueStringBuilderBase.GetSqlValue(column, (string)nameValueCollection[column.Name]));
                    }
                    else
                    {
                        throw new Exception("Column name not found");
                    }
                }
            }

            return string.Format(SQL_INSERT, String.Join(",", columnNamesInsertInto.ToArray()), String.Join(",", columnValuesInsertInto.ToArray()));
           
        }

        public const string SQL_INSERT =
@"
    INSERT INTO @answer ({0}) VALUES ({1})
";

        public string BuildColumnTableString()
        {
            List<string> sqlColumn = new List<string>();

            foreach(Column column in columns)
            {
               sqlColumn.Add(BuildColumnTableString(column));   
            }

            return string.Join(",", sqlColumn.ToArray());
        }
        public string BuildColumnTableString(Column column)
        {
            return String.Format(SQL_COLUMN, column.Name, GetSqlStringType(column), column.Nullable ? "" : "NOT NULL", column.Identity ? "IDENTITY" : "");
        }

        public const string SQL_COLUMN = @"{0} {1} {2} {3}";

       
    }
}
