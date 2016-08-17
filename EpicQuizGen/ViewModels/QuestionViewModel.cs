﻿
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

        private bool _correctToFAnswer;
        public bool CorrectToFAnswer
        {
            get { return _correctToFAnswer; }
            set { SetProperty(ref _correctToFAnswer, value); SendTrueFalse(); }
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
            set { SetProperty(ref _answer1, value); SendMultiAnswerList(); }
        }
        private string _answer2;
        public string Answer2
        {
            get { return _answer2; }
            set { SetProperty(ref _answer2, value); SendMultiAnswerList(); }
        }
        private string _answer3;
        public string Answer3
        {
            get { return _answer3; }
            set { SetProperty(ref _answer3, value); SendMultiAnswerList(); }
        }

        private string _answer4;
        public string Answer4
        {
            get { return _answer4; }
            set { SetProperty(ref _answer4, value); SendMultiAnswerList(); }
        }

        private Question _question;
        public Question Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value); }
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
        public DelegateCommand<string> QuestionViewCommand { get; set; }

        #endregion

        public QuestionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            QuestionViewCommand = new DelegateCommand<string>(QuestionView);

            if(Question == null)
            {
                SetDefaultQuestion();
            }

            CategoryList = new List<string>(Enum.GetNames(typeof(QuestionCategory)));
            QuestionTypeList = new ObservableCollection<string>(Enum.GetNames(typeof(QuestionTypes)));


            AnswerList = new List<string> (){"","","","" };

            MultichoiceAnswersPositions = new List<bool>() { false, false, false, false };

            UpdateMultiAnswerPositionsCommand = new DelegateCommand(UpdateMultiAnswerPosition);

            // Build Question Model from parent
            _eventAggregator.GetEvent<SendSelectedQuestionEvent>().Subscribe(SetQuestion);
            _eventAggregator.GetEvent<SendQuestionFromEditEvent>().Publish(Question);
            _eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(SetQuestion);

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

        private void QuestionView(string uri)
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
        }
        public void SendCategory()
        {
            _eventAggregator.GetEvent<SendQuestionCategoryEvent>().Publish((QuestionCategory)Enum.Parse(typeof(QuestionCategory), Question.QuestionCategory));
        }

        public void SendTrueFalse()
        {
            _eventAggregator.GetEvent<SendTrueFalseEvent>().Publish(CorrectToFAnswer);
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

        public void SendMultiAnswerList()
        {
            AnswerList[0] = Answer1;
            AnswerList[1] = Answer2;
            AnswerList[2] = Answer3;
            AnswerList[3] = Answer4;

            _eventAggregator.GetEvent<SendMultiAnswerListEvent>().Publish(AnswerList);
        }

        public void SetQuestion(Question obj)
        {
            Question = obj;
            // Navigate
            SelectedQuestionType = Question.QuestionType;
        }
        #endregion

        private void SetDefaultQuestion()
        {
            Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE.ToString(), QuestionCategory = QuestionCategory.MISC.ToString(), MultiAnswerPositions = new List<bool>() { false, false, false, false, }, MultiAnswerList = new List<string>() { "", "", "", "" }, TrueFalseAnswer = false, CreationDate = DateTime.Now };
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
