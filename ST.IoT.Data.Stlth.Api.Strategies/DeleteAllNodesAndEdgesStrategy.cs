using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class DeleteAllNodesAndEdgesStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public DeleteAllNodesAndEdgesStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            await ((StlthDataClient)_client).deleteAllNodesAndEdges();
        }
    }
}
