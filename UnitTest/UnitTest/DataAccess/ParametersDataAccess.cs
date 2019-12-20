using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace UnitTest.DataAccess
{
    public class ParametersDataAccess
    {
        public const string SqlCommandParameters =
                @"SELECT 
                        TYPE_NAME(user_type_id) AS typeName,
                        object_id,
                        name,
                        parameter_id,
                        system_type_id,
                        user_type_id,
                        max_length,
                        precision,
                        scale,
                        is_output,
                        is_cursor_ref,
                        has_default_value,
                        is_xml_document,
                        default_value,
                        xml_collection_id
                FROM sys.parameters
                WHERE object_id = OBJECT_ID('{0}')
                ORDER BY parameter_id
                ";

        private SqlConnection connection;

        public ParametersDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        public List<Parameter> GetParameters(string objectName)
        {
               List<Parameter> parameters = new List<Parameter>();
               using(SqlCommand command = new SqlCommand(String.Format(SqlCommandParameters, objectName), connection))
               {
                   
                   SqlDataReader reader = command.ExecuteReader();
                   while (reader.Read())
                   {
                       Parameter parameter = new Parameter();
                       parameter.TypeName = reader.GetString(0);
                       parameter.ObjectId = reader.GetInt32(1);
                       parameter.Name = reader.GetString(2);
                       parameter.ParameterId = reader.GetInt32(3);
                       parameter.SystemTypeId = Convert.ToInt32(reader.GetValue(4));
                       parameter.UserTypeId = reader.GetInt32(5);
                       parameter.MaxLength = Convert.ToInt32(reader.GetValue(6));
                       parameter.Precission = Convert.ToInt32(reader.GetValue(7));
                       parameter.Scale = Convert.ToInt32(reader.GetValue(8));
                       parameter.Output = reader.GetBoolean(9);
                       parameter.CursorRef = reader.GetBoolean(10);
                       parameter.HasDefaultValue = reader.GetBoolean(11);
                       parameter.IsXmlDocument = reader.GetBoolean(12);
                       parameter.DefaultValue = reader.GetValue(13);
                       parameter.XmlCollectionId = reader.GetInt32(14);
                       parameters.Add(parameter);
                   }

                   reader.Close();
               }
            return parameters;
        }
    }
}
