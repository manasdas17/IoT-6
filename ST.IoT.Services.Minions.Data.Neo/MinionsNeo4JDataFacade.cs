using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using ST.IoT.Services.Minions.Data.Interfaces;

namespace ST.IoT.Services.Minions.Data.Neo
{
    public class MinionsNeo4JDataFacade : IMinionsDataFacade
    {
        private GraphClient _client;

        public MinionsNeo4JDataFacade()
        {
        }

        public void Start()
        {
            connect();
        }

        public void Stop()
        {
            disconnect();
        }

        private void connect()
        {
        }

        private void disconnect()
        {

        }
    }
}
