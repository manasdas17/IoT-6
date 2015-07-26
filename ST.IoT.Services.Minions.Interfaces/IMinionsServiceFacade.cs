using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Interfaces
{
    public interface IMinionsServiceFacade
    {
        void Start();
        void Stop();

        Task<MinionsResponseMessage> ProcessRequestAsync(MinionsRequestMessage request);
    }
}
