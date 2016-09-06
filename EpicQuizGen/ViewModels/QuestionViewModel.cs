
using EpicQuizGen.Events;
using EpicQuizGen.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
/// <summary>
/// View Model for the Question View
/// </summary>
namespace EpicQuizGen.ViewModels
{
    public class QuestionViewModel :BindableBase
    {
        #region Properties
      

        private string _bindQuestionName;
        public string BindQuestionName
        {
            get { return _bindQuestionName; }
            set { SetProperty(ref _bindQuestionName, value); }
        }

        /// <summary>
        /// Properties for GUI
        /// </summary>
        /// 
        private string _questionName;
        public string QuestionName
        {
            get { return _questionName; }
            set { SetProperty(ref _questionName, value); Question.QuestionName = value; _eventAggregator.GetEvent<SendQuestionNameEvent>().Publish(Question.QuestionName); }
        }

        private List<string> _categoryList;
        public List<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value);

            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); Question.QuestionCategory = value;
                SendCategory();
            }
        }

        private string _selectedQuestionType;
        public string SelectedQuestionType
        {
            get { return _selectedQuestionType; }
            set { SetProperty(ref _selectedQuestionType, value);Question.QuestionType = value; SendQuestionTypes(); }
        }

        private ObservableCollection<string> _questionTypeList;
        public ObservableCollection<string> QuestionTypeList
        {
            get { return _questionTypeList; }
            set { SetProperty(ref _questionTypeList, value); }
        }

        private string _mainQuestion;
        public string MainQuestion
        {
            get { return _mainQuestion; }
            set { SetProperty(ref _mainQuestion, value); Question.MainQuestion = value; SendMainQuestion(); }
        }

        private List<string> _answerList;
        public List<string> AnswerList
        {
            get { return _answerList; }
            set { SetProperty(ref _answerList, value); }
        }

        private bool _fasleAnswer;
        public bool FalseAnswer
        {
            get { return _fasleAnswer; }
            set { SetProperty(ref _fasleAnswer, value); SendFalse(); }
        }
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); SendTrue(); }
        }
        private List<bool> _multichoiceAnswersPositions;
        public List<bool> MultichoiceAnswersPositions
        {
            get { return _multichoiceAnswersPositions; }
            set { SetProperty(ref _multichoiceAnswersPositions, value); }
        }
        private string _answer1;
        public string Answer1
        {
            get { return _answer1; }
            set { SetProperty(ref _answer1, value); SendMultiAnswer1();}
        }
        private string _answer2;
        public string Answer2
        {
            get { return _answer2; }
            set { SetProperty(ref _answer2, value); SendMultiAnswer2();}
        }
        private string _answer3;
        public string Answer3
        {
            get { return _answer3; }
            set { SetProperty(ref _answer3, value); SendMultiAnswer3();}
        }

        private string _answer4;
        public string Answer4
        {
            get { return _answer4; }
            set { SetProperty(ref _answer4, value); SendMultiAnswer4();}
        }

        private Question _question;
        public Question Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value); _eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(SetQuestion); }
        }

        private string _selectedType;
        public string SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); Question.QuestionType = value; Navigate(Question.QuestionType); }
        }
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region RegionManager
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand<string> QuestionViewLoadCommand { get; set; }

        #endregion

        public QuestionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            QuestionViewLoadCommand = new DelegateCommand<string>(QuestionViewLoad);

            if(Question == null)
            {
                SetDefaultQuestion();
            }

            CategoryList = new List<string>(Enum.GetNames(typeof(QuestionCategory)));
            QuestionTypeList = new ObservableCollection<string>(Enum.GetNames(typeof(QuestionTypes)));

            MultichoiceAnswersPositions = new List<bool>() { false, false, false, false };
            AnswerList = new List<string>() { "", "", "", "" };
            Answer1 = AnswerList[0];
            Answer2 = AnswerList[1];
            Answer3 = AnswerList[2];
            Answer4 = AnswerList[3];

            UpdateMultiAnswerPositionsCommand = new DelegateCommand(UpdateMultiAnswerPosition);

            // Build Question Model from parent
            _eventAggregator.GetEvent<SendSelectedQuestionEvent>().Subscribe(SetQuestion);
            //_eventAggregator.GetEvent<SendQuestionFromEditEvent>().Publish(Question);
            _eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(SetQuestion);
            _eventAggregator.GetEvent<SendQuestionEdit>().Subscribe(SetEditQuestion);

            //DEBUG
            TestBox_TextChanged = new DelegateCommand(Test);
        }

        #region Commands
        private void Navigate(string uri)
        {
            
            if(string.IsNullOrWhiteSpace(uri))
            {
                uri = "TRUEFALSE";
            }

            QuestionTypes QuestionTypeSelected = (QuestionTypes)Enum.Parse(typeof(QuestionTypes), uri);

            switch (QuestionTypeSelected)
            {
                case QuestionTypes.MULTICHOICE4:
                    _regionManager.RequestNavigate("AnswerSets", "MultiChoice4View");
                    break;
                default:
                    _regionManager.RequestNavigate("AnswerSets", "TrueFalseView");
                    break;
            }
            
        }

        private void QuestionViewLoad(string uri)
        {
            _eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(SetQuestion);
            Navigate(uri);
        }

        #endregion

        #region Events

        public void SendMainQuestion()
        {
            _eventAggregator.GetEvent<SendMainQuestionEvent>().Publish(MainQuestion);
        }
        public void SendQuestionTypes()
        {
            _eventAggregator.GetEvent<SendQuestionTypesEvent>().Publish((QuestionTypes)Enum.Parse(typeof(QuestionTypes), Question.QuestionType));
            Navigate(Question.QuestionType);
        }
        public void SendCategory()
        {
            _eventAggregator.GetEvent<SendQuestionCategoryEvent>().Publish((QuestionCategory)Enum.Parse(typeof(QuestionCategory), Question.QuestionCategory));
        }

        public void SendTrue()
        {
            _eventAggregator.GetEvent<SendTrueEvent>().Publish(TrueAnswer);
        }
        public void SendFalse()
        {
            _eventAggregator.GetEvent<SendFalseEvent>().Publish(FalseAnswer);
        }

        public void SendMultiAnswerPositions()
        {
            _eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Publish(MultichoiceAnswersPositions);
        }

        public DelegateCommand UpdateMultiAnswerPositionsCommand { get; set; }
        public void UpdateMultiAnswerPosition()
        {
            _eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Publish(MultichoiceAnswersPositions);
        }
        public void SendMultiAnswer1()
        {
            _eventAggregator.GetEvent<SendMultiAnswer1Event>().Publish(Answer1);
        }
        public void SendMultiAnswer2()
        {
            _eventAggregator.GetEvent<SendMultiAnswer2Event>().Publish(Answer2);
        }
        public void SendMultiAnswer3()
        {
            _eventAggregator.GetEvent<SendMultiAnswer3Event>().Publish(Answer3);
        }
        public void SendMultiAnswer4()
        {
            _eventAggregator.GetEvent<SendMultiAnswer4Event>().Publish(Answer4);
        }

        public void SendMultiAnswerList()
        {
            AnswerList = new List<string>();
            AnswerList.Add(Answer1);
            AnswerList.Add(Answer2);
            AnswerList.Add(Answer3);
            AnswerList.Add(Answer4);

            _eventAggregator.GetEvent<SendMultiAnswerListEvent>().Publish(AnswerList);
        }

        public void SetQuestion(Question obj)
        {

            Question = obj;
            QuestionName = obj.QuestionName;
            Question.MultiAnswerPositions = obj.MultiAnswerPositions;
            Question.MultiAnswerList = obj.MultiAnswerList;
            // Navigate
            SelectedQuestionType = Question.QuestionType;
            SelectedCategory = Question.QuestionCategory;
            TrueAnswer = Question.TrueAnswer;
            FalseAnswer = Question.FalseAnswer;
            MainQuestion = Question.MainQuestion;
            if (Question.QuestionType == QuestionTypes.MULTICHOICE4.ToString() && Question.QuestionName != "")
            {
                MultichoiceAnswersPositions =  new List<bool>(Question.MultiAnswerPositions);
                AnswerList = new List<string>(Question.MultiAnswerList);
                Answer1 = obj.MultiAnswerList[0];
                Answer2 = obj.MultiAnswerList[1];
                Answer3 = obj.MultiAnswerList[2];
                Answer4 = obj.MultiAnswerList[3];
            }else
            {
                ClearAnswersAndMultiChoice();
            }

        }
        public void SetEditQuestion(Question obj)
        {
            Question = obj;
            QuestionName = Question.QuestionName;
            MainQuestion = Question.MainQuestion;
            Question.MultiAnswerPositions = Question.MultiAnswerPositions;
            Question.MultiAnswerList = Question.MultiAnswerList;
            // Navigate
            SelectedQuestionType = Question.QuestionType;
            SelectedCategory = Question.QuestionCategory;
            TrueAnswer = Question.TrueAnswer;
            FalseAnswer = Question.FalseAnswer;
            if (Question.QuestionType == QuestionTypes.MULTICHOICE4.ToString() && Question.QuestionName != "")
            {
                MultichoiceAnswersPositions = new List<bool>(Question.MultiAnswerPositions);
                AnswerList = new List<string>(Question.MultiAnswerList);
                Answer1 = Question.MultiAnswerList[0];
                Answer2 = Question.MultiAnswerList[1];
                Answer3 = Question.MultiAnswerList[2];
                Answer4 = Question.MultiAnswerList[3];
            }
        }
        #endregion

        private void SetDefaultQuestion()
        {
            Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE.ToString(), QuestionCategory = QuestionCategory.MISC.ToString(), MultiAnswerPositions = new List<bool>() { false, false, false, false, }, MultiAnswerList = new List<string>() { "", "", "", "" }, TrueAnswer = false, FalseAnswer = false ,CreationDate = DateTime.Now };
        }
        private void ClearAnswersAndMultiChoice()
        {
            Answer1 = "";
            Answer2 = "";
            Answer3 = "";
            Answer4 = "";

            for (int i = 0; i < MultichoiceAnswersPositions.Count; i++)
            {
                MultichoiceAnswersPositions[i] = false;
            }
        }

        #region DEBUG
        public DelegateCommand TestBox_TextChanged { get; set; }
        public void Test()
        {
            foreach (var a in AnswerList)
            {
                Debug.WriteLine(a);
            }
            
        }
        #endregion
    }
}
