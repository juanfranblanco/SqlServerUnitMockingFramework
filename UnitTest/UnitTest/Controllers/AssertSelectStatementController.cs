using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using UnitTest.DataAccess;

namespace UnitTest.Controllers
{
    public class AssertSelectStatementController
    {
        SqlConnection connection;
        protected Hashtable columnValuesHashtable;
        string columnNames;
        string expectedColumnValues;
        string arraySeparator; 
        string selectStatement;
        
        public AssertSelectStatementController(SqlConnection connection, string columnNames, string expectedColumnValues, string arraySeparator, string selectStatement)
        {
             this.connection = connection;
             this.expectedColumnValues = expectedColumnValues;
             this.columnNames = columnNames;
             this.arraySeparator = arraySeparator;
             this.selectStatement = selectStatement;
             Init();
        }

        public void Init()
        {
            columnValuesHashtable = SqlMockValueStringBuilderBase.BuildParameterValues(columnNames,
                                                                                   expectedColumnValues,
                                                                                   arraySeparator);
        }

        public void DoAssertion()
        {
            DataSet dataSet = DataSetSqlStatementDataAccess.GetDataSet(connection, selectStatement);
            AssertDataSet(dataSet);
        }

        public string ListExpectations()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            foreach(string columnName in columnValuesHashtable.Keys)
            {
                builder.AppendLine(String.Format("{0} = {1}", columnName, columnValuesHashtable[columnName]));
            }

            return builder.ToString();
        }

        public void AssertDataSet(DataSet dataSet)
        {
            if(dataSet.Tables.Count == 0) throw new Exception("DataSet has not been populated");

            DataTable dataTable = dataSet.Tables[0];

            if(dataTable.Rows.Count == 0) throw new Exception("DataTable has no rows to validate");

            foreach(string columnName in columnValuesHashtable.Keys)
            {
                if(dataTable.Columns.Contains(columnName))
                {
                    object rowValue = dataTable.Rows[0][columnName] == DBNull.Value ? null : dataTable.Rows[0][columnName];
                    object expectedValue = ConvertStringValueToDataColumnType(dataTable.Columns[columnName], (string)columnValuesHashtable[columnName]);

                    if(BothAreNull(rowValue, expectedValue) || (rowValue!=null && rowValue.Equals(expectedValue)))
                    {
                         //passed
                    }
                    else
                    {
                       UnitTestFailureDataAccess.InsertUnitTestFailure(connection, 
                           String.Format("Expected value for {0} was {1} but was {2}", columnName, (string)columnValuesHashtable[columnName],
                           dataTable.Rows[0][columnName] == DBNull.Value ? "NULL" : dataTable.Rows[0][columnName]));

                    }
                }
                else
                {
                    throw new Exception("ColumnName not found");
                }

            }
        }

        public bool BothAreNull(object first, object second)
        {
            return first == null && second == null;
        }


        public object ConvertStringValueToDataColumnType(DataColumn column, string value)
        {
           if(value == "NULL") return null;
           return Convert.ChangeType(value, column.DataType);
        }
    }
}
