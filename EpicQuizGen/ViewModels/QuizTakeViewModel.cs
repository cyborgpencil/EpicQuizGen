using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string _currentMainQuestion;
        public string CurrentMainQuestion
        {
            get { return _currentMainQuestion; }
            set { SetProperty(ref _currentMainQuestion, value); }
        } 

        private QuizTimeManager _quizTimer;
        public QuizTimeManager QuizTimer
        {
            get { return _quizTimer; }
            set { SetProperty(ref _quizTimer, value); }
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
        public NavigationParameters NavPar { get; set; }
        public EventAggregator _eventAggregator { get; set; }

        // List of working Questions to be edited
        private ObservableCollection<QuizViewModelbase> _questionLists;
        public ObservableCollection<QuizViewModelbase> QuestionLists
        {
            get { return _questionLists; }
            set { SetProperty(ref _questionLists, value); }
        }
        private QuizViewModelbase _currentWorkingQuestion;
        public QuizViewModelbase CurrentWorkingQuestion
        {
            get { return _currentWorkingQuestion; }
            set { SetProperty(ref _currentWorkingQuestion, value); }
        }

        private int _currentQuestionIndex;
        public int CurrentQuestionIndex
        {
            get { return _currentQuestionIndex; }
            set
            {
                if (value < 0)
                {
                    _currentQuestionIndex = 0;
                }
                else if(value > MAX_QUESTIONS - 1)
                {
                    _currentQuestionIndex = MAX_QUESTIONS - 1;
                }
                else
                    _currentQuestionIndex = value;
            }
        }
        private readonly int MAX_QUESTIONS;
        #endregion

        #region Constructor
        public QuizTakeViewModel()
        {
            if (Quiz == null)
            {
                // Set main quiz based on Passed in Quiz
                Quiz = QuizIOManager.Instance.Quiz;
                MAX_QUESTIONS = Quiz.Questions.Count;
            }
            CurrentQuestionIndex = 0;
            CurrentWorkingQuestion = new QuizViewModelbase();
            QuestionLists = new ObservableCollection<QuizViewModelbase>();
            BuildWorkingQuestions();
            CurrentWorkingQuestion = QuestionLists[CurrentQuestionIndex];
            CurrentMainQuestion = Quiz.Questions[QuestionNavIndex].MainQuestion;
            Timer = int.Parse(Quiz.QuizTime);

        }
        public QuizTakeViewModel(IRegionManager regionManager, MainWindowViewModel mainVM, EventAggregator eventAggregator) : this()
        {
            _mainVM = mainVM;
            _eventAggregator = eventAggregator;
            QuestionNavIndex = 0;
            _regionManager = regionManager;
            
            TimeLeftString = "";
           
            QuizTakeLoadCommand = new DelegateCommand(QuizTakeLoad);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextQuestionCommand = new DelegateCommand(NextQuestion);
            PreviousQuestionCommand = new DelegateCommand(PreviousQuestion);
            FinishQuizCommand = new DelegateCommand(FinishQuiz);
        }
        #endregion

        #region Commands
        public DelegateCommand QuizTakeLoadCommand { get; set; }
        public void QuizTakeLoad()
        {
            // Set Main Quiz
            if (Quiz == null)
            {
                // Set main quiz based on Passed in Quiz
                Quiz = QuizIOManager.Instance.Quiz;
            }

            BuildWorkingQuestions();

            // Set current Question
            QuestionNavIndex = 0;

            // Set Up Timer For Quiz
            Timer = int.Parse(Quiz.QuizTime);
            QuizTimer = new QuizTimeManager(this);
            QuizTimer.StartTimer();

           // Navigate(CheckQuestionAnswerType());
        }

        public DelegateCommand<string> NavigateCommand { get; set; }
        public void Navigate(string uri)
        {
            _regionManager.RequestNavigate("CurrentQuestionType", uri);
        }

        public DelegateCommand NextQuestionCommand { get; set; }
        public void NextQuestion()
        {
            CurrentQuestionIndex++;
            CurrentWorkingQuestion = QuestionLists[CurrentQuestionIndex];
            CurrentMainQuestion = Quiz.Questions[CurrentQuestionIndex].MainQuestion;
        }
        public DelegateCommand PreviousQuestionCommand { get; set; }
        public void PreviousQuestion()
        {
            CurrentQuestionIndex--;
            CurrentWorkingQuestion = QuestionLists[CurrentQuestionIndex];
            CurrentMainQuestion = Quiz.Questions[CurrentQuestionIndex].MainQuestion;
        }
        public DelegateCommand FinishQuizCommand { get; set; }
        public void FinishQuiz()
        {
            // Navigate to Quiz View
            _regionManager.RequestNavigate("ContentRegion", "QuizzesShowView");
        }
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
        private void BuildWorkingQuestions()
        {
            for (int i = 0; i < Quiz.Questions.Count; i++)
            {
                if (Quiz.Questions[i].QuestionType == QuestionTypes.TRUEFALSE.ToString())
                {
                    var question = new TrueFalseQuizViewModel();
                    QuestionLists.Add(question);
                }
                else
                {
                    var question = new MultiChoice4QuizViewModel();
                    question.MultiChoiceAnswerQuestion1 = Quiz.Questions[i].MultiAnswerList[0];
                    question.MultiChoiceAnswerQuestion2 = Quiz.Questions[i].MultiAnswerList[1];
                    question.MultiChoiceAnswerQuestion3 = Quiz.Questions[i].MultiAnswerList[2];
                    question.MultiChoiceAnswerQuestion4 = Quiz.Questions[i].MultiAnswerList[3];
                    QuestionLists.Add(question);
                }
            }
        }

        #endregion
    }
}
