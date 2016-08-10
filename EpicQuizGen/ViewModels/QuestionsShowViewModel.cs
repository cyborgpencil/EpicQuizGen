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

            Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE, QuestionCategory = QuestionCategory.MISC, MultiAnswerPositions = new List<bool>() { false, false, false, false, }, MultiAnswerList = new List<string>() { "", "", "", "" }, TrueFalseAnswer = false, CreationDate = DateTime.Now };
            QuestionView = new QuestionView();
            eventAggregator.GetEvent<SendQuestionNameEvent>().Subscribe(SetQuestionName);
            eventAggregator.GetEvent<SendMainQuestionEvent>().Subscribe(SetMainQuestion);
            eventAggregator.GetEvent<SendQuestionTypesEvent>().Subscribe(SetQuestionType);
            eventAggregator.GetEvent<SendCategoryEvent>().Subscribe(SetQuestionCategory);
            eventAggregator.GetEvent<SendTrueFalseEvent>().Subscribe(SetTrueFalse);
            eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Subscribe(SetMuliAnswerPositions);
            eventAggregator.GetEvent<SendMultiAnswerListEvent>().Subscribe(SetMultiAnswerList);

            Questions = new ObservableCollection<Question>();
            for (int i = 0; i < 20; i++)
            {
                Question question = new Question();   
                question.CreationDate = DateTime.Now;
                Questions.Add(question);
            }

            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
        }

        #region Commands

        public DelegateCommand SaveQuestionCommand { get; set; }
        public void SaveQuestion()
        {
            Question.CreationDate = DateTime.Now;
            Debug.WriteLine(string.Format("Question Model:\n\nQuesrionName: {0}\nMainQuestion: {1}\nQuestionType: {2}\nQuestionCategory: {3}\nAnswerPosition1: {4}\n AnswerPosition2: {5}\n AnswerPosition3: {6}\nAnswerPosition4: {7}\n TruFalseAnswer: {8}\nCreationDate: {9}\n",Question.QuestionName, Question.MainQuestion, Question.QuestionType, Question.QuestionCategory, Question.MultiAnswerList[0], Question.MultiAnswerList[1], Question.MultiAnswerList[2], Question.MultiAnswerList[3], Question.TrueFalseAnswer, Question.CreationDate.ToString()));
        }

        public void Save()
        {
            Debug.WriteLine(this.Question.QuestionType);
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
            Question.QuestionType = obj;
        }

        public void SetQuestionCategory(QuestionCategory obj)
        {
            Question.QuestionCategory = obj;
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
        #endregion
    }
}
