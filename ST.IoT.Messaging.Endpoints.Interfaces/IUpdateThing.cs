using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Endpoints.Interfaces
{
    public interface IUpdateThing
    {
        string UpdateThing(string thing_json);
    }
}
