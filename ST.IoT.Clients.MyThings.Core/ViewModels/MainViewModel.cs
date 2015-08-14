using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;

namespace ST.IoT.Clients.MyThings.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private ObservableCollection<string> _appFunctions = new ObservableCollection<string>()
        {
            "Minions",
            "Stlth"
        };

        public ObservableCollection<string> AppFunctions
        {
            get { return _appFunctions; }
            set
            {
                _appFunctions = new ObservableCollection<string>();
                this.RaisePropertyChanged(() => AppFunctions);
            }
        }

        private ObservableCollection<PanelSectionViewModel> _panelSections = new ObservableCollection
            <PanelSectionViewModel>()
        {
            new PanelSectionViewModel() { Title = "Your things", DesiredWidth = 350},
            new PanelSectionViewModel() { Title = "The Minions", DesiredWidth = 350},
            new PanelSectionViewModel() { Title = "Timeline", DesiredWidth = 350},
            new PanelSectionViewModel() { Title = "Communities", DesiredWidth = 350},
            new PanelSectionViewModel() { Title = "Friends", DesiredWidth = 350},
            new PanelSectionViewModel() { Title = "Other", DesiredWidth = 350},
        };
        public ObservableCollection<PanelSectionViewModel> PanelSections
        {
            get { return _panelSections; }
            set
            {
                _panelSections = value;
                this.RaisePropertyChanged(() => PanelSections);
            }
        }
    }
}
