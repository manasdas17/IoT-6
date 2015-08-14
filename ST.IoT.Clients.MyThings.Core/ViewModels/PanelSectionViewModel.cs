using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;

namespace ST.IoT.Clients.MyThings.Core.ViewModels
{
    public class PanelSectionViewModel : MvxViewModel
    {
        public string Title { get; set; }
        public double DesiredWidth { get; set; }
    }
}
