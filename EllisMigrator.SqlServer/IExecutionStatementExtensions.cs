using EllisMigrator.SqlServer;
using System.Collections.Generic;

namespace EllisMigrator
{
    public static class IExecutionStatementExtensions
    {
        public static void RawSql(this IExecutionStatement execution, string sql, Dictionary<string, object> parameters = null)
        {
            var context = execution.Context as SqlServerExecutionContext;

            context.ExecuteSql(sql, parameters);
        }
    }
}
