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
    public interface IStlthSocialService
    {
        Task<NodeResult> CreateNewCommunityAsync(string communityName, string tenantID = null);
        Task<NodeResult> DeleteCommunityByIdAsync(string communityID, string tenantID = null);
        Task<NodeResult> CreateNewPersonAsync(string personData, string tenantID = null);
        Task<NodeResult> DeletePersonByIdAsync(string personID, string tenantID = null);
        Task<NodeResult> CreateNewThingAsync(string thingName, string tenantID = null);


        Task<NodeResult> CreateNewNodeAsync(string nodeType, string data, string tenantID = null);
        Task<RelateResult> RelateNodesAsync(string nodeId1, string relName, string nodeId2, string tenantID = null);
    }
}