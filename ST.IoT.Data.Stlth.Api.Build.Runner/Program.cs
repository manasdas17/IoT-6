using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Data.Stlth.Api.Strategies;

namespace ST.IoT.Data.Stlth.Api.Build.Runner
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {

            var db = new StlthDataClient();
            db.Connect();

            // give the service a little time to start
            Task.Delay(1000).Wait();

            var strategies = new List<IStrategy>()
            {
                new DeleteAllNodesAndEdgesStrategy(db),
                new CreateTopLevelElementsStrategy(db),

                new CreateMetaNodesStrategy(db),
                new CreateMetaEdgesStrategy(db),
                new CreateMetaRelsStrategy(db),
                new CreateTenantStrategy(db, "Stlth"),
                
                new LoadMetaNodesStrategy(db),
                new LoadMetaEdgesStrategy(db),
                new LoadMetaRelsStrategy(db),

                new BuildModelStlthCommunityStrategy(),
                //new AddModelDataStrategy(db),
            };

            try
            {
                strategies.ForEach(s => s.ExecuteAsync().Wait());

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
            }

            //db.NodeLabels.ForEach(l => Console.WriteLine(l));

            /*
            var restProcessor = new StlthRestApiRequestProcessor(db);

            //var response1 = restProcessor.handle(StlthRestRequestFactory.getNodeById("7")).Result;
            //var response2 = restProcessor.handle(StlthRestRequestFactory.getNodesByType("Person")).Result;
            //var response3 = restProcessor.handle(StlthRestRequestFactory.getNodeByTypeAndId("Person", "13")).Result;


            //_logger.Info(response);
            */

            //var client = new StlthDataClient();
            //var result = client.RelateAsync(StlthDataOperation.POST, "8", "17", StlthBuiltinRelNames.Friend, String.Empty).Result;

            db.Disconnect();

            Console.ReadLine();
        }
    }
}
