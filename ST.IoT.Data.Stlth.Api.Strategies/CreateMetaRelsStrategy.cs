using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateMetaRelsStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public CreateMetaRelsStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            _client.Connect();
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Feed  , StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Post);
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Friend, StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Person);
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Spouse, StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Person);
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Parent, StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Person);
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Member, StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Group);
            await _client.MetaRelAsync(StlthDataOperation.POST, StlthBuiltinRelNames.Member, StlthBuiltinNodeLabels.Person, StlthBuiltinNodeLabels.Community);
            _client.Disconnect();
        }
    }
}
