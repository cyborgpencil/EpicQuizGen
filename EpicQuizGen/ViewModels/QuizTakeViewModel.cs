using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Events;
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

        /// <summary>
        /// Question list to match up with loaded Quiz Questions
        /// </summary>
        private List<Question> _currentWorkingQustions;
        public List<Question> CurrentWorkingQuestions
        {
            get { return _currentWorkingQustions; }
            set { SetProperty(ref _currentWorkingQustions, value); }
        }
        private string _currentQuestionType;
        public string CurrentQuestionType
        {
            get { return _currentQuestionType; }
            set { SetProperty(ref _currentQuestionType, value); }
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
            set { value = $"Question {QuestionNavIndex + 1} of {Quiz.Questions.Count.ToString()}:"; SetProperty(ref _questionCounter, value); }
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
        private bool _multiChoiceAnswer1;
        public bool MultiChoiceAnswer1
        {
            get { return _multiChoiceAnswer1; }
            set { SetProperty(ref _multiChoiceAnswer1, value); SetWorkingQuestionsCurrentAnswers(); }
        }
        private bool _multiChoiceAnswer2;
        public bool MultiChoiceAnswer2
        {
            get { return _multiChoiceAnswer2; }
            set { SetProperty(ref _multiChoiceAnswer2, value); SetWorkingQuestionsCurrentAnswers(); }
        }
        private bool _multiChoiceAnswer3;
        public bool MultiChoiceAnswer3
        {
            get { return _multiChoiceAnswer3; }
            set { SetProperty(ref _multiChoiceAnswer3, value); SetWorkingQuestionsCurrentAnswers(); }
        }
        private bool _multiChoiceAnswer4;
        public bool MultiChoiceAnswer4
        {
            get { return _multiChoiceAnswer4; }
            set { SetProperty(ref _multiChoiceAnswer4, value);}
        }
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); SetWorkingQuestionsCurrentAnswers(); }
        }
        private bool _falseAnswer;
        public bool FalseAnswer
        {
            get { return _falseAnswer; }
            set { SetProperty(ref _falseAnswer, value); SetWorkingQuestionsCurrentAnswers(); }
        }
        //private List<TrueFalseQuizTake> _currentWorkingToF;
        //public List<TrueFalseQuizTake> CurrentWorkingToF
        //{
        //    get { return _currentWorkingToF; }
        //    set { SetProperty(ref _currentWorkingToF, value); }
        //}

        private List<BindableBase> _questionViews;
        public List<BindableBase> QuestionViews
        {
            get { return _questionViews; }
            set { SetProperty(ref _questionViews, value); }
        }
        private MainWindowViewModel _mainVM { get; set; }
        private IRegionManager _regionManager { get; set; }
        public NavigationParameters NavPar { get; set; }
        public EventAggregator _eventAggregator { get; set; }
        #endregion

        #region Constructor
        public QuizTakeViewModel(IRegionManager regionManager, MainWindowViewModel mainVM, EventAggregator eventAggregator)
        {
            _mainVM = mainVM;
            _eventAggregator = eventAggregator;
            QuestionNavIndex = 0;
            _regionManager = regionManager;
            Quiz = QuizIOManager.Instance.Quiz;
            Timer = int.Parse(Quiz.QuizTime);

            CurrentQuestion = Quiz.Questions[QuestionNavIndex];
            CurrentQuestionType = CurrentQuestion.QuestionType;
            TimeLeftString = "";
            QuestionCounter = "";

            // Question Views
            QuestionViews = new List<BindableBase>();

            QuizTakeLoadCommand = new DelegateCommand(QuizTakeLoad);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextQuestionCommand = new DelegateCommand(NextQuestion);
            PreviousQuestionCommand = new DelegateCommand(PreviousQuestion);

            SetUpWorkingQuestions();
            
            ClearQustionAnswers();
        }
        #endregion

        #region Commands
        public DelegateCommand QuizTakeLoadCommand { get; set; }
        public void QuizTakeLoad()
        {
            //_eventAggregator.GetEvent<SendQuizTakeTrueAnswer>().Subscribe(SetTrueAnswer);
            //_eventAggregator.GetEvent<SendQuizTakeFalseAnswer>().Subscribe(SetFalseAnswer);

            // Set Main Quiz
            if (Quiz == null)
            {
                // Set main quiz based on Passed in Quiz
                Quiz = QuizIOManager.Instance.Quiz;
            }
            // Set Quiz Questions that will match users answers
            CurrentWorkingQuestions = new List<Question>();
            CurrentWorkingQuestions = Quiz.Questions;

            // Clear out Working Questions
            ClearQustionAnswers();

            // Set current Question
            QuestionNavIndex = 0;


            // Set QuizView
            //foreach (var q in Quiz.Questions)
            //{
            //    SetQuestionViews(q);
            //}

            // Set Up Timer For Quiz
            Timer = int.Parse(Quiz.QuizTime);
            QuizTimer = new QuizTimeManager(this);
            QuizTimer.StartTimer();

            // Set Initial Question

            // if (CurrentWorkingToF == null)
            //    CurrentWorkingToF = new List<TrueFalseQuizTake>();
            //SetToFWorkingQuestions();

            // Parameters to send to current view
            CheckNavParameters();

           // _eventAggregator.GetEvent<SendQuizTakeTrueAnswer>().Subscribe(SetTrueAnswer);
           // _eventAggregator.GetEvent<SendQuizTakeFalseAnswer>().Subscribe(SetFalseAnswer);

            // Navigate to Question #1
            Navigate(CheckQuestionAnswerType());
        }

        public DelegateCommand<string> NavigateCommand { get; set; }
        public void Navigate(string uri)
        {
            _regionManager.RequestNavigate("CurrentQuestionType", uri, NavPar);
        }

        public DelegateCommand NextQuestionCommand { get; set; }
        public void NextQuestion()
        {
            CheckNavParameters();
            _eventAggregator.GetEvent<SendQuizTakeTrueAnswer>().Subscribe(SetTrueAnswer);
            _eventAggregator.GetEvent<SendQuizTakeFalseAnswer>().Subscribe(SetFalseAnswer);
            // set Prev previous View Question 
            SetPreviousViewQuestion();
            QuestionNavIndex += 1;
            if (QuestionNavIndex > Quiz.Questions.Count - 1)
                QuestionNavIndex = Quiz.Questions.Count - 1;
            
            SetQuestionAnswerView();
            
        }
        public DelegateCommand PreviousQuestionCommand { get; set; }
        public void PreviousQuestion()
        {
            CheckNavParameters();
            _eventAggregator.GetEvent<SendQuizTakeTrueAnswer>().Subscribe(SetTrueAnswer);
            _eventAggregator.GetEvent<SendQuizTakeFalseAnswer>().Subscribe(SetFalseAnswer);
            SetPreviousViewQuestion();
            QuestionNavIndex -= 1;

            SetQuestionAnswerView();
            
            SetAnswerProgress(-1);
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
            switch (CurrentQuestion.QuestionType)
            {
                case "MULTICHOICE4":
                    return "MultiChoice4QuizView";
                default:
                    return "TrueFalseQuizView";
            }
        }

        private void SetQuestionAnswerView()
        {
            if (QuestionNavIndex < Quiz.Questions.Count && QuestionNavIndex >= 0)
            {
                QuestionCounter = "";
                CurrentQuestion = Quiz.Questions[QuestionNavIndex];
                CurrentQuestionType = CurrentQuestion.QuestionType;
                NavPar = new NavigationParameters();
                CheckNavParameters();

                Navigate(CheckQuestionAnswerType());
            }
        }

        private void ClearQustionAnswers()
        {
            switch (CurrentWorkingQuestions[QuestionNavIndex].QuestionType)
            {
                case "MULTICHOICE4":
                    ClearMultiChoice4Answers();
                    break;
                default: // QuetionType = TRUEFALSE
                    CurrentWorkingQuestions[QuestionNavIndex].TrueAnswer = false;
                    CurrentWorkingQuestions[QuestionNavIndex].FalseAnswer = false;
                    break;
            }
        }
        private void ClearMultiChoice4Answers()
        {
            for (int i = 0; i < CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerPositions.Count; i++)
            {
                CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerPositions[i] = new bool();
            }
        }

        //private void SetToFWorkingQuestions()
        //{
        //    for (int i = 0; i < Quiz.Questions.Count; i++)
        //    {
        //        if (Quiz.Questions[i].QuestionType == QuestionTypes.TRUEFALSE.ToString())
        //        {
        //            CurrentWorkingToF.Add(new TrueFalseQuizTake());

        //        }
        //    }
            
        //}

        private void SetPreviousViewQuestion()
        {
            TrueAnswer = CurrentWorkingQuestions[QuestionNavIndex].TrueAnswer;
            FalseAnswer = CurrentWorkingQuestions[QuestionNavIndex].FalseAnswer;
        }

        private void SetQuestionViews(Question question)
        {
            //if(question.QuestionType == QuestionTypes.TRUEFALSE.ToString())
            //{
            //    TrueFalseQuizViewModel view = new TrueFalseQuizViewModel(new Prism.Events.EventAggregator());
            //    view.TrueAnswer = question.TrueAnswer;
            //    view.FalseAnswer = question.FalseAnswer;
            //    QuestionViews.Add(view);
            //}
            if (question.QuestionType == QuestionTypes.MULTICHOICE4.ToString())
            {
                MultiChoice4QuizViewModel view = new MultiChoice4QuizViewModel();
                view.MultiChoiceAnswerQuestion1 = question.MultiAnswerList[0];
                view.MultiChoiceAnswerQuestion2 = question.MultiAnswerList[1];
                view.MultiChoiceAnswerQuestion3 = question.MultiAnswerList[2];
                view.MultiChoiceAnswerQuestion4 = question.MultiAnswerList[3];
                QuestionViews.Add(new MultiChoice4QuizViewModel());
            }
        }
        
        //Sets the Parameters to be sent with Current Question User is working on (QuestionNavIndex) based on Question Type
        private void CheckNavParameters()
        {
            NavPar = new NavigationParameters();
            if (CurrentWorkingQuestions[QuestionNavIndex].QuestionType == QuestionTypes.TRUEFALSE.ToString())
            {
                NavPar.Add("TrueAnswer", CurrentWorkingQuestions[QuestionNavIndex].TrueAnswer);
                NavPar.Add("FalseAnswer", CurrentWorkingQuestions[QuestionNavIndex].FalseAnswer);
            }
            if (CurrentWorkingQuestions[QuestionNavIndex].QuestionType == QuestionTypes.MULTICHOICE4.ToString())
            {
                NavPar.Add("MultiList1", CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerList[0]);
                NavPar.Add("MultiList2", CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerList[1]);
                NavPar.Add("MultiList3", CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerList[2]);
                NavPar.Add("MultiList4", CurrentWorkingQuestions[QuestionNavIndex].MultiAnswerList[3]);
            }
        }

        private void SetWorkingQuestionsCurrentAnswers()
        {

        }

        private void SetAnswerProgress(int navDir)
        {
            
            #endregion
        }

        private void SetTrueAnswer(bool obj)
        {
            CurrentWorkingQuestions[QuestionNavIndex].TrueAnswer = obj;
        }
        private void SetFalseAnswer(bool obj)
        {
            CurrentWorkingQuestions[QuestionNavIndex].FalseAnswer = obj;
        }

        private void SetUpWorkingQuestions()
        {
            CurrentWorkingQuestions = new List<Question>(Quiz.Questions);

            // Clear True False questions
            ClearQustionAnswers();
        }
    }
}
