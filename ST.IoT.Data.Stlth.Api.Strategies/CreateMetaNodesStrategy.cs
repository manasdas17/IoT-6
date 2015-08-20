using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateMetaNodesStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public CreateMetaNodesStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            _client.Connect();
            await _client.NodeAsync(StlthDataOperation.POST, StlthNodeType.MetaRoot, StlthBuiltinNodeLabels.Root);
            await _client.NodeAsync(StlthDataOperation.POST, StlthNodeType.Meta, StlthBuiltinNodeLabels.Person);
            await _client.NodeAsync(StlthDataOperation.POST, StlthNodeType.Meta, StlthBuiltinNodeLabels.Post);
            _client.Disconnect();
        }
    }
}
