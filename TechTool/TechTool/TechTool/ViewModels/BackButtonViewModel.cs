using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTool.ViewModels
{
    class BackButtonViewModel : BindableBase
    {
        private DelegateCommand _backCommand;
        public DelegateCommand BackCommand
        {
            get { return _backCommand; }
            set { SetProperty(ref _backCommand, value); }
        }
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        public BackButtonViewModel()
        {
            BackCommand = new DelegateCommand(Back);
           
        }

        public BackButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        private void Back()
        {
            
        }
    }
}
