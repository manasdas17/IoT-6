using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NLog;

namespace ST.IoT.Stlth.Data.Neo4jUtils
{
    public static class QueryExecutor
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static async void executeWithoutResults(ICypherFluentQuery query)
        {
            var queryText = "";
            try
            {
                queryText = query.Query.DebugQueryText;
                _logger.Debug(queryText);
                await query.ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                throw;
            }
        }
    }
}
