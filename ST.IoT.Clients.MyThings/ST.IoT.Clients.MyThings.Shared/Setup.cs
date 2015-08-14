﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;

namespace ST.IoT.Clients.MyThings
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
