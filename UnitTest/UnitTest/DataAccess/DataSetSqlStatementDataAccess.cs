using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace UnitTest.DataAccess
{
    public class DataSetSqlStatementDataAccess
    {
        public static DataSet GetDataSet(SqlConnection connection, string sqlStatement)
        {
            DataSet dataSet = new DataSet();
            using(SqlCommand command = new SqlCommand())
            {
                //connection.Open();
                command.CommandText = sqlStatement;
                command.Connection = connection;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet);
                //connection.Close();
            }

            return dataSet;
       
        }
    }
}
