using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Interfaces.Servers
{
    public interface IHostableService
    {
        void Start();
        void Stop();
    }
}
