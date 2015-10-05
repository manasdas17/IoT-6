using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public abstract class NodeFactory
    {
        private static readonly Dictionary<string, Func<StlthNodeInternals, Node>> _factoryMethods =
            new Dictionary<string, Func<StlthNodeInternals, Node>>()
        {
            {"Person", buildPerson},
            {"Post", buildPost },
        };

        public static Node create(StlthNodeInternals internals)
        {
            if (internals == null) return null;
            var label = internals.Node["data"]["ClassName"].ToString();
            if (_factoryMethods.ContainsKey(label)) return _factoryMethods[label](internals);
            return new Node(internals);
        }

        private static Person buildPerson(StlthNodeInternals internals)
        {
            return new Person(internals);
        }

        private static Post buildPost(StlthNodeInternals internals)
        {
            return new Post(internals);
        }

        public static IEnumerable<Node> create(StlthNodeInternals[] rawnode)
        {
            var results = rawnode.Select(rn => create(rn)).ToArray();
            return results;
        }
    }
}
