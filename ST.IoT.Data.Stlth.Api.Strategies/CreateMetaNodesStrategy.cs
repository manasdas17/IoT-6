using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Data.Stlth.Model;

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

            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Person, DescribeAsNeoJSON<Person>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Post, DescribeAsNeoJSON<Post>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Group, DescribeAsNeoJSON<Group>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Community, DescribeAsNeoJSON<Community>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Space, DescribeAsNeoJSON<Room>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Timeline, DescribeAsNeoJSON<Timeline>.describe());
            await _client.MetaNodeAsync(StlthDataOperation.POST, StlthBuiltinNodeLabels.Thing, DescribeAsNeoJSON<Thing>.describe());

            _client.Disconnect();
        }
    }
}
