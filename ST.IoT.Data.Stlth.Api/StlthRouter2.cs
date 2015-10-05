using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ST.IoT.Data.Stlth.Model;

namespace ST.IoT.Data.Stlth.Api
{
    public class StlthRouter23
    {
        public class RouteSegment
        {
            public string Element { get; set; }
            public Func<string, bool> CanHandle { get; set; } 
            public Func<string, string> Handle { get; set; }

            public RouteSegment(string element, 
                Func<string, bool> canHandle = null,
                Func<string, string> handle = null) 
            {
                Element = element;
                CanHandle = canHandle;
                Handle = handle;
            }

            public virtual bool IHandleThis(string segment)
            {
                if (segment == Element) return true;
                if (CanHandle == null) return false;
                return CanHandle(segment);
            }

            public string handle(
                string value)
            {
                return (Handle != null) ? Handle(value) : value;
            }
        }

        public class RoutePattern
        {
            
            public HttpMethod Method { get; set; }
            public RouteSegment[] Segments { get; set; }
            public Func<HttpRequestMessage, List<string>, Task<HttpResponseMessage>> Handler { get; set; } 

            public RoutePattern(HttpMethod method, RouteSegment[] segments, Func<HttpRequestMessage, List<string>, Task<HttpResponseMessage>> handler)
            {
                Method = method;
                Segments = segments;
                Handler = handler;
            }

            public bool IHandleThis(HttpMethod method, string[] segments)
            {
                if (segments.Length != Segments.Length) return false;

                var allCanHandle = Segments
                    .Select((s, i) => new {Segment = s, Index = i})
                    .All(x => x.Segment.IHandleThis(segments[x.Index]));
                return allCanHandle;
            }

            internal async Task<HttpResponseMessage> process(HttpRequestMessage request, string[] segments)
            {
                var parameters = new List<string>();
                for (var i = 0; i < segments.Length; i++)
                {
                    parameters.Add(Segments[i].handle(segments[i]));
                }
                return await Handler(request, parameters);
            }
        }

        private StlthDataClient _dataClient;

        private Dictionary<string, string> _nodeLabels;

        private RoutePattern[] _routes;

        public StlthRouter23(StlthDataClient dataClient)
        {
            _dataClient = dataClient;
            _nodeLabels = dataClient.NodeLabels.ToDictionary(i => i);

            _routes = new []
            {
                new RoutePattern(
                    HttpMethod.Post,  
                    new [] {
                        new RouteSegment("/"), 
                        new RouteSegment("{nodetype}", isNodeType, getNodeType),
                        new RouteSegment("/"),  
                        new RouteSegment("{id}", isId, getId)
                        },
                    getNodeByTypeAndId),

                new RoutePattern(
                    HttpMethod.Post, 
                    new [] {
                        new RouteSegment("/"),
                        new RouteSegment("{nodetype}", isNodeType, getNodeType),
                    },
                    getNodesOfType),
                    
                new RoutePattern(
                    HttpMethod.Post,
                    new [] {
                        new RouteSegment("/"),
                        new RouteSegment("{id}", isId, getId)
                    },
                    getNodeById),


            };
        }

        private bool isId(string value)
        {
            return true;
        }

        private string getId(string value)
        {
            return value;
        }

        private bool isNodeType(string value)
        {
            return _nodeLabels.ContainsKey(value);
        }

        private string getNodeType(string value)
        {
            return value;
        }

        public async Task<HttpResponseMessage> process(HttpRequestMessage request)
        {
            var segments = getSegments(request.RequestUri.Segments);
            var route = _routes.FirstOrDefault(r => r.IHandleThis(request.Method, segments));
            var response = route == null ? notImplemented() : await route.process(request, segments);
            return response;
        }

        private string[] getSegments(string[] urlSegments)
        {
            var segments = new List<string>();

            foreach (var segment in urlSegments)
            {
                if (segment.Length > 1 && segment.EndsWith("/"))
                {
                    segments.Add(segment.Substring(0, segment.Length - 1));
                    segments.Add("/");
                }
                else
                {
                    segments.Add(segment);
                }
            }

            return segments.ToArray();
        }
            
        async Task<HttpResponseMessage> getNodeById(HttpRequestMessage request, List<string> parameters)
        {
            var nodeId = parameters[1];
            var node = await _dataClient.GetNodeByIdAsync(nodeId);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(DescribeAsNeoJSON.describe(node))
            };

            return response;
        }

        async Task<HttpResponseMessage> getNodesOfType(HttpRequestMessage request, List<string> parameters)
        {
            var name = parameters[1];
            var node = await _dataClient.GetNodesOfTypeAsync(name);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(DescribeAsNeoJSON.describe(node))
            };

            return response;
        }

        Task<HttpResponseMessage> getNodeByTypeAndId(HttpRequestMessage request, List<string> parameters)
        {
            return null;
        }

        private HttpResponseMessage notImplemented()
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
