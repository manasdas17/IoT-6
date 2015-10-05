using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Data.Stlth.Model;
using ST.IoT.Services.Stlth.API.StlthSharp;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public class BuildModelStlthCommunityStrategy : IStrategy
    {
        private readonly static string _appToken = "";
        private readonly static string _userToken = "";

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        //client.execute(client.Community.Command.Create("Community 1"));

        public async Task setupCommunity()
        {
            var client = new StlthRestSocialApi(appToken: _appToken, userToken: _userToken);

            var communityOfPeople = client.Community.New("My Very Own Community - for People").Result.Community;
            var communityOfThings = client.Community.New("A community of things").Result.Community;

            var person1 = client.Person.Register("mike@heydt.org", "password").Result.Person;
            var person2 = client.Person.Register("marcia@heydt.org", "password").Result.Person;

            var thing1 = client.Thing.New("Thing-1").Result.Thing;
            var thing2 = client.Thing.New("Thing-2").Result.Thing;
                
            var groupOfThings = client.Group.New("A Group of Things 1").Result.Group;

            var ar1 = client.With(communityOfPeople).Add(person1).Result;
            var ar2 = client.With(communityOfPeople).Add(person2).Result;

            var ar3 = client.With(groupOfThings).Add(thing1).Result;
            var ar4 = client.With(groupOfThings).Add(thing2).Result;

            //client.Relate.Person(person1).As("Friend").To(person2);

            //var person1sFreinds = client.Find.Friends.Of(person1);




            /*

            // we'll add the group to the community, making updates to people in the group on the community wall
            var addedToGroup = client.With(community).Add(group).Result.Success;

            // now we will add the two things to the group
            var addToGroupResult1 = client.With(group).Add(thing1).Result.Success;
            var addToGroupResult2 = client.With(group).Add(thing2).Result.Success;

            // these subscribe to real-time updates in the group and to both "things"
            // the hander here does nothing at this point, just called by SignalR underneath when it happens
            client.With(group).Subscribe(groupUpdate => { });
            client.With(thing1).Subscribe(thingUpdate => { });
            client.With(thing2).Subscribe(thingUpdate => { });

            // these update the two things.  Causes four update calls, one for each thing, 
            // and once for the one group / thing in the group
            var update1 = client.With(thing1).Update(thingData: "{'Prop1': 'value1'}").Within(community).Result;
            var update2 = client.With(thing1).Update(thingData: "{'Prop2': 'value2'}").Within(community).Result;

            // gets the last "x" posts by any members of the community - not real time
            // will return the two updates just made in chromological order

            // actually, since the things are not part of that community, these updates wont' show up there
            var wallContentForCommunity = client.Wall.For(community).Result.Wall;
            */
        }
        /*
        public async Task signInWithCachedCredentials()
        {
            // attempt using cached credentials
            var client = new StlthRestSocialApi();

            // exception if we need to refresh credentials
            client.CheckAuth();

            var status = client.Me.Status.New("I'm adding a post").Result.Status;

            // get my 20 most recent statuses
            var myMostRecentStatuses = client.Me.Status.Recent.Result.Statuses;

            // get 20 most recent statuses in my default community
            var communitiesMostRecentStatuses = client.Community.Status.Result.Statuses;

            // like a status
            client.With(communitiesMostRecentStatuses[0]).Like();
        }

        public async Task workingWithNodeTypes()
        {
            var client = new StlthRestSocialApi();

            var nodes = client.Nodes.Defintiions.All;

            var hobbyNodeDef = client.Nodes.Definitions.New("HOBBY").Named("Making").Result.Node;

        }

        public async Task workingWithNodes()
        {
            var client = new StlthRestSocialApi();

            var nodes = client.Nodes.Defintiions.All;

            var hobbyNodeDef = client.Nodes.Definitions.New("HOBBY").Named("Making").Result.Node;

        }

        public async Task findFriends()
        {
            // attempt using cached credentials
            var client = new StlthRestSocialApi();

            // these are my friends
            var myFriends = client.Me.Friends;

            // find someone with a hobby of being a maker
            // basically, any person with any relationship to a hobby node
            // named is a special clause that can be applied to any type of node to look up its name
            var hasMakerHobby = client.Person.Has("HOBBY").Named("Making").Result.Persons;



        }
        */
        public async Task ExecuteAsync()
        {
            await setupCommunity();
        }
    }
}
