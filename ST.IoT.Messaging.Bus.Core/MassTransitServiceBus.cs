using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Internals.Extensions;
using MassTransit.RabbitMqTransport;
using NLog;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;

namespace ST.IoT.Messaging.Bus.Core
{
    public interface IServiceBus
    {
    }

    public class EndpointParameters
    {
        public EndpointParameters(Uri address, string username, string password, string virtualHost,
            double? timeout = null)
        {
            Address = address;
            Username = username;
            Password = password;
            VirtualHost = virtualHost;
            Timeout = timeout != null ? TimeSpan.FromMilliseconds(timeout.Value) : TimeSpan.FromSeconds(5);
        }

        public Uri Address { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string VirtualHost { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public static EndpointParameters Default { get; set; }
    }

    public abstract class EndpointBase : IDisposable
    {
        /*
        public Uri Address { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string VirtualHost { get; private set; }
         * */

        public EndpointParameters EndpointParameters { get; private set; }
        //public string QueueName { get; private set; }
        public EndpointConfigurator EndpointConfigurator { get; private set; }

        public IBusControl BusControl { get; set; }
        public BusHandle BusHandle { get; set; }

        public IRabbitMqHost Host { get; set; }

        public string QueueName { get; set; }

        public EndpointBase()
        {
        }

        public EndpointBase(EndpointParameters parameters = null)
        {
            EndpointParameters = parameters ?? EndpointParameters.Default;
        }

        public EndpointBase(EndpointConfigurator configurator = null)
        {
            EndpointConfigurator = configurator ?? EndpointConfigurator.Default;
        }

        public void Dispose()
        {
            unwire();
        }

        protected abstract void wire();
        protected abstract void unwire();

        public Uri CalculatedAddress { get; set; }
    }

    public interface IMassTransitEndpoint
    {
        void Start();
        void Stop();
    }

    public interface ISendEndpoint<Request> : IMassTransitEndpoint where Request : class
    {
        Task SendAsync<Request>(Request request);
    }

    public interface IRequestReplySendEndpoint<Request, Reply> : IMassTransitEndpoint where Request : class
        where Reply : class
    {
        Task<Reply> SendAsync(Request request, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class SendEndpoint<Request> : EndpointBase, ISendEndpoint<Request> where Request : class
    {
        public SendEndpoint() : base()
        {

        }

        /*
        public MassTransitServiceBusSendEndpoint(
             Uri address, string username, string password, string queueName, string virtualHost = "", IBus bus = null)
            :base(address, username, password, queueName, virtualHost, bus)
        {
        }
         * */

        public async Task SendAsync<Request>(Request request)
        {
            wire();
        }

        protected override void wire()
        {
            /*
            if (BusHandle != null) return;

            _calculatedAddress = Address;
            if (!string.IsNullOrEmpty(VirtualHost))
            {
                _calculatedAddress = new Uri(Address + "/" + VirtualHost);
            }
            */
        }

        protected override void unwire()
        {
        }

        public void Start()
        {
            wire();
        }

        public void Stop()
        {
            unwire();
        }
    }

    /*
    public class MassTransitServiceBusRequestReplyEndpointConfig
    {
        public Uri Address { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string VirtualHost { get; private set; }
    }
    */


    public abstract class EndpointConfigurator
    {
        public static EndpointConfigurator Default = new RabbitMqEndpointConfigurator();

        protected EndpointParameters _parameters;


        public EndpointConfigurator(EndpointParameters parameters)
        {
            _parameters = parameters;
        }

        public abstract IBusControl Configure(EndpointBase endpoint);
    }

    public class RabbitMqEndpointConfigurator : EndpointConfigurator
    {

        public RabbitMqEndpointConfigurator()
            : base(EndpointParameters.Default)
        {
        }

        public RabbitMqEndpointConfigurator(EndpointParameters parameters)
            : base(parameters)
        {
            _parameters = parameters;
        }

        public override IBusControl Configure(EndpointBase endpoint)
        {
            endpoint.BusControl = MassTransit.Bus.Factory.CreateUsingRabbitMq(rabbit => configure(endpoint, rabbit));
            return endpoint.BusControl;
        }

        protected virtual void configure(EndpointBase endpoint, IRabbitMqBusFactoryConfigurator rabbit)
        {
            var parameters = _parameters ?? EndpointParameters.Default;

            var calculatedAddress = parameters.Address;
            if (!string.IsNullOrEmpty(parameters.VirtualHost))
            {
                calculatedAddress = new Uri(parameters.Address + parameters.VirtualHost + "/");
            }
            if (!string.IsNullOrEmpty(endpoint.QueueName))
            {
                calculatedAddress = new Uri(calculatedAddress + endpoint.QueueName);
            }

            endpoint.Host = rabbit.Host(calculatedAddress, hostConfigure =>
            {
                hostConfigure.Username(_parameters.Username);
                hostConfigure.Password(_parameters.Password);
            });
        }
    }


    public class RabbitMqConsumeEndpointConfigurator : EndpointConfigurator
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static RabbitMqConsumeEndpointConfigurator Default = new RabbitMqConsumeEndpointConfigurator();

        public RabbitMqConsumeEndpointConfigurator(EndpointParameters parameters = null) : base(parameters)
        {
        }

        public override IBusControl Configure(EndpointBase endpoint)
        {
            endpoint.BusControl = MassTransit.Bus.Factory.CreateUsingRabbitMq(rabbit => configure(endpoint, rabbit));
            return endpoint.BusControl;
        }

        protected virtual void configure(EndpointBase endpoint, IRabbitMqBusFactoryConfigurator rabbit)
        {
            var epc = endpoint as RequestReplyConsumeEndpoint;

            var parameters = _parameters ?? EndpointParameters.Default;

            var calculatedAddress = parameters.Address;
            if (!string.IsNullOrEmpty(parameters.VirtualHost))
            {
                calculatedAddress = new Uri(parameters.Address + parameters.VirtualHost + "/");
            }
            /*
            if (!string.IsNullOrEmpty(endpoint.QueueName))
            {
                calculatedAddress = new Uri(calculatedAddress + endpoint.QueueName);
            }
            */
            endpoint.Host = rabbit.Host(calculatedAddress, hostConfigure =>
            {
                hostConfigure.Username(parameters.Username);
                hostConfigure.Password(parameters.Password);
            });

            rabbit.ReceiveEndpoint(endpoint.Host, endpoint.QueueName, receiverConfigure =>
            {
                _logger.Info("Adding receive consumer: {0}", epc.RequestType);
                receiverConfigure.Consumer(epc.ConsumerType, t => epc.createConsumer());
            });
        }
    }

    public class RequestReplySendEndpoint<Request, Reply> : EndpointBase,
        IRequestReplySendEndpoint<Request, Reply>
        where Request : class
        where Reply : class
    {
        public class RequestReplySendEndpointConfigurator : EndpointConfigurator
        {
            public static RequestReplySendEndpointConfigurator Default = new RequestReplySendEndpointConfigurator(EndpointParameters.Default);
            public RequestReplySendEndpointConfigurator() : base(EndpointParameters.Default)
            {
            }

            public RequestReplySendEndpointConfigurator(EndpointParameters parameters)
                : base(parameters)
            {
                _parameters = parameters;
            }

            public override IBusControl Configure(EndpointBase endpoint)
            {
                endpoint.BusControl = MassTransit.Bus.Factory.CreateUsingRabbitMq(rabbit => configure(endpoint, rabbit));
                var e = endpoint as RequestReplySendEndpoint<Request, Reply>;

                var uri = new Uri(endpoint.CalculatedAddress + endpoint.QueueName);

                var parameters = _parameters ?? EndpointParameters.Default;
                e._client = new MessageRequestClient<Request, Reply>(
                    endpoint.BusControl, 
                    uri,
                    parameters.Timeout);

                return endpoint.BusControl;
            }

            protected virtual void configure(EndpointBase endpoint, IRabbitMqBusFactoryConfigurator rabbit)
            {
                var parameters = _parameters ?? EndpointParameters.Default;

                var calculatedAddress = parameters.Address;
                if (!string.IsNullOrEmpty(parameters.VirtualHost))
                {
                    calculatedAddress = new Uri(parameters.Address + parameters.VirtualHost + "/");
                }
                endpoint.CalculatedAddress = calculatedAddress;
                /*
                if (!string.IsNullOrEmpty(endpoint.QueueName))
                {
                    calculatedAddress = new Uri(calculatedAddress + endpoint.QueueName);
                }
                */
                
                endpoint.Host = rabbit.Host(calculatedAddress, hostConfigure =>
                {
                    hostConfigure.Username(parameters.Username);
                    hostConfigure.Password(parameters.Password);
                });
            }
        }

        private IRequestClient<Request, Reply> _client;

        public RequestReplySendEndpoint(string queueName)
            : base(RequestReplySendEndpointConfigurator.Default)
        {
            QueueName = queueName;
        }

        public async Task<Reply> SendAsync(Request request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            wire();

            var reply = await _client.Request(request, cancellationToken);

            return reply;
        }

        protected override void wire()
        {
            if (_client != null) return;

            var configurator = EndpointConfigurator ?? EndpointConfigurator.Default;
            configurator.Configure(this);

            BusHandle = BusControl.Start();
        }

        protected override void unwire()
        {
            if (BusHandle != null)
            {
                BusHandle.Stop();
                BusHandle.Dispose();
            }
        }

        public void Start()
        {
            wire();
        }

        public void Stop()
        {
            unwire();
        }
    }


    public interface IRequestReplyReceiveEndpoint<Request, Reply> : IMassTransitEndpoint
        where Request : class
        where Reply : class
    {
        Task<Reply> ProcessAsync(Request request);
    }

    public abstract class RequestReplyConsumeEndpoint : EndpointBase
    {
        public Type ConsumerType { get; set; }
        public Type RequestType { get; set; }
        public Type ReplyType { get; set; }

        public RequestReplyConsumeEndpoint(Type requestType, Type replyType, Type consumerType, EndpointConfigurator configurator) : base(configurator)
        {
            RequestType = requestType;
            ReplyType = replyType;
            ConsumerType = consumerType;
        }

        protected override void wire()
        {
        }

        protected override void unwire()
        {
        }

        public abstract IConsumer createConsumer();
    }

    public class RequestReplyConsumeEndpoint<Request, Reply> : RequestReplyConsumeEndpoint,
        IRequestReplyReceiveEndpoint<Request, Reply>
        where Request : class
        where Reply : class
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public class MessageReceivedEventArgs<Request, Reply> : EventArgs
        {
            public Request Message { get; private set; }
            public Reply Response { get; set; }

            public MessageReceivedEventArgs(Request request)
            {
                Message = request;
            }
        }

        public class Consumer : IConsumer<Request>
        {
            public ConsumeContext<Request> Context { get; set; }
            private Func<Consumer, ConsumeContext<Request>, Task<Reply>> _handler;

            public Consumer(Func<Consumer, ConsumeContext<Request>, Task<Reply>> handler)
            {
                _handler = handler;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                this.Context = context;
                var result = await _handler(this, context);
                context.RespondAsync(result);
            }
        }

        public event EventHandler<MessageReceivedEventArgs<Request, Reply>> MessageReceived;

        public override IConsumer createConsumer()
        {
            return new Consumer(handler);
        }

        protected async Task<Reply> handler(Consumer consumer, ConsumeContext<Request> context)
        {
            var reply = await ProcessAsync(context.Message);
            return reply;
        }

        private Consumer _consumer;

        public RequestReplyConsumeEndpoint(string queueName) : base(typeof (Request), typeof (Reply), typeof(Consumer), RabbitMqConsumeEndpointConfigurator.Default)
        {
            QueueName = queueName;
        }

        public async virtual Task<Reply> ProcessAsync(Request request)
        {
            if (MessageReceived != null)
            {
                var args = new MessageReceivedEventArgs<Request, Reply>(request);
                MessageReceived(this, args);
                return args.Response;

            }
            return null;
        }

        public void Start()
        {
            wire();
        }

        public void Stop()
        {
            unwire();
        }

        protected override void wire()
        {
            var configurator = EndpointConfigurator ?? RabbitMqConsumeEndpointConfigurator.Default;
            configurator.Configure(this);
            BusHandle = BusControl.Start();
        }

        protected override void unwire()
        {
            if (BusHandle != null)
            {
                BusHandle.Stop();
                BusHandle.Dispose();
            }
        }
    }
}

