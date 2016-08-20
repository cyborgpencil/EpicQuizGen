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
                SetDefaultQuestion();
            }

            QuestionView = new QuestionView();

            // Command setup
            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
            EditQuestionCommand = new DelegateCommand(EditQuestion);
            NewQuestionCommand = new DelegateCommand(NewQuestion);
            DeleteQuestionCommand = new DelegateCommand(DeleteQuestion);
            QuestionShowLoadCommand = new DelegateCommand(LoadQuestionShow);

            Questions = new ObservableCollection<Question>(QuestionIOManager.Instance.LoadQuestionsFromFile());

            //Events Setup
            
            _eventAggregator.GetEvent<SendQuestionNameEvent>().Subscribe(SetQuestionName);
            _eventAggregator.GetEvent<SendQuestionTypesEvent>().Subscribe(SetQuestionType);
            _eventAggregator.GetEvent<SendQuestionCategoryEvent>().Subscribe(SetQuestionCategory);
            _eventAggregator.GetEvent<SendMainQuestionEvent>().Subscribe(SetMainQuestion);
            _eventAggregator.GetEvent<SendTrueFalseEvent>().Subscribe(SetTrueFalse);
            _eventAggregator.GetEvent<SendMultiAnswerPositionsEvent>().Subscribe(SetMuliAnswerPositions);
            _eventAggregator.GetEvent<SendMultiAnswerListEvent>().Subscribe(SetMultiAnswerList);
            _eventAggregator.GetEvent<SendMultiAnswer1Event>().Subscribe(SetMultiAnswer1);
            _eventAggregator.GetEvent<SendMultiAnswer2Event>().Subscribe(SetMultiAnswer2);
            _eventAggregator.GetEvent<SendMultiAnswer3Event>().Subscribe(SetMultiAnswer3);
            _eventAggregator.GetEvent<SendMultiAnswer4Event>().Subscribe(SetMultiAnswer4);
        }

        #region Commands

        public DelegateCommand SaveQuestionCommand { get; set; }
        public void SaveQuestion()
        {

            Question.CreationDate = DateTime.Now;
            QuestionIOManager.Instance.QuestionModel = Question;
            QuestionIOManager.Instance.SaveQuestionModel();

            // Update Question List
            Questions = new ObservableCollection<Question>( QuestionIOManager.Instance.LoadQuestionsFromFile());

            // Clear question
            SetDefaultQuestion();
            SendQuestion();
            
        }

        public DelegateCommand QuestionShowLoadCommand { get; set; }
        public void LoadQuestionShow()
        {
            SetDefaultQuestion();
            SendQuestion();
        }

        public DelegateCommand EditQuestionCommand { get; set; }
        public void EditQuestion()
        {
            _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
        }

        public DelegateCommand DeleteQuestionCommand { get; set; }
        public void DeleteQuestion()
        {
            //if(!string.IsNullOrWhiteSpace(Question.QuestionName))
            //{
                QuestionIOManager.Instance.DeleteQuestionFromFile(Question.QuestionName);
                
                Questions = new ObservableCollection<Question>(QuestionIOManager.Instance.LoadQuestionsFromFile());
            SetDefaultQuestion();
            // }
        }

        public DelegateCommand NewQuestionCommand { get; set; }
        public void NewQuestion()
        {
            SetDefaultQuestion();
            _eventAggregator.GetEvent<SendQuestionEvent>().Publish(Question);
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
            if (Question == null)
                SetDefaultQuestion();
            
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

        private void SetDefaultQuestion()
        {
            Question = new Question() { QuestionName = "", MainQuestion = "", QuestionType = QuestionTypes.TRUEFALSE.ToString(), QuestionCategory = QuestionCategory.MISC.ToString(), MultiAnswerPositions = new List<bool>() { false, false, false, false, }, MultiAnswerList = new List<string>() { "", "", "", "" }, TrueFalseAnswer = false, CreationDate = DateTime.Now };
        }
    }
}
