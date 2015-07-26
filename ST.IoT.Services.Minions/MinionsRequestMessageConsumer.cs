using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions
{
    internal class MinionsRequestMessageConsumer : IConsumer<MinionsRequestMessage>
    {
        public async Task Consume(ConsumeContext<MinionsRequestMessage> context)
        {
            context.RespondAsync(new MinionsResponseMessage("OK"));
            await Task.FromResult(0);
        }
    }
}
