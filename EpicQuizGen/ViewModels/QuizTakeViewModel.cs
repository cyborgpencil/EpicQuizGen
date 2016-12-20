using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using EpicQuizGen.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows;

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

        //Binding for yes/no Popup when finish is clicked
        private bool _isCompleteOpen;
        public bool IsCompleteOpen
        {
            get { return _isCompleteOpen; }
            set { SetProperty(ref _isCompleteOpen, value); }
        }
        // Popup for Final Quiz info
        private bool _isFinalInfoOpen;
        public bool IsFinalInfoOpen
        {
            get { return _isFinalInfoOpen; }
            set { SetProperty(ref _isFinalInfoOpen, value); }
        }
        private bool _isQuizComplete;
        public bool IsQuizComplete
        {
            get { return _isQuizComplete; }
            set { SetProperty(ref _isQuizComplete, value); }
        }
        private int _currentCorrectAnswers;
        public int CurrentCorrectAnswers
        {
            get { return _currentCorrectAnswers; }
            set { SetProperty(ref _currentCorrectAnswers, value); }
        }
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
            YesEndQuizCommand = new DelegateCommand(YesEndQuiz);
            NoEndQuizCommand = new DelegateCommand(NoEndQuiz);
            ConfirmOKCommand = new DelegateCommand(ConfirmOK);
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

            // Clear Question list
            QuestionLists = new ObservableCollection<QuizViewModelbase>();

            BuildWorkingQuestions();

            // Set current Question
            QuestionNavIndex = 0;

            // Set Up Timer For Quiz
            Timer = int.Parse(Quiz.QuizTime);
            QuizTimer = new QuizTimeManager(this);
            QuizTimer.StartTimer();

            // Make sure Quiz Complete is false
            IsQuizComplete = false;

            // Make sure Popups are all false
            IsCompleteOpen = false;
            IsFinalInfoOpen = false;

            // Clear correct answers
            CurrentCorrectAnswers = 0;

            CurrentWorkingQuestion = QuestionLists[CurrentQuestionIndex];
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
            if(QuestionLists[CurrentQuestionIndex].GetType() == typeof(TrueFalseQuizViewModel))
            {
                var newToF = QuestionLists[CurrentQuestionIndex] as TrueFalseQuizViewModel;
                
            }
            
            CurrentMainQuestion = Quiz.Questions[CurrentQuestionIndex].MainQuestion;
        }
        public DelegateCommand FinishQuizCommand { get; set; }
        public void FinishQuiz()
        {
            // Ask user if they are sure
            // only if popup is already closed
            if(!IsCompleteOpen)
            ShowCompletePopup();

            // Check if Quiz has ended or user ended quiz
            if (IsQuizComplete)
            {
                CompleteQuiz();
            }
            else
                return;
        }
        public DelegateCommand YesEndQuizCommand { get; set; }
        public void YesEndQuiz()
        {
            // Close Popup
            IsCompleteOpen = false;

            // Show Final Info Popup
            IsQuizComplete = true;

            FinishQuiz();
            
        }
        public DelegateCommand NoEndQuizCommand { get; set; }
        public void NoEndQuiz()
        {
            // Set Completed to True
            IsQuizComplete = false;

            // Close Popup
            IsCompleteOpen = false;
        }
        public DelegateCommand ConfirmOKCommand { get; set; }
        public void ConfirmOK()
        {
            CurrentQuestionIndex = 0;

            // Navigate to Quiz List Screen
            _regionManager.RequestNavigate("ContentRegion", "QuizzesShowView");
        }
        #endregion

        #region Events
        internal void QuizTimerEvent(object sender, EventArgs e)
        {
            Timer = QuizTimer.SendCurrentSecound();
            TimeLeftString = "";
            if (Timer == 0)
            {
               CompleteQuiz();
            }
        }
        #endregion

        #region Methods
        private void CompleteQuiz()
        {
            // Calulate Score
            CalculateScore();

            // Show Quiz Grade
            IsFinalInfoOpen = true;

            // Save Quiz Grade

        }
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
        private void ShowCompletePopup()
        {
            IsCompleteOpen = true;
        }

        private void CalculateScore()
        {

            for(int i = 0; i < Quiz.Questions.Count;++i)
            {
                
                if(QuestionLists[i].GetType() == typeof(TrueFalseQuizViewModel) )
                {
                    
                    CheckCorrectTrueFalse(Quiz.Questions[i], QuestionLists[i] as TrueFalseQuizViewModel);
                }
                if (QuestionLists[i].GetType() == typeof(MultiChoice4QuizViewModel))
                {
                    CheckCorrectMultiChoice4(Quiz.Questions[i], QuestionLists[i] as MultiChoice4QuizViewModel);
                }
            }

            //BEBUG
            Quiz.Grade = (100.0f / Quiz.Questions.Count) * CurrentCorrectAnswers;
        }
        private void CheckCorrectTrueFalse(Question quizQuestion, TrueFalseQuizViewModel question)
        {
            // Check if question has been answered
            if (question.Answered)
            {
                if (question.TrueAnswer == quizQuestion.TrueAnswer)
                {
                    if (question.FalseAnswer == quizQuestion.FalseAnswer)
                    {
                        CurrentCorrectAnswers++;
                    }
                }
                
            }
        }

        private void CheckCorrectMultiChoice4(Question quizQuestion, MultiChoice4QuizViewModel question)
        {
            // Check if question has been answered
            if (question.Answered)
            {
                if (question.MultiChoiceAnswer1 == quizQuestion.MultiAnswerPositions[0])
                {
                    if (question.MultiChoiceAnswer2 == quizQuestion.MultiAnswerPositions[1])
                    {
                        if (question.MultiChoiceAnswer3 == quizQuestion.MultiAnswerPositions[2])
                        {
                            if (question.MultiChoiceAnswer4 == quizQuestion.MultiAnswerPositions[3])
                            {
                                CurrentCorrectAnswers++;
                            }
                        }
                    }

                }
                
            }
        }
        #endregion
    }
}
