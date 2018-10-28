using System;
using System.Collections.Generic;
using System.Data;

namespace EllisMigrator.SqlServer
{
    public class SqlServerExecutionContext : IExecutionContext
    {
        private readonly Func<IDbConnection> connectionFactory;
        private IDbConnection connection;

        public SqlServerExecutionContext(Func<IDbConnection> connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void PreExecution()
        {
            connection = connectionFactory();
            connection.Open();
        }

        public void PostExecution()
        {
            connection.Dispose();
        }

        public void ExecuteSql(string sql, Dictionary<string, object> parameters = null)
        {
            using (var command = connection.CreateCommand())
            {
                PrepareSqlStatement(sql, command);
                PrepareSqlParameters(parameters, command);

                command.ExecuteNonQuery();
            }
        }

        private static void PrepareSqlStatement(string sql, IDbCommand command)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
        }

        private static void PrepareSqlParameters(Dictionary<string, object> parameters, IDbCommand command)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    AddSqlParameterToCommand(parameter.Key, parameter.Value, command);
                }
            }
        }

        private static void AddSqlParameterToCommand(string name, object value, IDbCommand command)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
