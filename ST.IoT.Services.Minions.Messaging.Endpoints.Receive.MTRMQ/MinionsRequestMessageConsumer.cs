using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using ST.IoT.Services.Minions.Interfaces;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Messaging.Endpoints.Receive.MTRMQ
{
    /*
    internal class MinionsRequestMessageConsumer : IConsumer<MinionsRequestMessage>
    {
        private IMinionsReceiveRequestEndpoint _receiver;

        public MinionsRequestMessageConsumer()
        {
            
        }

        public MinionsRequestMessageConsumer(IMinionsReceiveRequestEndpoint receiver)
        {
            _receiver = receiver;
        }

        public async Task Consume(ConsumeContext<MinionsRequestMessage> context)
        {
            var result = await _receiver.ProcessRequestAsync(context.Message);
            context.RespondAsync(result);
        }
    }
     * */
}
