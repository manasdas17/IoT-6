using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Data.Stlth.Social.Model;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class AddThingToGroupCommand : StlthCommand
    {
        private Thing _thing;
        private WithGroupRequestBuilder _group;


        public AddThingToGroupCommand(WithGroupRequestBuilder group, Thing thing) : base()
        {
            _group = group;
            _thing = thing;
        }

        public Task<AddPersonToCommunityResult> ResultAsync => this.executeAsync();
        public AddPersonToCommunityResult Result => this.execute();


        protected override string createCommandBody()
        {
            throw new NotImplementedException();
        }

        public AddPersonToCommunityResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<AddPersonToCommunityResult> executeAsync()
        {
            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: string.Format("/Group/{0}/Member/Thing/{1}", _group.Group.ID, _thing.ID));

            var result = new AddPersonToCommunityResult(response);
            return result;
        }
    }
}
