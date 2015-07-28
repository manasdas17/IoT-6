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
using NLog;
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

        private static Logger _logger = LogManager.GetCurrentClassLogger();


        public MinionsService(IMinionsReceiveRequestEndpoint endpoint, IMinionsDataService dataService)
        {
            _logger.Info("Configuring minions service");

            _endpoint = endpoint;
            _dataService = dataService;

            _endpoint.ReceivedRequestMessage += _endpoint_ReceivedRequestMessage;

            _handlers = new Dictionary<string, Func<JObject, MinionsRequestMessage, MinionsResponseMessage>>()
            {
                {"quote/for", quote_for},
                {"get/latest/quote/for", get_latest_quote_for },
                {"get/quotes/for", get_quotes_for },
            };

            _logger.Info("Done configuration");
        }

        public void Start()
        {
            _endpoint.Start();
        }

        public void Stop()
        {
            _endpoint.Stop();
        }


        void _endpoint_ReceivedRequestMessage(object sender, MinionsRequestMessageReceivedEventArgs e)
        {
            _logger.Info("Received a minions request");
            _logger.Info(e.RequestMessage);

            try
            {
                var request = JObject.Parse(e.RequestMessage.Request);

                var action = request["action"].ToString();
                if (_handlers.ContainsKey(action))
                {
                    _logger.Info("Calling handler");
                    e.ResponseMessage = _handlers[action](request, e.RequestMessage);
                    _logger.Info("Got response from handler");
                    _logger.Info(e.ResponseMessage);
                }
                else
                {
                    _logger.Warn("Unknown action: " + action);
                    e.ResponseMessage = new MinionsResponseMessage(
                        HttpStatusCode.BadRequest,
                        "Invalid action: " + action);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
                    _logger.Warn("name field not specified");
                    return buildPutError(request, HttpStatusCode.BadRequest, "Name field not specified");
                }
                var content = request["content"];
                if (content == null)
                {
                    _logger.Warn("Content field not specified");
                    return buildPutError(request, HttpStatusCode.BadRequest, "Content field not specified");
                }

                MinionsResponseMessage minion_response = null;
                MinionsResponseMessage result = null;

                try
                {
                    _logger.Info("Putting the minion");
                    minion_response = _dataService.PutMinion(message);
                    _logger.Info("Out of putting the minion");


                    if (minion_response.StatusCode != HttpStatusCode.OK)
                    {
                        result = buildPutError(request, minion_response);
                    }
                    else
                    {
                        result = buildPutSuccess(request, minion_response);
                    }
                }
                catch (Exception ex2)
                {
                    result = buildPutError(request, HttpStatusCode.InternalServerError, ex2.Message);
                }


                return result;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new MinionsResponseMessage(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        private MinionsResponseMessage get_latest_quote_for(JObject request, MinionsRequestMessage message)
        {
            try
            {
                var name = request["name"];
                if (name == null)
                {
                    _logger.Warn("name field not specified");
                    return buildPutError(request, HttpStatusCode.BadRequest, "Name field not specified");
                }

                MinionsResponseMessage minion_response = null;
                MinionsResponseMessage result = null;

                try
                {
                    // transmogrophy request a little

                    var meta = request["Meta"];
                    if (meta == null)
                    {
                        request["Meta"] = JObject.Parse("{}");
                    }

                    request["Meta"]["Paging"] = JObject.Parse(("{\"Limit\": 1}"));

                    message.Request = request.ToString();

                    _logger.Info("Getting the minion's quotes");
                    minion_response = _dataService.GetMinion(message);
                    _logger.Info("Out of getting the minion states");

                    if (minion_response.StatusCode != HttpStatusCode.OK)
                    {
                        result = buildGetError(request, minion_response);
                    }
                    else
                    {
                        result = buildGetSuccess(request, minion_response, true);
                    }
                }
                catch (Exception ex2)
                {
                    result = buildGetError(request, HttpStatusCode.InternalServerError, ex2.Message);
                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new MinionsResponseMessage(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        private MinionsResponseMessage get_quotes_for(JObject request, MinionsRequestMessage message)
        {
            try
            {
                var name = request["name"];
                if (name == null)
                {
                    _logger.Warn("name field not specified");
                    return buildPutError(request, HttpStatusCode.BadRequest, "Name field not specified");
                }

                MinionsResponseMessage minion_response = null;
                MinionsResponseMessage result = null;

                try
                {
                    // transmogrophy request a little

                    var meta = request["Meta"];
                    if (meta == null)
                    {
                        request["Meta"] = JObject.Parse("{}");
                    }

                    request["Meta"]["Paging"] = JObject.Parse(("{\"Limit\": 500}"));

                    message.Request = request.ToString();

                    _logger.Info("Getting the minion's quotes");
                    minion_response = _dataService.GetMinion(message);
                    _logger.Info("Out of getting the minion states");

                    if (minion_response.StatusCode != HttpStatusCode.OK)
                    {
                        result = buildGetError(request, minion_response);
                    }
                    else
                    {
                        result = buildGetSuccess(request, minion_response, true);
                    }
                }
                catch (Exception ex2)
                {
                    result = buildGetError(request, HttpStatusCode.InternalServerError, ex2.Message);
                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new MinionsResponseMessage(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        private MinionsResponseMessage buildPutSuccess(JObject request, MinionsResponseMessage data_response)
        {
            var r = new MinionsResponseMessage(data_response.StatusCode, wrapPutSuccess(request, data_response.Response));
            return r;
        }

        private MinionsResponseMessage buildPutError(JObject request, MinionsResponseMessage data_response)
        {
            var r = new MinionsResponseMessage(data_response.StatusCode, wrapPutError(request, data_response.StatusCode, data_response.Response));
            return r;
        }

        private MinionsResponseMessage buildPutError(JObject request, HttpStatusCode code, string innerMessage)
        {
            var r = new MinionsResponseMessage(code, wrapPutError(request, code, innerMessage));
            return r;
        }

        private string wrapPutSuccess(JObject request, string message)
        {
            var template = File.ReadAllText("put_minions_success_template.json");
            var result = template.Replace("{code}", string.Format("{0:d} OK", HttpStatusCode.OK))
                .Replace("{request}", request.ToString())
                .Replace("{detail}", message);

            try
            {
                return JObject.Parse(result).ToString();
            }
            catch (Exception)
            {
                return result;
            }
        }

        private string wrapPutError(JObject request, HttpStatusCode code, string message)
        {
            var template = File.ReadAllText("put_minions_error_template.json");
            var result = template.Replace("{code}", string.Format("{0:d} {1}", code, code.ToString()))
                .Replace("{request}", request.ToString())
                .Replace("{detail}", message);
            return JObject.Parse(result).ToString();
        }

        private MinionsResponseMessage buildGetSuccess(JObject request, MinionsResponseMessage data_response, bool single)
        {
            var r = new MinionsResponseMessage(data_response.StatusCode, wrapGetSuccess(request, data_response.Response, single));
            return r;
        }

        private MinionsResponseMessage buildGetError(JObject request, MinionsResponseMessage data_response)
        {
            var r = new MinionsResponseMessage(data_response.StatusCode, wrapGetError(request, data_response.StatusCode, data_response.Response));
            return r;
        }

        private MinionsResponseMessage buildGetError(JObject request, HttpStatusCode code, string innerMessage)
        {
            var r = new MinionsResponseMessage(code, wrapGetError(request, code, innerMessage));
            return r;
        }

        private string wrapGetSuccess(JObject request, string message, bool single)
        {
            var template = File.ReadAllText("get_minions_success_template.json");
            var result = template.Replace("{code}", string.Format("{0:d} OK", HttpStatusCode.OK))
                .Replace("{request}", request.ToString())
                .Replace("{detail}", buildGetDetail(request, message, single));

            try
            {
                return JObject.Parse(result).ToString();
            }
            catch (Exception)
            {
                return result;
            }
        }

        private string buildGetDetail(JObject request, string message, bool single)
        {
            var template = File.ReadAllText("get_minion_detail_single.json");
            var jo = JObject.Parse(message);

            var items = new List<string>();
            var results = jo["Results"].ToArray();
            foreach (var r in results)
            {
                var t2 = string.Copy(template);
                var val = t2.Replace("{thing}", request["name"].ToString())
                    .Replace("{content}", r.ToString());
                items.Add(val);
            }

            return "[" + string.Join(",", items) + "]";
        }

        private string wrapGetError(JObject request, HttpStatusCode code, string message)
        {
            var template = File.ReadAllText("put_minions_error_template.json");
            var result = template.Replace("{code}", string.Format("{0:d} {1}", code, code))
                .Replace("{request}", request.ToString())
                .Replace("{detail}", code == HttpStatusCode.NotFound ? "Where is my thing? Because we could not find it :(" : message);
            var formatted = JObject.Parse(result).ToString();
            return formatted;
        }
    }
}
