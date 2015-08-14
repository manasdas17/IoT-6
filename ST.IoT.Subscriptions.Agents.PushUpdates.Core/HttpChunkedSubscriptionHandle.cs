using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Subscriptions.Agents.PushUpdates.Core
{
    public class HttpChunkedSubscriptionHandle : SubscriptionHandle
    {
        public PushStreamContent PushStream { get; set; }

        private StreamWriter _writer;
        private Stream _stream;

        public HttpChunkedSubscriptionHandle()
        {
            PushStream = new PushStreamContent(
                (stream, content, context) =>
                {
                    _stream = stream;

                    try
                    {
                        _writer = new StreamWriter(_stream);
                    }
                    catch (Exception ex)
                    {
                    }
                });
        }

        public override void cancel()
        {
            if (_writer != null)
            {
                _writer.Close();
            }
            if (_stream != null)
            {
                _stream.Close();
            }
        }

        public override void pushUpdate(string thing)
        {
            if (_writer == null) throw new Exception("Response channel not open");
            _writer.Write(thing);
            _writer.Flush();
        }
    }
}
