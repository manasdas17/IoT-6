using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Endpoints
{
    public interface IConsumeMinionsRequestEndpoint
    {
        
    }

    public class ConsumeMinionsRequestEndpoint : RequestReplyConsumeEndpoint<MinionsRequestMessage, MinionsResponseMessage>, IConsumeMinionsRequestEndpoint
    {
        public ConsumeMinionsRequestEndpoint() : base("minions_requests")
        {
            
        }
    }
}
