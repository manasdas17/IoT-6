using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class LoadMetaRelsStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public LoadMetaRelsStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            await _client.LoadMetaRelsAsync();
        }
    }
}
