using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class MultiChoice4QuizViewModel : QuizViewModelbase
    {
        #region Properties
        private bool _multiChoiceAnswer1;
        public bool MultiChoiceAnswer1
        {
            get { return _multiChoiceAnswer1; }
            set { SetProperty(ref _multiChoiceAnswer1, value); }
        }
        private bool _multiChoiceAnswer2;
        public bool MultiChoiceAnswer2
        {
            get { return _multiChoiceAnswer2; }
            set { SetProperty(ref _multiChoiceAnswer2, value); }
        }
        private bool _multiChoiceAnswer3;
        public bool MultiChoiceAnswer3
        {
            get { return _multiChoiceAnswer3; }
            set { SetProperty(ref _multiChoiceAnswer3, value); }
        }
        private bool _multiChoiceAnswer4;
        public bool MultiChoiceAnswer4
        {
            get { return _multiChoiceAnswer4; }
            set { SetProperty(ref _multiChoiceAnswer4, value); }
        }
        private string _multiChoiceAnswerQuestion1;
        public string MultiChoiceAnswerQuestion1
        {
            get { return _multiChoiceAnswerQuestion1; }
            set { SetProperty(ref _multiChoiceAnswerQuestion1, value); }
        }
        private string _multiChoiceAnswerQuestion2;
        public string MultiChoiceAnswerQuestion2
        {
            get { return _multiChoiceAnswerQuestion2; }
            set { SetProperty(ref _multiChoiceAnswerQuestion2, value); }
        }
        private string _multiChoiceAnswerQuestion3;
        public string MultiChoiceAnswerQuestion3
        {
            get { return _multiChoiceAnswerQuestion3; }
            set { SetProperty(ref _multiChoiceAnswerQuestion3, value); }
        }
        private string _multiChoiceAnswerQuestion4;
        public string MultiChoiceAnswerQuestion4
        {
            get { return _multiChoiceAnswerQuestion4; }
            set { SetProperty(ref _multiChoiceAnswerQuestion4, value); }
        }
        #endregion

        #region Contructors
        public MultiChoice4QuizViewModel()
        {
            LoadMulti4QuizViewCommand = new DelegateCommand(LoadMulti4QuizView);
        }
        #endregion

        #region Commands
        public DelegateCommand LoadMulti4QuizViewCommand { get; set; }
        public void LoadMulti4QuizView()
        {

        }


        #endregion

        #region Events
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            MultiChoiceAnswerQuestion1 = navigationContext.Parameters["MultiList1"].ToString();
            MultiChoiceAnswerQuestion2 = navigationContext.Parameters["MultiList2"].ToString();
            MultiChoiceAnswerQuestion3 = navigationContext.Parameters["MultiList3"].ToString();
            MultiChoiceAnswerQuestion4 = navigationContext.Parameters["MultiList4"].ToString();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion

        #region Methods
        #endregion

    }
}
