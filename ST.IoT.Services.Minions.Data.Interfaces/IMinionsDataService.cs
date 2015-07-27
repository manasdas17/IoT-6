using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Data.Interfaces
{
    public interface IMinionsDataService
    {
        MinionsResponseMessage PutMinion(MinionsRequestMessage request);
        MinionsResponseMessage GetMinion(MinionsRequestMessage request);
    }
}
