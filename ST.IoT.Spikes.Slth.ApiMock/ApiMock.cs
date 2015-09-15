using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;

namespace ST.IoT.Spikes.Slth.ApiMock
{
    public class Person : DynamicObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class StlthService
    {
        public StlthService(string token)
        {
            
        }

        public dynamic CreateNodeInstance(string nodeType)
        {
            return null;
        }

        async public Task ExecuteAsync()
        {
            
        }

        public dynamic GetNodeByID(string nodeID)
        {
            return null;
        }

        public dynamic QueryNode(string properties)
        {
            return null;
        }

        public dynamic Me { get; set; }

        public dynamic Relate(dynamic fromThisNode, dynamic toThisNode, string relationshipType)
        {
            return null;
        }

        public IEnumerable<dynamic> GetRelationshipRequests(dynamic forNode, string relationshipType)
        {
            return null;
        }

        public void ApproveRequests(IEnumerable<dynamic> requests)
        {
            
        }

        public IEnumerable<dynamic> RelationshipQuery(
            dynamic fromNode,
            string relationshipName,
            string secondRelationshipName = null,
            double within = 0.0,
            int depth = 1,
            string properties = null)
        {
            return null;
        }
    }

    public static class Scenarios
    {
        async public static void addAddNewPerson()
        {
            var stlth = new StlthService(token: "your_api_token");
            var person = stlth.CreateNodeInstance(nodeType: "Person");
            person.Name = "Mike";
            await stlth.ExecuteAsync();
            var id = person.ID;  // assigned during the execute process
        }

        public static void getNodeInstance()
        {
            var stlth = new StlthService(token: "your_api_token");
            var node = stlth.GetNodeByID(nodeID: "1");
            var name = node.Name;
        }

        public static void queryByProperties()
        {
            var stlth = new StlthService(token: "your_api_token");
            var person = stlth.QueryNode(properties: "{'Name': 'Mike'}");
        }

        async public static void makeFriends()
        {
            var stlth = new StlthService(token: "your_api_token");
            var me = stlth.Me;
            var toBeFriend = stlth.QueryNode(properties: "{'Name': 'Will'");
            stlth.Relate(fromThisNode: me, toThisNode: toBeFriend, relationshipType: "Friend");
        }

        public static void approveAllThatWantToBeMyFriend()
        {
            var stlth = new StlthService(token: "your_api_token");
            var requests = stlth.GetRelationshipRequests(forNode: stlth.Me, relationshipType: "Friend");
            stlth.ApproveRequests(requests);
        }

        public static void getMyFriends()
        {
            var stlth = new StlthService(token: "your_api_token");
            var friends = stlth.RelationshipQuery(
                fromNode: stlth.Me,
                relationshipName: "Friend");
        }

        public static void getOnlyMyFriendsNames()
        {
            var stlth = new StlthService(token: "your_api_token");
            var friends = stlth.RelationshipQuery(
                fromNode: stlth.Me,
                relationshipName: "Friend",
                properties: "Name");
        }

        public static void getMostRecentFriendStatusUpdatesWithinTimeframe()
        {
            var stlth = new StlthService(token: "your_api_token");
            stlth.RelationshipQuery(
                fromNode: stlth.Me, 
                relationshipName: "Friend", 
                secondRelationshipName: "Status", 
                within: 10.0,
                depth: 1);
        }
    }
}
