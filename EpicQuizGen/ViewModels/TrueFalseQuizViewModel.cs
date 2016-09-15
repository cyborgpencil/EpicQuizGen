using EpicQuizGen.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    class TrueFalseQuizViewModel : BindableBase, INavigationAware
    {
        #region Properties
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set {SetProperty(ref _trueAnswer, value);}
        }
        private bool _falseAnswer;
        public bool FalseAnswer
        {
            get { return _falseAnswer; }
            set { SetProperty(ref _falseAnswer, value);
            }
        }
        private EventAggregator _eventAggregator;
        #endregion

        #region Contructors
        public TrueFalseQuizViewModel(EventAggregator eventaggregator)
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
        private void SendCurrentAnswer()
        {
            
            
        }

        
        #endregion

    }
}
