using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class CreateTenantStrategy : IStrategy<CreateTenantStrategy>, IStrategy
    {
        private IStlthDataClient _client;
        private string _tenantID;
        
        public CreateTenantStrategy(IStlthDataClient client, string tenantID)
        {
            _client = client;
            _tenantID = tenantID;
        }

        async Task<CreateTenantStrategy> IStrategy<CreateTenantStrategy>.ExecuteAsync()
        {
            _client.Connect();

            var id = await _client.AllocateIDsAsync();
            var query = _client
                .Cypher
                .Match(("(n:stlth:Tenants)"))
                .Create(string.Format("(t:stlth:Tenant:Root:{0} {{Name: '{0}', ID: {1}}})", _tenantID, id))
                .Create(string.Format("(mn:stlth:Tenant:Meta:{0})", _tenantID))
                .Create(string.Format("(dn:stlth:Tenant:Data:Root:{0})", _tenantID))
                .Create(string.Format("(n)-[:TENANT_INSTANCE {{Name: '{0}'}}]->(t)", _tenantID))
                .Create(string.Format("(t)-[:TENANT_META {{Name: '{0}'}}]->(mn)", _tenantID))
                .Create(string.Format("(t)-[:TENANT_DATA {{Name: '{0}'}}]->(dn)", _tenantID))

                .Create(string.Format("(tme:stlth:Tenant:Meta:Edges:{0})", _tenantID))
                .Create(string.Format("(mn)-[:TENANT_META_EDGES {{TenantName: '{0}'}}]->(tme)", _tenantID))
                .Create(string.Format("(tmn:stlth:Tenant:Meta:Nodes:{0})", _tenantID))
                .Create(string.Format("(mn)-[:TENANT_META_NODES {{TenantName: '{0}'}}]->(tmn)", _tenantID))
                .Create(string.Format("(tmr:stlth:Tenant:Meta:Rels:{0})", _tenantID))
                .Create(string.Format("(mn)-[:TENANT_META_RELS {{TenantName: '{0}'}}]->(tmr)", _tenantID))

                .Create(string.Format("(tde:stlth:Tenant:Data:Edges:{0})", _tenantID))
                .Create(string.Format("(dn)-[:TENANT_DATA_EDGES {{TenantName: '{0}'}}]->(tde)", _tenantID))
                .Create(string.Format("(tdn:stlth:Tenant:Data:Nodes:{0})", _tenantID))
                .Create(string.Format("(dn)-[:TENANT_DATA_NODES {{TenantName: '{0}'}}]->(tdn)", _tenantID))
                .Create(string.Format("(tdr:stlth:Tenant:Data:Rels:{0})", _tenantID))
                .Create(string.Format("(dn)-[:TENANT_DATA_RELS {{TenantName: '{0}'}}]->(tdr)", _tenantID));

                await query.ExecuteWithoutResultsAsync();

            return this;
        }

        async Task IStrategy.ExecuteAsync()
        {
            await ((IStrategy<CreateTenantStrategy>)this).ExecuteAsync();
        }
    }
}
