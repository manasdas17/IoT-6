using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Spikes.RabbitMQ.MassTransit.Messages
{
    public interface IRequestMessage
    {
        string TheMessage { get; }
    }

    public class RequestMesssge : IRequestMessage
    {
        public string TheMessage { get; set; }

        public RequestMesssge()
        {
        }
    }
    public interface IReplyMessage
    {
        string TheReply { get; }
    }

    public class ReplyMesssge : IReplyMessage
    {
        public string TheReply { get; set; }

        public ReplyMesssge()
        {
        }
    }
}
