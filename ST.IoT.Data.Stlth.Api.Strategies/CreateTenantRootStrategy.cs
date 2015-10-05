using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateTenantRootStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public CreateTenantRootStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            await Task.FromResult<int>(0);
        }
    }
}
