using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace EpicQuizGen.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties

        private string _question;
        public string Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value); }
        }

        #endregion

        #region RegionManager
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }
        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }
        #endregion

        #region Commands

        #endregion

    }
}
 