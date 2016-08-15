using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace EpicQuizGen.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties

        #endregion

        #region RegionManager
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand LoadQuizzesCommand { get; set; }
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            LoadQuizzesCommand = new DelegateCommand(LoadQuizzes);
        }

        #endregion

        #region Commands

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        private void LoadQuizzes()
        {
            _regionManager.RequestNavigate("ContentRegion", "QuizzesShowView");
        }

        #endregion

    }
}
 