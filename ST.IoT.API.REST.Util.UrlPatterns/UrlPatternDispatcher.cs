using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.API.REST.Util.UrlPatterns
{
    public class UrlPatternDispatcher
    {
        public class UriPattern
        {
            public HttpMethod Method { get; set; }
            public string[] Parts { get; set; }
            public Func<HttpRequestMessage, string[], Task<HttpResponseMessage>> Handler { get; set; }
            public Func<string, string, bool> PartLookup { get; set; } 

            public bool match(HttpRequestMessage request, string[] segments, Func<string, string, bool> segmentEvaluator)
            {
                if (request.Method != Method)
                {
                    return false;
                }
                //var segments = request.RequestUri.Segments;
                if (segments.Length != Parts.Length)
                {
                    return false;
                }
                /*
                for (var i = 0; i< Parts.Length; i++)
                {
                    var part = Parts[i];
                    if (part.StartsWith("{{") && part.EndsWith("}}") && PartLookup != null)
                        return PartLookup(part.Substring(2, part.Length - 4), segments[i]);
                    if (part.StartsWith("{") && part.EndsWith("}")) continue;
                    if (part != segments[i]) return false;
                }
                */
                for (var i = 0; i < Parts.Length; i++)
                {
                    if (Parts[i] != segments[i]) continue;
                    if (Parts[i].StartsWith("{") && Parts[i].EndsWith("}"))
                    {
                        if (!segmentEvaluator(Parts[i], segments[i])) return false;
                    }
                }

                return true;
            }
        }

        private List<UriPattern> _patterns;

        public enum SegmentEvaluationResult
        {
            Any,
            NodeId,
            NodeType,
            RelationshipType
        }

        private readonly Func<string, string, bool> _segmentEvaluator;

        public UrlPatternDispatcher(IEnumerable<UriPattern> patterns, Func<string, string, bool> segmentEvaluator)
        {
            _patterns = new List<UriPattern>(patterns);
            _segmentEvaluator = segmentEvaluator;
        }

        public UriPattern getHandler(HttpRequestMessage request, string[] segments)
        {
            foreach (var pattern in _patterns)
            {
                if (pattern.match(request, segments, _segmentEvaluator))
                {
                    return pattern;
                }
            }
            return null;
        }
    }

}
