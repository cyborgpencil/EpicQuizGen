using EpicQuizGen.Events;
using EpicQuizGen.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Collections.Generic;

namespace EpicQuizGen.ViewModels
{
    class TrueFalseQuizViewModel : QuizViewModelbase
    {
        #region Properties
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); SetAnswer(); }
        }
        private bool _falseAnswer;
        public bool FalseAnswer
        {
            get { return _falseAnswer; }
            set {SetProperty(ref _falseAnswer, value); SetAnswer();
            }
        }
        private bool _answered;
        public bool Answered
        {
            get { return _answered; }
            set { SetProperty(ref _answered, value); }
        }

        private EventAggregator _eventAggregator;
        #endregion

        #region Contructors
        public TrueFalseQuizViewModel()
        {
        }
        public TrueFalseQuizViewModel(EventAggregator eventaggregator) :this()
        {
            _eventAggregator = eventaggregator;
            TFQuizViewLoadCommand = new DelegateCommand(TFQuizViewLoad);
        }
        #endregion

        #region Commands
        public DelegateCommand TFQuizViewLoadCommand { get; set; }
        public void TFQuizViewLoad()
        {

        }
        #endregion

        #region Events
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TrueAnswer = (bool)navigationContext.Parameters["TrueAnswer"];
            FalseAnswer = (bool)navigationContext.Parameters["FalseAnswer"];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<SendQuizTakeTrueAnswer>().Publish(TrueAnswer);
            _eventAggregator.GetEvent<SendQuizTakeFalseAnswer>().Publish(FalseAnswer);
        }
        #endregion

        #region Methods
        private void SetAnswer()
        {
            if (TrueAnswer == false && FalseAnswer == false)
            {
                Answered = false;
            }
            else
                Answered = true;
        }

        #endregion

    }
}
