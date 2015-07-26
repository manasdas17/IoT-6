using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Minions.Messages
{
    public class MinionsResponseMessage
    {
        public string Response;

        public MinionsResponseMessage()
        {
            
        }

        public MinionsResponseMessage(string response)
        {
            Response = response;
        }
    }
}
