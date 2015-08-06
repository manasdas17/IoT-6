using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.API.REST.PushRequestHttpHandler
{
    public class ChunkedResponseStreamForMinion : StreamContent
    {
        public ChunkedResponseStreamForMinion(Stream stream) : base(stream)
        {
            
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
