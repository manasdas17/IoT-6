using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Minions.Messages
{
    public class MinionsRequestMessage
    {
        public string Request { get; set; }

        public MinionsRequestMessage()
        {
            
        }

        public MinionsRequestMessage(string request)
        {
            Request = request;
        }
    }
}
