using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Stlth.Data.Layer.Neo;

namespace ST.IoT.Services.Stlth.Data.NeoReferenceCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Neo4jDataFacade();
            //db.PutNode();
        }
    }
}
