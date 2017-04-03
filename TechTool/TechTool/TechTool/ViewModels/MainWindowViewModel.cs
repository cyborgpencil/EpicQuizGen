using Prism.Mvvm;
using System.Windows;
using Prism.Commands;
using Prism.Regions;

namespace TechTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    { 
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        private IRegionManager _regionManager;

        public MainWindowViewModel()
        {
            ExitCommand = new DelegateCommand(Exit);
        }
           
        public MainWindowViewModel(IRegionManager regionManager) :this()
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public void Navigate(string uri)
        {
            _regionManager.RequestNavigate("MainContentRegion", uri);
        }

        
    }
}
