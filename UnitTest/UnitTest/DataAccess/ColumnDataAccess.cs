using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace UnitTest.DataAccess
{
    public class ColumnDataAccess
    {
        public const string SqlCommandColumns =
                @"SELECT 
                        TYPE_NAME(user_type_id) AS typeName,
                        object_id,
                        name,
                        column_id,
                        system_type_id,
                        user_type_id,
                        max_length,
                        precision,
                        scale,
                        is_nullable,
                        is_identity,
                        is_xml_document,
                        xml_collection_id
                FROM sys.columns
                WHERE object_id = OBJECT_ID('{0}')
                ORDER BY column_id
                ";
        
        private SqlConnection connection;

        public ColumnDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        public List<Column> GetColumns(string objectName)
        {
            List<Column> columns = new List<Column>();
            using(SqlCommand command = new SqlCommand(String.Format(SqlCommandColumns, objectName), connection))
            {

                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Column column = new Column();
                    column.TypeName = reader.GetString(0);
                    column.ObjectId = reader.GetInt32(1);
                    column.Name = reader.GetString(2);
                    column.ColumnId = reader.GetInt32(3);
                    column.SystemTypeId = Convert.ToInt32(reader.GetValue(4));
                    column.UserTypeId = reader.GetInt32(5);
                    column.MaxLength = Convert.ToInt32(reader.GetValue(6));
                    column.Precission = Convert.ToInt32(reader.GetValue(7));
                    column.Scale = Convert.ToInt32(reader.GetValue(8));
                    column.Nullable  = reader.GetBoolean(9);
                    column.Identity = reader.GetBoolean(10);
                    column.IsXmlDocument = reader.GetBoolean(11);
                    column.XmlCollectionId = reader.GetInt32(12);
                    columns.Add(column);
                }

                reader.Close();
            }
            return columns;
        }
    }
}
