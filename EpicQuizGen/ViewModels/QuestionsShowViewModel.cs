using EpicQuizGen.Events;
using EpicQuizGen.Models;
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
            set { SetProperty(ref _question, value);}
        }

        public QuestionsShowViewModel(IEventAggregator eventAggregator)
        {
            
            Question = new Question();
            Question.QuestionName = "Test";
            QuestionView = new QuestionView();
            eventAggregator.GetEvent<SendQuestionNameEvent>().Subscribe(SetQuestionName);
            eventAggregator.GetEvent<SendMainQuestionEvent>().Subscribe(SetMainQuestion);
            eventAggregator.GetEvent<SendQuestionTypesEvent>().Subscribe(SetQuestionType);
            eventAggregator.GetEvent<SendCategoryEvent>().Subscribe(SetQuestionCategory);
            eventAggregator.GetEvent<SendTrueFalseEvent>().Subscribe(SetTrueFalse);
            eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Subscribe(SetMuliAnswerPositions);

            Questions = new ObservableCollection<Question>();
            for (int i = 0; i < 20; i++)
            {
                Question question = new Question();   
                question.CreationDate = DateTime.Now;
                Questions.Add(question);
            }
        }

        #region Commands

        public DelegateCommand<string> SaveCommand { get; set; }

        public void Save()
        {
            Debug.WriteLine(this.Question.QuestionType);
        }

        #endregion

        #region Events

        public void SetQuestionName(string obj)
        {
            Question.QuestionName = obj;
            Debug.WriteLine(Question.QuestionName);
        }

        public void SetMainQuestion(string obj)
        {
            Question.MainQuestion = obj;
            Debug.WriteLine(Question.MainQuestion);
        }
        public void SetQuestionType(QuestionTypes obj)
        {
            Question.QuestionType = obj;
            Debug.WriteLine(Question.QuestionType);
        }

        public void SetQuestionCategory(QuestionCategory obj)
        {
            Question.QuestionCategory = obj;
            Debug.WriteLine(Question.QuestionCategory);
        }

        public void SetTrueFalse(bool obj)
        {
            Question.TrueFalseAnswer = obj;
            Debug.WriteLine(Question.TrueFalseAnswer);
        }

        public void SetMuliAnswerPositions(List<bool> obj)
        {
            Question.MultiAnswerPotions = obj;
            foreach (var m in Question.MultiAnswerPotions)
            {
                Debug.WriteLine(m);
            }
        }
        #endregion
    }
}
