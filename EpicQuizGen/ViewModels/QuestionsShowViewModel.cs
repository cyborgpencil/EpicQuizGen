using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
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

            Questions = new ObservableCollection<Question>();
            for (int i = 0; i < 20; i++)
            {
                Question question = new Question();
                question.QuestionName = "Test" + i;
                question.QuestionCategory = new QuestionCategory();
                question.QuestionCategory = QuestionCategory.OPHTHALMIC;
                question.CreationDate = DateTime.Now;
                question.QuestionType = new QuestionTypes();
                question.QuestionType = QuestionTypes.TRUEFALSE;
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
        #endregion
    }
}
