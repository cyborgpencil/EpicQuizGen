using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using EpicQuizGen.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EpicQuizGen.ViewModels
{
    public class QuestionsShowViewModel : BindableBase
    {
        private QuestionView _questionView;
        public QuestionView QuestionView
        {
            get { return _questionView; }
            set { SetProperty(ref _questionView, value); }
        }
        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }
        private Question _question;
        public Question Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value); SendQuestion(); }
        }
        private readonly IEventAggregator _eventAggregator;

        public QuestionsShowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // Check for null Question
            if (Question == null)
            {
                Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE.ToString(), QuestionCategory = QuestionCategory.MISC.ToString(), MultiAnswerPositions = new List<bool>() { false, false, false, false, }, MultiAnswerList = new List<string>() { "", "", "", "" }, TrueFalseAnswer = false, CreationDate = DateTime.Now };
            }
            QuestionView = new QuestionView();
            _eventAggregator.GetEvent<SendQuestionNameEvent>().Subscribe(SetQuestionName);
            _eventAggregator.GetEvent<SendMainQuestionEvent>().Subscribe(SetMainQuestion);
            _eventAggregator.GetEvent<SendQuestionTypesEvent>().Subscribe(SetQuestionType);
            _eventAggregator.GetEvent<SendCategoryEvent>().Subscribe(SetQuestionCategory);
            _eventAggregator.GetEvent<SendTrueFalseEvent>().Subscribe(SetTrueFalse);
            _eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Subscribe(SetMuliAnswerPositions);
            _eventAggregator.GetEvent<SendMultiAnswerListEvent>().Subscribe(SetMultiAnswerList);
           

             SaveQuestionCommand = new DelegateCommand(SaveQuestion);

            Questions = new ObservableCollection<Question>(QuestionIOManager.Instance.GetQuestionsFromFile());

            _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
        }

        #region Commands

        public DelegateCommand SaveQuestionCommand { get; set; }
        public void SaveQuestion()
        {
            Question.CreationDate = DateTime.Now;
            QuestionIOManager.Instance.QuestionModel = Question;
            QuestionIOManager.Instance.SaveQuestionModel();

            // Update Question List
            Questions = new ObservableCollection<Question>( QuestionIOManager.Instance.GetQuestionsFromFile());
        }

        #endregion

        #region Events

        public void SetQuestionName(string obj)
        {
            Question.QuestionName = obj;
        }

        public void SetMainQuestion(string obj)
        {
            Question.MainQuestion = obj;
        }
        public void SetQuestionType(QuestionTypes obj)
        {
            Question.QuestionType = obj.ToString();
        }

        public void SetQuestionCategory(QuestionCategory obj)
        {
            Question.QuestionCategory = obj.ToString();
        }

        public void SetTrueFalse(bool obj)
        {
            Question.TrueFalseAnswer = obj;
        }

        public void SetMuliAnswerPositions(List<bool> obj)
        {
            Question.MultiAnswerPositions = obj;
        }

        public void SetMultiAnswerList(List<string> obj)
        {
            Question.MultiAnswerList = obj;
        }

        public void SendQuestion()
        {
            _eventAggregator.GetEvent<SendSelectedQuestionEvent>().Publish(Question);
        }
        #endregion
    }
}
