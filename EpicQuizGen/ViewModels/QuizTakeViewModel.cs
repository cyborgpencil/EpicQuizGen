using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class QuizTakeViewModel : BindableBase
    {
        #region Properties
        private string _timeLeftString;
        public string TimeLeftString
        {
            get { return _timeLeftString; }
            set { value = $"Time Left: {Timer}"; SetProperty(ref _timeLeftString, value); }
        }
        private int _timer;
        public int Timer
        {
            get { return _timer; }
            set { SetProperty(ref _timer, value); }
        }
        private Quiz _quiz;
        public Quiz Quiz
        {
            get { return _quiz; }
            set { SetProperty(ref _quiz, value); }
        }
        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { SetProperty(ref _currentQuestion, value); }
        }
        private string _currentQuestionType;
        public string CurrentQuestionType
        {
            get { return _currentQuestionType; }
            set {SetProperty(ref _currentQuestionType, value); }
        }

        private QuizTimeManager _quizTimer;
        public QuizTimeManager QuizTimer
        {
            get { return _quizTimer; }
            set { SetProperty(ref _quizTimer, value); }
        }
        private string _questionCounter;
        public string QuestionCounter
        {
            get { return _questionCounter; }
            set { value = $"Question {QuestionNavIndex+1} of {Quiz.Questions.Count.ToString()}:"; SetProperty(ref _questionCounter, value); }
        }
        private int _questionNavIndex;
        public int QuestionNavIndex
        {
            get { return _questionNavIndex; }
            set
            {
                if (value < 0)
                    value = 0;
                SetProperty(ref _questionNavIndex, value);
            }
        }
        private MainWindowViewModel _mainVM { get; set; }
        private IRegionManager _regionManager { get; set; }
        #endregion

        #region Constructor
        public QuizTakeViewModel(IRegionManager regionManager, MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            QuestionNavIndex = 0;
            _regionManager = regionManager;
            Quiz = QuizIOManager.Instance.Quiz;
            Timer = int.Parse(Quiz.QuizTime);

            CurrentQuestion = Quiz.Questions[QuestionNavIndex];
            CurrentQuestionType = CurrentQuestion.QuestionType;
            TimeLeftString = "";
            QuestionCounter = "";
            QuizTakeLoadCommand = new DelegateCommand(QuizTakeLoad);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextQuestionCommand = new DelegateCommand(NextQuestion);
            PreviousQuestionCommand = new DelegateCommand(PreviousQuestion);
        }
        #endregion

        #region Commands
        public DelegateCommand QuizTakeLoadCommand { get; set; }
        public void QuizTakeLoad()
        {
            // Timer
            Timer = int.Parse(Quiz.QuizTime);
            QuizTimer = new QuizTimeManager(this);
            QuizTimer.StartTimer();

            Navigate(CheckQuestionAnswerType());
        }

       

        public DelegateCommand<string> NavigateCommand { get; set; }
        public void Navigate(string uri)
        {
            _regionManager.RequestNavigate("CurrentQuestionType", uri);
        }

        public DelegateCommand NextQuestionCommand { get; set; }
        public void NextQuestion()
        {
            QuestionNavIndex += 1;
            if (QuestionNavIndex > Quiz.Questions.Count - 1)
                QuestionNavIndex = Quiz.Questions.Count - 1;

            SetQuestionAnserView();
        }
        public DelegateCommand PreviousQuestionCommand { get; set; }
        public void PreviousQuestion()
        {
            QuestionNavIndex -= 1;
            SetQuestionAnserView(); ;
        }
        public DelegateCommand FinishQuizCommand { get; set; }
        #endregion

        #region Events
        internal void QuizTimerEvent(object sender, EventArgs e)
        {
            //Timer = QuizTimer.SendCurrentSecound();
            //TimeLeftString = "";
            //if (Timer == 0)
            //{
            //    _regionManager.RequestNavigate("ContentRegion", "QuizzesShowView");
            //}
        }
        #endregion

        #region Methods
        private string CheckQuestionAnswerType()
        {
            switch(CurrentQuestion.QuestionType)
            {
                case "MULTICHOICE4":
                    return "MultiChoice4QuizView";
                default:
                    return "TrueFalseQuizView";
            }
        }

        private void SetQuestionAnserView()
        {
            if (QuestionNavIndex < Quiz.Questions.Count && QuestionNavIndex >= 0)
            {
                QuestionCounter = "";
                CurrentQuestion = Quiz.Questions[QuestionNavIndex];
                CurrentQuestionType = CurrentQuestion.QuestionType;
                Navigate(CheckQuestionAnswerType());
            }
        }
        #endregion
    }
}
