using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.API.REST.Util.UrlPatterns;
using ST.IoT.Data.Stlth.Api;
using ST.IoT.Data.Stlth.Model;

namespace ST.IoT.Services.Stlth.Core
{
    internal class CoreRequestProcessor : StlthRequestProcessor
    {
        private UrlPatternDispatcher _patternDispatcher;
        private IStlthService _service;


        public CoreRequestProcessor(IStlthService service)
        {
            _service = service;
            _patternDispatcher = new UrlPatternDispatcher(
                new[]
                {
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "{id}"},
                        Handler = getAnyNodeByIdAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "nodes/", "{{nodetype}}"},
                        PartLookup = analyzeSegment,
                        Handler = getNodesOfTypeAsync
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Put,
                        Parts = new[] {"/", "nodes/", "{{nodetype}}"},
                        PartLookup = analyzeSegment,
                        Handler = getNodesOfTypeAsync
                    },
                }, null);
        }

        public override bool willHandle(HttpRequestMessage request)
        {
            var uri = request.RequestUri.Host.ToString().ToLower();
            return uri.StartsWith("core.api.stlth.");
        }

        public async override Task<HttpResponseMessage> handleAsync(HttpRequestMessage request)
        {
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
            var trimmed = urlSegments.Select(s => s.TrimEnd(new char[] { '/' })).ToArray();
            var nonEmpty = trimmed.Where(s => s.Length > 0).ToArray();
            return nonEmpty;
        }

        private bool analyzeSegment(string code, string segment)
        {
            switch (code)
            {
                case "nodetype":
                    {
                        return _service.IsNodeTypeAsync(segment).Result;
                    }
                    break;
            }
            return false;
        }

        private async Task<HttpResponseMessage> getAnyNodeByIdAsync(HttpRequestMessage request, string[] segments)
        {
            var id = request.RequestUri.Segments[1];
            var result = await _service.GetNodeByIdAsync(id);

            return new HttpResponseMessage(result.StatusCode)
            {
                Content = getContentForResult(result)
            };
        }

        public async Task<HttpResponseMessage> getNodesOfTypeAsync(HttpRequestMessage request, string[] segments)
        {
            var result = await _service.GetNodesOfTypeAsync(request.RequestUri.Segments[2]);
            return new HttpResponseMessage(result.StatusCode)
            {
                Content = getContentForResult(result, true)
            };
        }
    }
}
