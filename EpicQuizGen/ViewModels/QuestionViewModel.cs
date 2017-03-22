
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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
/// <summary>
/// View Model for the Question View
/// </summary>
namespace EpicQuizGen.ViewModels
{
    public class QuestionViewModel :BindableBase, IDataErrorInfo
    {
        #region Properties
      

        private string _bindQuestionName;
        public string BindQuestionName
        {
            get { return _bindQuestionName; }
            set { SetProperty(ref _bindQuestionName, value); }
        }

        // tracks if a question can be edited
        public bool _canEditQuestion;
        public bool CanEditQuestion
        {
            get { return _canEditQuestion; }
            set { SetProperty(ref _canEditQuestion, value); }
        }

        // track if question can be deleted
        public bool _canDeleteQuestion;
        public bool CanDeleteQuestion
        {
            get { return _canDeleteQuestion; }
            set { SetProperty(ref _canDeleteQuestion, value); }
        }

        /// <summary>
        /// Properties for GUI
        /// </summary>
        /// 
        private string _questionName;
        public string QuestionName
        {
            get { return _questionName; }
            set { SetProperty(ref _questionName, value); Question.QuestionName = value; }
        }

        private ObservableCollection<string> _categoryList;
        public ObservableCollection<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value);

            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); Question.QuestionCategory.CategoryName = value;
                //SendCategory();
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
            set { SetProperty(ref _mainQuestion, value); Question.MainQuestion = value; }
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
            set { SetProperty(ref _fasleAnswer, value); Question.FalseAnswer = value; }
        }
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); Question.TrueAnswer = value; }
        }
        private List<bool> _multichoiceAnswersPositions;
        public List<bool> MultichoiceAnswersPositions
        {
            get { return _multichoiceAnswersPositions; }
            set { SetProperty(ref _multichoiceAnswersPositions, value);}
        }
        private string _answer1;
        public string Answer1
        {
            get { return _answer1; }
            set { SetProperty(ref _answer1, value); Question.MultiAnswerList[0] = value; }
        }
        private string _answer2;
        public string Answer2
        {
            get { return _answer2; }
            set { SetProperty(ref _answer2, value); Question.MultiAnswerList[1] = value; }
        }
        private string _answer3;
        public string Answer3
        {
            get { return _answer3; }
            set { SetProperty(ref _answer3, value); Question.MultiAnswerList[2] = value; }
        }

        private string _answer4;
        public string Answer4
        {
            get { return _answer4; }
            set { SetProperty(ref _answer4, value); Question.MultiAnswerList[3] = value; }
        }

        private Question _question;
        public Question Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value);}
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

        public QuestionViewModel()
        {
            // clear question
            SetDefaultQuestion();

            SaveQuestionCommand = new DelegateCommand(SaveQuestion, CanExecuteSave).ObservesProperty(() => MainQuestion).ObservesProperty(()=>QuestionName);
            NewQuestionCommand = new DelegateCommand(NewQuestion);
            EditQuestionCommand = new DelegateCommand(EditQuestion, CanExecuteEdit).ObservesProperty(()=>CanEditQuestion);
            DeleteQuestionCommand = new DelegateCommand(DeleteQuestion, CanExecuteDelete).ObservesProperty(()=>CanEditQuestion);
            CategoryList = new ObservableCollection<string>();
        }
        public QuestionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : this()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            QuestionViewLoadCommand = new DelegateCommand<string>(QuestionViewLoad);

            QuestionTypeList = new ObservableCollection<string>(Enum.GetNames(typeof(QuestionTypes)));

            ClearAnswersAndMultiChoice();

            // Build Question Model from parent
            _eventAggregator.GetEvent<SendSelectedQuestionEvent>().Subscribe(SetEditQuestion);
   
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
            // clear question
            SetDefaultQuestion();

            CanEditQuestion = false;
            //_eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(SetQuestion);

            // load categories
            if (CategoriesIOManager.Instance.GetCategoriesFromFile())
            {
                CategoryList = new ObservableCollection<string>();
                foreach (var catName in CategoriesIOManager.Instance.LoadCategoriesFromFile())
                {
                    CategoryList.Add(catName.CategoryName);
                }
            }
            else
            {
                CategoryList = new ObservableCollection<string>();
                CategoryList.Add("Empty");
                Debug.WriteLine(CategoryList);
            }

            Navigate(uri);
        }

        public DelegateCommand SaveQuestionCommand { get; set; }
        public void SaveQuestion()
        {
            // Assemble Question
            Question.CreationDate = DateTime.Now;

            //Save Current Question
            QuestionIOManager.Instance.QuestionModel = Question;
            QuestionIOManager.Instance.SaveQuestionModel();

            // Clear question
            NewQuestion();
            _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
        }
        private bool CanExecuteSave()
        {
            // verify QuestionName and Main Question are not blank
            return !string.IsNullOrWhiteSpace(QuestionName) && !string.IsNullOrWhiteSpace(MainQuestion);
        }

        public DelegateCommand NewQuestionCommand { get; set; }
        public void NewQuestion()
        {
            SetDefaultQuestion();
            // _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
            CanEditQuestion = false;
        }

        public DelegateCommand EditQuestionCommand { get; set; }
        public void EditQuestion()
        {
            // Set Question
        }
        public bool CanExecuteEdit()
        {
            if (CanEditQuestion)
            {
                return true;
            }
            else
                return false;
        }
        public DelegateCommand DeleteQuestionCommand { get; set; }
        public void DeleteQuestion()
        {
            if(!string.IsNullOrWhiteSpace(Question.QuestionName))
            {
                // Delete Question
                QuestionIOManager.Instance.DeleteQuestionFromFile(Question.QuestionName);
                NewQuestion();

                // Send Delete Message to QuestionShowVM
                _eventAggregator.GetEvent<SendDeleteToUpdateList>().Publish(true);
            }
        }
        public bool CanExecuteDelete()
        {
            // delete base on if a question can be edited
            if (CanEditQuestion)
            {
                return true;
            }
            else
                return false;
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
            _eventAggregator.GetEvent<SendQuestionCategoryEvent>().Publish((QuestionCategory)Enum.Parse(typeof(QuestionCategory), Question.QuestionCategory.CategoryName));
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
            SelectedCategory = Question.QuestionCategory.CategoryName;
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
            SelectedCategory = Question.QuestionCategory.CategoryName;
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

            CanEditQuestion = true;
        }
        #endregion

        private void SetDefaultQuestion()
        {
            Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE.ToString(), QuestionCategory = new QuestionCategories(), MultiAnswerPositions = new List<bool>() { false, false, false, true, }, MultiAnswerList = new List<string>() { "", "", "", "All of the Above" }, TrueAnswer = false, FalseAnswer = true ,CreationDate = DateTime.Now };

            // reset GUI each time we get a new Question
            ClearAnswersAndMultiChoice();
        }

        // clears all the Properties out on the GUI
        private void ClearAnswersAndMultiChoice()
        {
            QuestionName = "";
            MainQuestion = "";
            TrueAnswer = Question.TrueAnswer;
            FalseAnswer = Question.FalseAnswer;
            Answer1 = Question.MultiAnswerList[0];
            Answer2 = Question.MultiAnswerList[1];
            Answer3 = Question.MultiAnswerList[2];
            Answer4 = Question.MultiAnswerList[3];
            MultichoiceAnswersPositions = Question.MultiAnswerPositions;
            
        }
        

        #region Validation
        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }
        readonly string[] ValidateProperties =
        {
            "QuestionName",
            "MainQuestion",
            "TrueAnswer",
            "FalseAnswer"
        };

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return GetPropertyValidationErrror(propertyName);
            }
        }

        public bool IsValid()
        {
            foreach (string property in ValidateProperties)
            {
                if (GetPropertyValidationErrror(property) != null)
                    return false;
            }

            return true;
        }

        string GetPropertyValidationErrror(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case "QuestionName":
                    error = ValidateQuestionName();
                    break;
                case "MainQuestion":
                    error = ValidateMainQuestion();
                    break;
                case "TrueAnswer":
                    error = ValidateTrueFalse();
                    break;
                case "FalseAnswer":
                    error = ValidateTrueFalse();
                    break;
            }
            return error;
        }

        private string ValidateTrueFalse()
        {
            if(TrueAnswer == false && _fasleAnswer == false)
            {
                return "True or False Must be selected";
            }
            return null;
        }

        private string ValidateQuestionName()
        {
            if (string.IsNullOrWhiteSpace(QuestionName))
            {
                return "QuestionName Cannot be Blank";
            }
            return null;
        }
        private string ValidateMainQuestion()
        {
            if (string.IsNullOrWhiteSpace(MainQuestion))
            {
                return "Main Question Cannot be Blank";
            }
            return null;
        }
        #endregion
    }
}
