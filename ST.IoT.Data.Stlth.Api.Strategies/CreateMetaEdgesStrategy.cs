using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateMetaEdgesStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public CreateMetaEdgesStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            _client.Connect();
            await _client.MetaEdgeAsync(StlthDataOperation.POST, StlthBuiltinEdgeLabels.Interest, "['Likes', 'Agrees', 'Follows', 'Welcomes']");
            await _client.MetaEdgeAsync(StlthDataOperation.POST, StlthBuiltinEdgeLabels.Action, "['Watches', 'Reads']");
            _client.Disconnect();
        }
    }
}
