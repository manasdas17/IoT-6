using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.IoT.Messaging.Busses.Factories.Core
{
    public interface IEndpointFactoryInitializationParameters
    {
        string BaseAddress { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
