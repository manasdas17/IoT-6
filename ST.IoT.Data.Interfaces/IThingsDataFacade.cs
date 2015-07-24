using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Data.Model;

namespace ST.IoT.Data.Interfaces
{
    public interface IThingsDataFacade
    {
        void Put(Thing theThing);
        IEnumerable<Thing> GetMostRecentVersionsOfThing(string thingID, int count = 1);
    }
}
