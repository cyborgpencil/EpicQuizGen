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
        private Quiz _editQuiz;
        public Quiz EditQuiz
        {
            get { return _editQuiz; }
            set { SetProperty(ref _editQuiz, value); EditExecute(); }
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
        private string _quizName;
        public string QuizName
        {
            get { return _quizName; }
            set { SetProperty(ref _quizName, value); }
        }
        #endregion

        public QuizzesShowViewModel()
        {
            QuizList = new ObservableCollection<Quiz>();
            QuizName = "";
            QuestionCount = "1";
            SelectedCategory = QuestionCategory.MISC.ToString();
            BuildNewQuiz();

            SelectedCategory = QuestionCategory.MISC.ToString();
            // Get list of QuestionCategoriesSelect to a string
            QuestionCategoriesSelect = new List<string>(Enum.GetNames(typeof(QuestionCategory)));

            // Commands
            NewQuizCommand = new DelegateCommand(NewQuiz);
            SaveQuizCommand = new DelegateCommand(SaveQuiz);
            EditCommand = new DelegateCommand(EditExecute, EditCanExecute).ObservesProperty(() => EditQuiz);
            DeleteCommand = new DelegateCommand(DeleteQuiz);

            // Load Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
        }

        #region Commands

        public DelegateCommand NewQuizCommand { get; set; }
        public void NewQuiz()
        {
            BuildNewQuiz();
        }

        public DelegateCommand SaveQuizCommand { get; set; }
        public void SaveQuiz()
        {
            BuildSaveQuiz();
            EditQuiz = new Quiz();
            EditQuiz.Questions = new List<Question>();
            CurrentQuiz.Questions = QuestionIOManager.Instance.GetQuestionsByCategory(SelectedCategory, ConvertQuestionCount(QuestionCount));

            QuizIOManager.Instance.Quiz = CurrentQuiz;
            QuizIOManager.Instance.SaveQuiz();

            // Update Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
            BuildNewQuiz();
            QuestionCount = "1";
            QuizName = "";
            EditQuiz = new Quiz();

        }

        public DelegateCommand EditCommand { get; set; }
        public bool EditCanExecute()
        {
            
            return true;
        }
        public void EditExecute()
        {
            if (EditQuiz != null && EditQuiz.Questions != null)
            {
                CurrentQuiz = EditQuiz;
                QuizName = CurrentQuiz.QuizName;
                QuestionCount = CurrentQuiz.Questions.Count.ToString();
                SelectedCategory = CurrentQuiz.QuizCategory;
            }
        }
        public DelegateCommand DeleteCommand { get; set; }
        public void DeleteQuiz()
        {
            if (EditQuiz != null)
            {
                QuizIOManager.Instance.Quiz = EditQuiz;
                QuizIOManager.Instance.DeleteQuestionFromFile(EditQuiz.QuizName);

            }
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Utility to build a Quiz
        /// </summary>
        public void BuildNewQuiz()
        {
            CurrentQuiz = new Quiz();
            CurrentQuiz.CreationDate = DateTime.Now;
            // Build Question Count for list
            CurrentQuiz.Questions = new List<Question>(ConvertQuestionCount(QuestionCount));
            CurrentQuiz.QuizName = "";
            CurrentQuiz.QuizCategory = QuestionCategory.MISC.ToString();
        }

        public void BuildSaveQuiz()
        {
            CurrentQuiz.QuizName = QuizName;
            CurrentQuiz.CreationDate = DateTime.Now;
            CurrentQuiz.QuizCategory = SelectedCategory;
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
