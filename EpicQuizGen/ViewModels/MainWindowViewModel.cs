using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;

namespace EpicQuizGen.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Properties

        #endregion

        #region RegionManager
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand LoadQuizzesCommand { get; set; }
        private IEventAggregator _evenAggregator;
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _evenAggregator = eventAggregator;
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            LoadQuizzesCommand = new DelegateCommand(LoadQuizzes);
        }

        #endregion

        #region Commands

        public void Navigate(string uri)
        {
            _evenAggregator.GetEvent<TakeQuizEvent>().Subscribe(TakeQuiz);
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        private void LoadQuizzes()
        {
            _regionManager.RequestNavigate("ContentRegion", "QuizzesShowView");
        }
        #endregion
        public void TakeQuiz(Quiz obj)
        {
            QuizIOManager.Instance.Quiz = obj;
            Navigate("QuizTakeView");
        }
    }
}
 