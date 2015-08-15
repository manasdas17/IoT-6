using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Busses.Factories.Core
{
    public interface ISendEndpoint
    {
        Task SendAsync<T>(T message) where T : class;
    }
}
