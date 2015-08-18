using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.Endpoints
{
    public interface IForwardToServiceEndpoint
    {
        
    }

    [Export(typeof(IForwardToServiceEndpoint))]
    public class ForwardToServiceEndpoint : RequestReplySendEndpoint<RestRequest, RestResponse>, IForwardToServiceEndpoint
    {
        public ForwardToServiceEndpoint() : base("stlth_requests")
        {
            
        }
    }
}
