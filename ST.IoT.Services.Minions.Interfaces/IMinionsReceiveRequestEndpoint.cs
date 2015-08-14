using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Interfaces
{
    public class MinionsRequestMessageReceivedEventArgs : EventArgs
    {
        public MinionsRequestMessage RequestMessage { get; private set; }
        public MinionsResponseMessage ResponseMessage { get; set; }

        public MinionsRequestMessageReceivedEventArgs(object sender, MinionsRequestMessage message)
        {
            RequestMessage = message;
            ResponseMessage = null;
        }
    }

    public interface IMinionsReceiveRequestEndpoint 
    {
        Task<MinionsResponseMessage> ProcessRequestAsync(MinionsRequestMessage request);

        event EventHandler<MinionsRequestMessageReceivedEventArgs> ReceivedRequestMessage;
    }
}
