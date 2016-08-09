
using EpicQuizGen.Events;
using EpicQuizGen.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
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
        private QuestionCategory _questionCategory;
        public QuestionCategory QuestionCategory
        {
            get { return _questionCategory; }
            set { SetProperty(ref _questionCategory, value);}
        }

        private List<string> _categoryList;
        public List<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        private QuestionTypes _questionTypes;
        public QuestionTypes QuestionTypes
        {
            get { return _questionTypes; }
            set { SetProperty(ref _questionTypes, value); Navigate(value); }
        }

        private List<string> _questionTypeList;
        public List<string> QuestionTypeList
        {
            get { return _questionTypeList; }
            set { SetProperty(ref _questionTypeList, value); }
        }

        private string _questionName;
        public string QuestionName
        {
            get { return _questionName; }
            set { SetProperty(ref _questionName, value); SendQuestionName(); }
        }

        private readonly IEventAggregator _eventAggregator;

        #endregion

        public QuestionViewModel()
        {
        }

        public QuestionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            CategoryList = new List<string>(Enum.GetNames(typeof(QuestionCategory)));
            QuestionTypeList = new List<string>(Enum.GetNames(typeof(QuestionTypes)));

            _regionManager = regionManager;

            _regionManager.RequestNavigate("AnswerSets", "TrueFalseView");

            // NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        #region RegionManager
        private readonly IRegionManager _regionManager;
        

        public DelegateCommand<string> NavigateCommand { get; set; }

        #endregion

        #region Commands
        private void Navigate(QuestionTypes questionTypes)
        {
            switch (questionTypes)
            {
                case QuestionTypes.MULTICHOICE4:
                    _regionManager.RequestNavigate("AnswerSets", "MultiChoice4View");
                    break;
                default:
                    _regionManager.RequestNavigate("AnswerSets", "TrueFalseView");
                    break;
            }
            
        }


        #endregion

        #region Events
        public void SendQuestionName()
        {
            _eventAggregator.GetEvent<SendQuestionNameEvent>().Publish(QuestionName);
        }
        #endregion
    }
}
