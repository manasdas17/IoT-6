using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        internal object GetNodeTypes()
        {
            throw new NotImplementedException();
        }

        internal object GetNodeMetadata(string forNodeName)
        {
            throw new NotImplementedException();
        }

        internal object SetNodeMetadata(string forNodeType, string metadata)
        {
            throw new NotImplementedException();
        }

        public class QueryBuilder
        {
            public class NodeQuerySpec
            {
                public RelationQuerySpec Relationship(string relationshipName)
                {
                    return new RelationQuerySpec();   
                }

                public NodeQuerySpec()
                {
                    
                }

                public NodeQuerySpec(dynamic aSpecificNode)
                {
                    
                }
            }

            public class RelationshipPropertyQuerySpec
            {
                public RelationshipPropertyQuerySpec Is => new RelationshipPropertyQuerySpec();

                public RelationshipPropertyQuerySpec Constant(string value)
                {
                    return this;
                }
            }

            public class RelationQuerySpec
            {
                public RelationQuerySpec()
                {
                    
                }

                public RelationshipPropertyQuerySpec Property(string propertyName)
                {
                    return new RelationshipPropertyQuerySpec();
                }


            }

            public NodeQuerySpec From(string nodeOfTypeName)
            {
                return new NodeQuerySpec();
            }

            public NodeQuerySpec From(dynamic aSpecificNode)
            {
                return new NodeQuerySpec(aSpecificNode);   
            }

            public NodeQuerySpec Node(dynamic node)
            {
                return null;
            }

            public NodeQuerySpec Any()
            {
                return new NodeQuerySpec();
            }
        }

        public class CommunityBuilder
        {
            public CommunityBuilder Create(string name)
            {
                return this;
            }
        }
        /*
        public class InviteSpecPersistence
        {
            public 
        }
        */
        public class InviteSpecPersistence
        {
            public void Persistent()
            {
                
            }
        }
        

        public class ToInviteSpec
        {
            public InviteSpecPersistence To(CommunityBuilder community)
            {
                return new InviteSpecPersistence();
            }
        }

        public class InviteSpec
        {
            internal ToInviteSpec People(QueryBuilder.RelationshipPropertyQuerySpec who)
            {
                return new ToInviteSpec();
            }
        }

        public CommunityBuilder Community => new CommunityBuilder();
        public QueryBuilder Query => new QueryBuilder();
        public InviteSpec Invite => new InviteSpec();
    }

    public static class MetaDataScenarios
    {
        public static void listNodeTypes()
        {
            var stlth = new StlthService(token: "your_api_token");
            var nodeTypeNames = stlth.GetNodeTypes();
        }
        public static void getNodeMetadata()
        {
            var stlth = new StlthService(token: "your_api_token");
            var nodeTypeNames = stlth.GetNodeMetadata(forNodeName: "Person");
        }

        public static void setNodeMetaData()
        {
            var stlth = new StlthService(token: "your_api_token");
            var meta = @"{ 
                            Properties: { Name: { IsRequired: True, IsUnique: False} } },
                            AcceptsRelationsFrom: [ ""Person"" ],
                            RequiresApprovalForRelationshipsFrom: [ ""Person"" ]
                         }";
            stlth.SetNodeMetadata(forNodeType: "Person", metadata: meta);
        }
    }

    public abstract class QueryClause
    {
    }

    public class NodeRelationshipQueryClause
    {
        public NodeRelationshipQueryClause()
        {
            
        }
    }

    public static class SocialScenarios
    {
        public async static Task foo()
        {
            /*
            var stlth = new StlthService(token: "your_api_token");

            // creata a new community for people to share their experiences
            // around being a maker
            var community = stlth.Community.Create("Makers");

            var invitees = stlth
                .Social
                .Query
                .From(nodeOfTypeName: "Person")
                .Relationship("HAS_HOBBY").Property("Name").Is.Constant("Being a Maker");

            // this query invites everyone that now has a hobby of being a maker
            // and anyone that takes up this hobby in the future
            stlth.Social
                 .Invite
                 .People(invitees)
                 .To(community)
                 .Persistent();

            var community_locations = stlth.Community.Locations(community);

            // we can map people to locations
            var member_to_locations  = stlth
                .Spatial
                .Map
                .People(who)
                .Reside
                .Within.
                .Miles(10)
                .Of(community_locations);
                */
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
        public static void fluentFindAllMyFriends()
        {
            var stlth = new StlthService(token: "your_api_token");
            var query = stlth.Query
                .From(aSpecificNode: stlth.Me)
                .Relationship(relationshipName: "Friend")
                .ToAnyNodes();
            var allMyFriends = query.Execute();
        }

        public static void findAllRelationsBetweenMeAndAnotherPerson()
        {
            var stlth = new StlthService(token: "your_api_token");
            var query = stlth.Query
                .From(aSpecificNode: stlth.Me)
                .AnyRelationship()
                .To(aNodeType: "Person")
                .Where(properties: "{Name: 'Will'}");
            var relationshipBetweenMikeAndWill = query.Execute();
        }

        public static void foo()
        {
            /*
            var stlth = new StlthService(token: "your_api_token");
            var query = stlth.Query
                .ShortestPathFrom(stlth.Me).To(stlth_experts your => stlth_experts.Relationship("USES").To(aNodeType: "BaaS").Where(properties: "{Name: 'Slth.IO}"))
                .ViaRelationship("FRIEND")
                .MaxDegreesOfSeparation(5);
            */
        }
    }
}
