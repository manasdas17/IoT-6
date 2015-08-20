using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ST.IoT.Data.Stlth.Api.Strategies;

namespace ST.IoT.Data.Stlth.Api.Tests
{
    [TestClass]
    public class CreateReferenceModelTest
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void CreateReferenceModel()
        {
            var db = new StlthDataClient();
            db.Connect();

            var strategies = new List<IStrategy>()
            {
                new DeleteAllNodesAndEdgesStrategy(db),
                new CreateMetaNodesStrategy(db),
                new LoadMetaNodesStrategy(db),
                new AddModelUsersStrategy(db),
            };
            strategies.ForEach(s => s.ExecuteAsync().Wait());

            db.Disconnect();
        }
    }
}
