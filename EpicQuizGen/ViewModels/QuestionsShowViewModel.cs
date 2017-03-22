﻿using EpicQuizGen.Events;
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
            set { SetProperty(ref _question, value); SendEditQuestion(); }
        }
        private readonly IEventAggregator _eventAggregator;
        public QuestionsShowViewModel()
        {
        }
        public QuestionsShowViewModel(IEventAggregator eventAggregator) : this()
        {
            _eventAggregator = eventAggregator;

            QuestionView = new QuestionView();

            // Command setup
            QuestionShowLoadCommand = new DelegateCommand(LoadQuestionShow);

            Questions = new ObservableCollection<Question>(QuestionIOManager.Instance.LoadQuestionsFromFile());

            //Events Setup
            
            _eventAggregator.GetEvent<SendQuestionNameEvent>().Subscribe(SetQuestionName);
            
            _eventAggregator.GetEvent<SendQuestionCategoryEvent>().Subscribe(SetQuestionCategory);
            _eventAggregator.GetEvent<SendMainQuestionEvent>().Subscribe(SetMainQuestion);
            _eventAggregator.GetEvent<SendTrueEvent>().Subscribe(SetTrue);
            _eventAggregator.GetEvent<SendFalseEvent>().Subscribe(SetFalse);

            _eventAggregator.GetEvent<SendMultiAnswerListEvent>().Subscribe(SetMultiAnswerList);
            _eventAggregator.GetEvent<SendMultiAnswer1Event>().Subscribe(SetMultiAnswer1);
            _eventAggregator.GetEvent<SendMultiAnswer2Event>().Subscribe(SetMultiAnswer2);
            _eventAggregator.GetEvent<SendMultiAnswer3Event>().Subscribe(SetMultiAnswer3);
            _eventAggregator.GetEvent<SendMultiAnswer4Event>().Subscribe(SetMultiAnswer4);
            _eventAggregator.GetEvent<SendQuestionEvent>().Subscribe(UpdateList);
            _eventAggregator.GetEvent<SendDeleteToUpdateList>().Subscribe(DeleteUpdateList);
        }

        #region Commands
        public DelegateCommand QuestionShowLoadCommand { get; set; }
        public void LoadQuestionShow()
        {
            SendQuestion();
        }

        public void UpdateList(object obj)
        {
            Questions = new ObservableCollection<Question>( QuestionIOManager.Instance.LoadQuestionsFromFile());
        }

        public void DeleteUpdateList(bool obj)
        {
            if(obj == true)
             Questions = new ObservableCollection<Question>(QuestionIOManager.Instance.LoadQuestionsFromFile());
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

        public void SetQuestionCategory(QuestionCategory obj)
        {
            Question.QuestionCategory.CategoryName = obj.ToString();
        }

        public void SetTrue(bool obj)
        {
            Question.TrueAnswer = obj;
        }
        public void SetFalse(bool obj)
        {
            Question.FalseAnswer = obj;
        }

        public void SetMultiAnswerList(List<string> obj)
        {
            Question.MultiAnswerList = obj;
        }
        public void SetMultiAnswer1(string obj)
        {
            Question.MultiAnswerList[0] = obj;
        }
        public void SetMultiAnswer2(string obj)
        {
            Question.MultiAnswerList[1] = obj;
        }
        public void SetMultiAnswer3(string obj)
        {
            Question.MultiAnswerList[2] = obj;
        }
        public void SetMultiAnswer4(string obj)
        {
            Question.MultiAnswerList[3] = obj;
        }
        public void SendQuestion()
        {
            _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
        }
        public void SetQuestion(Question obj)
        {
            Question = obj;
        }
        #endregion

        private void LoadQuestion()
        {
            _eventAggregator.GetEvent<SendQuestionFromEditEvent>().Publish(Question);
        }


        private void SendEditQuestion()
        {
            //send Question
            if (Question != null)
            {
                if (!string.IsNullOrWhiteSpace(Question.QuestionName))
                {
                    _eventAggregator.GetEvent<SendSelectedQuestionEvent>().Publish(Question);
                }
            }
            
        }
    }
}
