using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Newtonsoft.Json.Linq;
using NLog;

namespace ST.IoT.Services.Stlth.Data.Layer.Neo
{
    public class Neo4jDataFacade
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private GraphClient _client;

        public enum Operation
        {
            PUT,
            GET,
            POST,
            DELETE
        }

        public Neo4jDataFacade()
        {
            
        }

        public JObject GetNode(string NodeType, string nodeID)
        {
            return null;
        }

        public void PutNode(string nodeType, string nodeID, string data)
        {
            
        }

        public void PostNode(string nodeType, string nodeID, string data)
        {

        }

        public IEnumerable<JObject> GetNodeTypeNames()
        {
            return null;
        }

        private void connect()
        {
            _logger.Info("Connecting");
            if (_client == null)
            {
                _logger.Info("Creating GraphClient");
                _client = new GraphClient(new Uri("http://neo4j:stlth@localhost:7474/db/data"));
            }
            try
            {
                if (!_client.IsConnected)
                {
                    _logger.Info("Not connected: attempting to connect");
                    try
                    {
                        _client.Connect();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Hard error");
                        _logger.Error(ex.Message);
                    }
                    _logger.Info("Connected");
                }
            }
            catch (AggregateException aex)
            {
                _logger.Error(aex.InnerExceptions.First().Message);
                throw aex.InnerExceptions.First();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            finally
            {
                _logger.Info("Leaving connect");
            }
        }
    }
}
