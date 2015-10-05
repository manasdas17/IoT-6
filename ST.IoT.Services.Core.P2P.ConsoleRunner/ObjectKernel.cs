using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace ST.IoT.Services.Core.P2P.ConsoleRunner
{
    public class ObjectKernel
    {
        private static IKernel _kernel;

        public static ObjectKernel Instance
        {
            get; private set;
        }

        public static IKernel Kernel => _kernel;

        public ISupernodeService SupernodeService
        {
            get { return _kernel.Get<ISupernodeService>(); }
        }

        public ObjectKernel(IKernel kernel)
        {
            _kernel = kernel;
            Instance = this;
        }
    }
}
