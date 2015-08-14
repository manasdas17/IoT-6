using System;
using System.Collections.Generic;
using System.Text;

namespace ST.IoT.Clients.Win.MyThings
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
