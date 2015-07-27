using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Newtonsoft.Json.Linq;
using ST.IoT.Services.Interfaces;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Interfaces;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions
{
    public class MinionsService : IIoTService
    {
        private IMinionsReceiveRequestEndpoint _endpoint;
        private IMinionsDataService _dataService;

        private Dictionary<string, Func<JObject, MinionsRequestMessage, MinionsResponseMessage>> _handlers; 

        public MinionsService(IMinionsReceiveRequestEndpoint endpoint, IMinionsDataService dataService)
        {
            _endpoint = endpoint;
            _dataService = dataService;

            _endpoint.ReceivedRequestMessage += _endpoint_ReceivedRequestMessage;

            _handlers = new Dictionary<string, Func<JObject, MinionsRequestMessage, MinionsResponseMessage>>()
            {
                {"quote/for", quote_for}
            };
        }

        void _endpoint_ReceivedRequestMessage(object sender, MinionsRequestMessageReceivedEventArgs e)
        {
            try
            {
                var request = JObject.Parse(e.RequestMessage.Request);

                var action = request["action"].ToString();
                if (_handlers.ContainsKey(action))
                {
                    e.ResponseMessage = _handlers[action](request, e.RequestMessage);
                }
                else
                {
                    e.ResponseMessage = new MinionsResponseMessage(
                        HttpStatusCode.BadRequest,
                        "Invalid action: " + action);
                }
            }
            catch (Exception ex)
            {
                e.ResponseMessage = new MinionsResponseMessage(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        private MinionsResponseMessage quote_for(JObject request, MinionsRequestMessage message)
        {
            try
            {
                var name = request["name"];
                if (name == null)
                {
                    return new MinionsResponseMessage(
                        HttpStatusCode.BadRequest,
                        "Name field not specified");
                }
                var content = request["content"];
                if (content == null)
                {
                    return new MinionsResponseMessage(
                        HttpStatusCode.BadRequest,
                        "Content field not specified");
                }

                var result = _dataService.PutMinion(message);
                return result;

            }
            catch (Exception ex)
            {
                return new MinionsResponseMessage(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        public void Start()
        {
            _endpoint.Start();
        }

        public void Stop()
        {
            _endpoint.Stop();
        }
    }
}
