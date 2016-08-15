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

        public List<string> QuestionCategoriesSelect { get; set; }

        private string _questionCount;
        public string QuestionCount
        {
            get { return _questionCount; }
            set { SetProperty(ref _questionCount, value); }
        }
        
        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }
        #endregion

        public QuizzesShowViewModel()
        {
            QuizList = new ObservableCollection<Quiz>();
            CurrentQuiz = new Quiz();

            SelectedCategory = QuestionCategory.MISC.ToString();
            // Get list of QuestionCategoriesSelect to a string
            QuestionCategoriesSelect = new List<string>(Enum.GetNames(typeof(QuestionCategory)));

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
            CurrentQuiz.Questions = QuestionIOManager.Instance.GetQuestionsByCategory(SelectedCategory, ConvertQuestionCount(QuestionCount));
            QuizIOManager.Instance.Quiz = CurrentQuiz;
            QuizIOManager.Instance.SaveQuiz();

            // Update Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
            CurrentQuiz = null;
            QuestionCount = "";
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
