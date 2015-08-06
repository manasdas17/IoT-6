using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Messages.Push
{
    public class ThingUpdatedMessage
    {
        public string Thing { get; private set; }

        public ThingUpdatedMessage(string thing)
        {
            Thing = thing;
        }
    }
}
