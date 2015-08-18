using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.Endpoints
{
    public interface IConsumeRestMessageEndpoint
    {
        
    }

    public class ConsumeRestMessageEndpoint : RequestReplyConsumeEndpoint<RestRequest, RestResponse>, IConsumeRestMessageEndpoint
    {
        public ConsumeRestMessageEndpoint() : base("stlth_requests")
        {
        }
    }
}
