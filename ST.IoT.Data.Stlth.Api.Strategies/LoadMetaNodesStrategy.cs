using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class LoadMetaNodesStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public LoadMetaNodesStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            await _client.LoadMetaNodesAsync();
        }
    }
}
