using ST.IoT.API.REST.Util.UrlPatterns;
using ST.IoT.Data.Stlth.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;

namespace ST.IoT.Services.Stlth.Core
{
    internal class SocialRequestProcessor : StlthRequestProcessor
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private UrlPatternDispatcher _patternDispatcher;
        private IStlthService _service;


        public SocialRequestProcessor(IStlthService service)
        {
            _service = service;
            _patternDispatcher = new UrlPatternDispatcher(
                new[]
                {
                    /*
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"{id}"},
                        Handler = getAnyNodeByIdAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"Community"},
                        Handler = getAllCommunities
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"Community"},
                        Handler = addNewCommunityAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"Person"},
                        Handler = addNewPersonAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"Thing"},
                        Handler = addNewThingAsync
                    },
                    */

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"{nodeid}"},
                        Handler = getAnyNodeByIdAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"{nodename}"},
                        Handler = getNodesOfType
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"{any}"},
                        Handler = createNewNodeOfTypeAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Put,
                        Parts = new[] {"{nodeid}"},
                        Handler = updateNode
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"{nodename}", "{nodeid}"},
                        Handler = getNodeOfTypeWithId
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Put,
                        Parts = new[] {"{nodename}", "{nodeid}"},
                        Handler = updateNodeOfTypeWithId
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Delete,
                        Parts = new[] {"{nodename}", "{nodeid}"},
                        Handler = deleteNodeOfTypeWithId
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"{nodename}", "{nodeid}", "{relname}" },
                        Handler = getNodesInRelationFromNodeWithTypeId
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Delete,
                        Parts = new[] {"{nodename}", "{nodeid}", "{relname}" },
                        Handler = deleteRelationFromNodeWithTypeAndId
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"{nodename}", "{nodeid}", "{nodename}", "{nodeid}" },
                        Handler = relateTwoNodes
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Delete,
                        Parts = new[] {"{nodename}", "{nodeid}", "{nodename}", "{nodeid}" },
                        Handler = unrelateTwoNodes
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"{nodename}", "{nodeid}", "{relname}", "{nodename}", "{nodeid}" },
                        Handler = relateTwoNodesWithExplicitRelation
                    },

                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Post,
                        Parts = new[] {"{nodename}", "{nodeid}", "{relname}", "{nodename}", "{nodeid}" },
                        Handler = unrelateTwoNodesWithExplicitRelation
                    },
                }, nodeEvaluator);

        }

        private bool nodeEvaluator(string tag, string segment)
        {
            switch (tag)
            {
                case "{any}":
                {
                    return true;
                }
                break;

                case "{nodeid}":
                {
                    long lid;
                    Guid guidid;
                    return long.TryParse(segment, out lid) || Guid.TryParse(segment, out guidid);
                }

                case "{nodename}":
                {
                    return _service.IsNodeTypeAsync(segment).Result;
                }
                break;

                case "{relname}":
                {
                    return _service.IsRelTypeAsync(segment).Result;
                }
                break;
            }
            return false;
        }

        public override bool willHandle(HttpRequestMessage request)
        {
            var uri = request.RequestUri.Host.ToString().ToLower();
            return uri.StartsWith("social.api.stlth.");
        }

        public async override Task<HttpResponseMessage> handleAsync(HttpRequestMessage request)
        {
            _logger.Info("handleAsync");

            HttpResponseMessage response = null;

            var segments = getSegments(request);

            var handler = _patternDispatcher.getHandler(request, segments);
            if (handler == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                response = await handler.Handler(request, segments);
            }

            return response;
        }

        private string[] getSegments(HttpRequestMessage request)
        {
            var urlSegments = request.RequestUri.Segments;
            var trimmed = urlSegments.Select(s => s.TrimEnd(new char[] {'/'})).ToArray();
            var nonEmpty = trimmed.Where(s => s.Length > 0).ToArray();
            return nonEmpty;
        }


        private Task<HttpResponseMessage> getAnyNodeByIdAsync(HttpRequestMessage request, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> getNodesOfType(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> createNewNodeOfTypeAsync(HttpRequestMessage request, string[] segments)
        {
            return await processAsync(request,
                async (_, data, tenantID, service) =>
                {
                    return await service.CreateNewNodeAsync(segments[0], data, tenantID);

                });
        }

        private async Task<HttpResponseMessage> processAsync(HttpRequestMessage request, Func<string[], string, string, IStlthSocialService, Task<NodeResult>> action)
        {
            var social = _service as IStlthSocialService;
            var data = await request.Content.ReadAsStringAsync();
            var tenantID = getTenantId(request);
            var result = await action(request.RequestUri.Segments, data, tenantID, social);
            return new HttpResponseMessage(result.StatusCode)
            {
                Content = new StringContent(result.ToString())
            };
        }
        private async Task<HttpResponseMessage> processRelAsync(HttpRequestMessage request, Func<string[], string, string, IStlthSocialService, Task<RelateResult>> action)
        {
            var social = _service as IStlthSocialService;
            var data = await request.Content.ReadAsStringAsync();
            var tenantID = getTenantId(request);
            var result = await action(request.RequestUri.Segments, data, tenantID, social);
            return new HttpResponseMessage(result.StatusCode)
            {
                Content = new StringContent(result.ToString())
            };
        }

        private Task<HttpResponseMessage> updateNode(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> getNodeOfTypeWithId(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> updateNodeOfTypeWithId(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> deleteNodeOfTypeWithId(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> getNodesInRelationFromNodeWithTypeId(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private Task<HttpResponseMessage> deleteRelationFromNodeWithTypeAndId(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> relateTwoNodes(HttpRequestMessage request, string[] segments)
        {
            return await processRelAsync(request,
                async (_, data, tenantID, service) =>
                {
                    return await service.RelateNodesAsync(segments[1], null, segments[3]);

                });
        }

        private async Task<HttpResponseMessage> relateTwoNodesWithExplicitRelation(HttpRequestMessage request, string[] segments)
        {
            return await processRelAsync(request,
                async (_, data, tenantID, service) =>
                {
                    return await service.RelateNodesAsync(segments[1], segments[2], segments[4]);

                });
        }

        private Task<HttpResponseMessage> unrelateTwoNodes(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

        /*
        private HttpResponseMessage processLength2(HttpRequestMessage request)
        {
            var segment = request.RequestUri.Segments[1];

            // this is either an id, or a name of an object class

            if (_service.IsMetaNodeType(segment))
            {
                return processMetaNode(request, segment);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private HttpResponseMessage processMetaNode(HttpRequestMessage request, string metaNodeName)
        {
            if (request.Method == HttpMethod.Get)
            {
                return getNodesOfType(request, metaNodeName);
            }
            if (request.Method == HttpMethod.Post)
            {
                return createNewNodeOfType(request, metaNodeName);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private HttpResponseMessage createNewNodeOfType(HttpRequestMessage request, string metaNodeName)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage getNodesOfType(HttpRequestMessage request, string metaNodeName)
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> getAnyNodeByIdAsync(HttpRequestMessage request)
        {
            var id = request.RequestUri.Segments[1];
            var result = await _service.GetNodeByIdAsync(id, tenantID: getTenantId(request));

            return new HttpResponseMessage(result.StatusCode)
            {
                Content = getContentForResult(result)
            };
        }

        private async Task<HttpResponseMessage> addNewCommunityAsync(HttpRequestMessage request)
        {
            var social = _service as IStlthSocialService;

            var communityData = await request.Content.ReadAsStringAsync();
            var result = await social.CreateNewCommunityAsync(communityData, getTenantId(request));

            return new HttpResponseMessage(result.StatusCode)
            {
                Content = new StringContent(result.ToString())
            };
        }

        private async Task<HttpResponseMessage> getAllCommunities(HttpRequestMessage request)
        {
            var result = await _service.GetNodesOfTypeAsync("Community");
            return new HttpResponseMessage(result.StatusCode)
            {
                //Content = getContentForResult(result, true)
            };
        }

        private async Task<HttpResponseMessage> addNewPersonAsync(HttpRequestMessage request)
        {
            var social = _service as IStlthSocialService;

            var personData = await request.Content.ReadAsStringAsync();
            var result = await social.CreateNewPersonAsync(personData, getTenantId(request));

            return new HttpResponseMessage(result.StatusCode)
            {
                Content = new StringContent(result.ToString())
            };
        }
        private async Task<HttpResponseMessage> addNewThingAsync(HttpRequestMessage request)
        {
            var social = _service as IStlthSocialService;

            var personData = await request.Content.ReadAsStringAsync();
            var result = await social.CreateNewThingAsync(personData, getTenantId(request));

            return new HttpResponseMessage(result.StatusCode)
            {
                Content = new StringContent(result.ToString())
            };
        }
        */

        private Task<HttpResponseMessage> unrelateTwoNodesWithExplicitRelation(HttpRequestMessage arg, string[] segments)
        {
            throw new NotImplementedException();
        }

    }
}
