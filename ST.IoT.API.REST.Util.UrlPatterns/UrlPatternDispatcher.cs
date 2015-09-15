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
            public Func<HttpRequestMessage, Task<HttpResponseMessage>> Handler { get; set; }


            public bool match(HttpRequestMessage request)
            {
                if (request.Method != Method)
                {
                    return false;
                }
                var segments = request.RequestUri.Segments;
                if (segments.Length < Parts.Length)
                {
                    return false;
                }
                var i = 0;
                foreach (var part in Parts)
                {
                    if (part != segments[i]) return false;
                    i++;
                }
                return true;
            }
        }

        private List<UriPattern> _patterns;

        public UrlPatternDispatcher(IEnumerable<UriPattern> patterns)
        {
            _patterns = new List<UriPattern>(patterns);
        }

        public UriPattern handle(HttpRequestMessage request)
        {
            foreach (var pattern in _patterns)
            {
                if (pattern.match(request))
                {
                    return pattern;
                }
            }
            return null;
        }
    }

}
