using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DataAccess;
using System.Data.SqlClient;

namespace UnitTest.Controllers
{
    public class SqlShouldNotCallStoredProcedureBuilder:SqlBuilderBase
    {
        public const string SQL_PROCEDURE = 
        
        @"ALTER PROCEDURE {0} 
        (
            {1}
        )
        AS
            BEGIN
               EXEC tsu_failure 'Stored procedure {0} should have not been called'
            END
        ";

        protected List<Parameter> parameters;
        protected SqlParameterStringBuilder sqlParameterStringBuilder;
        private string procedureName;
        private SqlConnection connection;

        public SqlShouldNotCallStoredProcedureBuilder(SqlConnection connection, string procedureName)
        {
            this.procedureName = procedureName;
            this.connection = connection;
            Init();
        }

        public string BuildStoredProcedure()
        {
            return String.Format(SQL_PROCEDURE, procedureName, sqlParameterStringBuilder.BuildSqlString());
        }

        protected virtual void Init()
        {
            parameters = GetParameters();
            sqlParameterStringBuilder = new SqlParameterStringBuilder(parameters);
        }

        public List<Parameter> GetParameters()
        {
            ParametersDataAccess parameterDataAccess = new ParametersDataAccess(connection);
            return parameterDataAccess.GetParameters(procedureName);
        }
    }
}
