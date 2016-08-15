using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EpicQuizGen.ViewModels
{
    public class QuizzesShowViewModel : BindableBase
    {
        #region Properties
        /// <summary>
        /// Current working quiz
        /// </summary>
        private Quiz _currentQuiz;
        public Quiz CurrentQuiz
        {
            get { return _currentQuiz; }
            set { SetProperty(ref _currentQuiz, value); Debug.WriteLine("Changing Quiz"); }
        }

        /// <summary>
        /// Current Working List of Quizes
        /// </summary>
        /// 
        private ObservableCollection<Quiz> _quizList;
        public ObservableCollection<Quiz> QuizList
        {
            get { return _quizList; }
            set { SetProperty(ref _quizList, value); }
        }

        public List<string> QuestionCategories { get; set; }

        private string _questionCount;
        public string QuestionCount
        {
            get { return _questionCount; }
            set { SetProperty(ref _questionCount, value); }
        }
        #endregion

        public QuizzesShowViewModel()
        {
            QuizList = new ObservableCollection<Quiz>();
            CurrentQuiz = new Quiz();

            // Get list of QuestionCategories to a string
            QuestionCategories = new List<string>(Enum.GetNames(typeof(QuestionTypes)));

            // Commands
            NewQuizCommand = new DelegateCommand(NewQuiz);
            SaveQuizCommand = new DelegateCommand(SaveQuiz);

            // Load Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
        }

        #region Commands

        public DelegateCommand NewQuizCommand { get; set; }
        public void NewQuiz()
        {
            CurrentQuiz = new Quiz();
            Debug.WriteLine("Create created");
        }

        public DelegateCommand SaveQuizCommand { get; set; }
        public void SaveQuiz()
        {
            BuildQuiz();
            QuizIOManager.Instance.Quiz = CurrentQuiz;
            QuizIOManager.Instance.SaveQuiz();

            // Update Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Utility to build a Quiz
        /// </summary>
        public void BuildQuiz()
        {
            CurrentQuiz.CreationDate = DateTime.Now;
            // Build Question Count for list
            CurrentQuiz.Questions = new List<Question>(ConvertQuestionCount(QuestionCount));
            for (int i = 0; i < CurrentQuiz.Questions.Count; i++)
            {
                CurrentQuiz.Questions[i] = new Question();
            }
        }

        private int ConvertQuestionCount(string count)
        {
            int result;
            int.TryParse(count, out result);
            return result;
        }
        #endregion
    }
}
