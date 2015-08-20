using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using PostSharp.Patterns.Model;

namespace ST.IoT.Clients.Stlth.ViewModels
{
    [NotifyPropertyChanged]
    public class MainStlthViewModel 
    {
        public string Message { get; set; }

        public SubViewModel Content { get; set; }

        public MainStlthViewModel()
        {
            Message = "HI";
            Content = new SubViewModel();
        }
    }
}
