using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NLog;
using ST.IoT.API.REST.Util.UrlPatterns;
using ST.IoT.Data.Stlth.Api;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Stlth.Endpoints;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.Core
{

    public class StlthService : IStlthService, IStlthSocialService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IStlthDataClient _dataClient;

        public StlthService(IStlthDataClient dataClient)
        {
            _dataClient = dataClient;
        }

        public void Start()
        {
            _logger.Info("Started");
            _dataClient.InitializeAsync().Wait();
        }

        public void Stop()
        {
            _logger.Info("Stopped");
        }

        public async Task<NodeResult> GetNodeByIdAsync(string nodeID, string tenantID = null)
        {
            var nodeResult = await _dataClient.NodeGetAsync(nodeID, tenantID);
            return nodeResult;
        }

        public async Task<NodeResult> GetNodesOfTypeAsync(string nodeTypeName, int skip = 0, int limit =100)
        {
            var nodeResult = await _dataClient.GetNodesOfTypeAsync(nodeTypeName);
            return nodeResult;
        }

        public async Task<bool> IsNodeTypeAsync(string segment)
        {
            return await _dataClient.IsNodeTypeNameAsync(segment);
        }
        public async Task<bool> IsRelTypeAsync(string segment)
        {
            return await _dataClient.IsRelTypeNameAsync(segment);
        }


        public async Task<NodeResult> CreateNewNodeAsync(string nodeType, string data, string tenantID = null)
        {
            _logger.Info("Request to create a new node: '{0}', '{1}'", data, tenantID);

            var isNewType = false;

            if (!await IsNodeTypeAsync(nodeType))
            {
                // create new node type
                await _dataClient.MetaNodeAsync(StlthDataOperation.POST, nodeType, "{}");
                await _dataClient.LoadMetaNodesAsync();

                isNewType = true;
            }

            if (!isNewType)
            {
                // only see if it exists if we didn't just create the type
                var node = await _dataClient.QueryNodesAsync(nodeType, data, tenantID);
                if (node.HasResult)
                {
                    _logger.Info("Node already exists");
                    return new NodeResult(HttpStatusCode.Conflict);
                }
            }

            var result = await _dataClient.NodePostAsync(nodeType, data, tenantID);
            _logger.Info("Created new node: {0}", result);

            return result;
        }


        public async Task<NodeResult> CreateNewCommunityAsync(string communityData, string tenantID = null)
        {
            _logger.Info("Request to create a new community: '{0}', '{1}'", communityData, tenantID);

            var community = await _dataClient.QueryNodesAsync(StlthBuiltinNodeLabels.Community, communityData, tenantID);
            if (community.HasResult)
            {
                _logger.Info("Community already exists");
                return new NodeResult(HttpStatusCode.Conflict);
            }

            var result = await _dataClient.NodePostAsync(StlthBuiltinNodeLabels.Community, communityData, tenantID);
            _logger.Info("Created new community: {0}", result);

            return result;
        }

        public async Task<NodeResult> DeleteCommunityByIdAsync(string communityID, string tenantID = null)
        {
            _logger.Info("Request to delete a community by id: '{0}', '{1}'", communityID, tenantID);

            var queryResult = await _dataClient.QueryNodesAsync(StlthBuiltinNodeLabels.Community, "{'ID': '" + communityID + "'}", tenantID);
            if (!queryResult.HasResult)
            {
                _logger.Info("Community did not exist (or error): {0}", queryResult);
                return queryResult;
            }

            var result = await _dataClient.NodeDeleteAsync(queryResult.ID, tenantID);
            _logger.Info("Deleted community: {0}", result);

            return result;
        }

        public async Task<NodeResult> CreateNewPersonAsync(string personData, string tenantID = null)
        {
            _logger.Info("Request to create a new person: '{0}', '{1}'", personData, tenantID);

            var person = JObject.Parse(personData);
            // should look up what the key is: we will use EMailAddress
            var key = person["EmailAddress"].ToString();

            var r1 = await _dataClient.QueryNodesAsync(StlthBuiltinNodeLabels.Person, "{'EmailAddress': '" + key + "'}", tenantID);
            if (r1.HasResult)
            {
                _logger.Info("Person already exists");
                return new NodeResult(HttpStatusCode.Conflict);
            }

            var result = await _dataClient.NodePostAsync(StlthBuiltinNodeLabels.Person, personData, tenantID);
            _logger.Info("Created new Person: {0}", result);

            return result;
        }

        public async Task<NodeResult> DeletePersonByIdAsync(string personID, string tenantID = null)
        {
            _logger.Info("Request to delete a person by id: '{0}', '{1}'", personID, tenantID);

            var queryResult = await _dataClient.QueryNodesAsync(StlthBuiltinNodeLabels.Person, "{'ID': '" + personID + "'}", tenantID);
            if (!queryResult.HasResult)
            {
                _logger.Info("Person did not exist (or error): {0}", queryResult);
                return queryResult;
            }

            var result = await _dataClient.NodeDeleteAsync(queryResult.ID, tenantID);
            _logger.Info("Deleted person: {0}", result);

            return result;
        }

        public async Task<NodeResult> CreateNewThingAsync(string thingData, string tenantID = null)
        {
            _logger.Info("Request to create a new thing: '{0}', '{1}'", thingData, tenantID);

            var thing = JObject.Parse(thingData);
            // should look up what the key is: we will use EMailAddress
            var key = thing["Name"].ToString();

            var r1 = await _dataClient.QueryNodesAsync(StlthBuiltinNodeLabels.Thing, "{'Name': '" + key + "'}", tenantID);
            if (r1.HasResult)
            {
                _logger.Info("Thing already exists");
                return new NodeResult(HttpStatusCode.Conflict);
            }

            var result = await _dataClient.NodePostAsync(StlthBuiltinNodeLabels.Thing, thingData, tenantID);
            _logger.Info("Created new Thing: {0}", result);

            return result;
        }

        public async Task<RelateResult> RelateNodesAsync(string nodeId1, string relName, string nodeId2, string tenantID = null)
        {
            var result = await _dataClient.RelPostAsync(nodeId1, relName, nodeId2, tenantID);

            return result;
        }
    }
}
