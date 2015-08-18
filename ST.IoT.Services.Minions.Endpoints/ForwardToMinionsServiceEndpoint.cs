using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Endpoints
{
    public interface IForwardToMinionsServiceEndpoint
    {
        
    }

    [Export(typeof(IForwardToMinionsServiceEndpoint))]
    public class ForwardToMinionsServiceEndpoint : RequestReplySendEndpoint<MinionsRequestMessage, MinionsResponseMessage>, IForwardToMinionsServiceEndpoint
    {
        public ForwardToMinionsServiceEndpoint() : base("minions_requests")
        {
            
        }
    }
}
