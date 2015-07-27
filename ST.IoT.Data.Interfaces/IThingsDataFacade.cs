using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Interfaces
{
    public interface IThingsDataFacade
    {
        void Put(string request_json);
        string Get(string request_json);
    }
}
