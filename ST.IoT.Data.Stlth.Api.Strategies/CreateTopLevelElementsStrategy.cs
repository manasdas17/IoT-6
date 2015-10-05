using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using ST.IoT.Stlth.Data.Neo4jUtils;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateTopLevelElementsStrategy : IStrategy
    {
        private readonly IStlthDataClient _client;

        public CreateTopLevelElementsStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            string queryText = "";
            var query = _client.Cypher
                .Create("(ur:stlth:UberRoot)")
                .Create("(sg:stlth:Globals)")
                .Create("(ur)-[rsg:STLTH_GLOBALS]->(sg)")
                .Create(
                    "(sguid:stlth:Global:UniqueIDs {count: 0})  create (sg)-[rsguid:GLOBAL_ID_ALLOCATOR]->(sguid)")
                .Create("(tenants:stlth:Tenants) create (ur)-[rurt:TENANTS]->(tenants)")
                .Create("(mr:stlth:Global:Meta:Root) create (sg)-[rsgmr:META_ROOT]->(mr)")
                .Create("(mrn:stlth:Global:Meta:Root:Nodes) create (mr)-[:META_ROOT_NODES]->(mrn)")
                .Create("(mre:stlth:Global:Meta:Root:Edges) create (mr)-[:META_ROOT_EDGES]->(mre)")
                .Create("(mrr:stlth:Global:Meta:Root:Rels) create (mr)-[:META_ROOT_RELS]->(mrr)");
            QueryExecutor.executeWithoutResults(query);
            /*
            await _client.ExecuteCypherAsync(string.Format(
                @"
create (ur:stlth:UberRoot)
create (sg:stlth:Globals) create (ur)-[rsg:STLTH_GLOBALS]->(sg)
create (sguid:stlth:Global:UniqueIDs {{count: 0}})  create (sg)-[rsguid:GLOBAL_ID_ALLOCATOR]->(sguid)
create (tenants:stlth:Tenants) create (ur)-[rurt:TENANTS]->(tenants)
create (mr:stlth:Global:Meta:Root) create (sg)-[rsgmr:META_ROOT]->(mr)
create (mrn:stlth:Global:Meta:Root:Nodes) create (mr)-[:META_ROOT_NODES]->(mrn)
create (mre:stlth:Global:Meta:Root:Edges) create (mr)-[:META_ROOT_EDGES]->(mre)
create (mrr:stlth:Global:Meta:Root:Rels) create (mr)-[:META_ROOT_RELS]->(mrr)
", _client.TenantID));
*/
        }
    }
}
