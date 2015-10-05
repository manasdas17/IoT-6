using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P
{
    public interface ISupernodeService
    {
        void Initialize();
        void Shutdown();

        void Announce();
    }
}
