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
    public interface IStlthService
    {
        void Start();
        void Stop();

        Task<NodeResult> GetNodeByIdAsync(string id, string tenantID = null);
        Task<bool> IsNodeTypeAsync(string segment);
        Task<NodeResult> GetNodesOfTypeAsync(string nodeTypeName, int skip = 0, int limit = 100);
        Task<bool> IsRelTypeAsync(string segment);
    }
}