using EpicQuizGen.Events;
using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Events;
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
            set { SetProperty(ref _currentQuiz, value); }
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

        private ObservableCollection<string> _questionCategoriesSelect;
        public ObservableCollection<string> QuestionCategoriesSelect {
            get { return _questionCategoriesSelect; }
            set { SetProperty(ref _questionCategoriesSelect, value); }
        }

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

        private string _quizTime;
        public string QuizTime
        {
            get { return _quizTime; }
            set { SetProperty(ref _quizTime, value); }
        }
        #endregion
        private IEventAggregator _eventAggregator;

        public QuizzesShowViewModel()
        {
            // Commands
            NewQuizCommand = new DelegateCommand(NewQuiz);
            SaveQuizCommand = new DelegateCommand(SaveQuiz);
            EditCommand = new DelegateCommand(EditExecute, EditCanExecute).ObservesProperty(() => EditQuiz);
            DeleteCommand = new DelegateCommand(DeleteQuiz);
            TakeQuizCommand = new DelegateCommand(TakeQuiz);
            QuizzesShowLoadCommand = new DelegateCommand(QuizzesShowLoad);

            QuestionCategoriesSelect = new ObservableCollection<string>();
            SelectedCategory = "";
        }
        public QuizzesShowViewModel( IEventAggregator eventAggregator):this()
        {
            _eventAggregator = eventAggregator;
            BuildNewQuiz();

            QuizList = new ObservableCollection<Quiz>();
            QuizName = "";
            QuestionCount = "1";
            QuizTime = "30";

            // Get list of QuestionCategoriesSelect to a string
            QuestionCategoriesSelect = new ObservableCollection<string>();

            // Commands
            NewQuizCommand = new DelegateCommand(NewQuiz);
            SaveQuizCommand = new DelegateCommand(SaveQuiz);
            EditCommand = new DelegateCommand(EditExecute, EditCanExecute).ObservesProperty(() => EditQuiz);
            DeleteCommand = new DelegateCommand(DeleteQuiz);

            // Load Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
            TakeQuizCommand = new DelegateCommand(TakeQuiz);
            QuizzesShowLoadCommand = new DelegateCommand(QuizzesShowLoad);
        }

        #region Commands

        public DelegateCommand NewQuizCommand { get; set; }
        public void NewQuiz()
        {
            BuildNewQuiz();

            // Clear GUI
            ClearGUI();
        }

        public DelegateCommand QuizzesShowLoadCommand { get; set; }
        public void QuizzesShowLoad()
        {

            QuizList = new ObservableCollection<Quiz>();
            QuizName = "";
            QuestionCount = "1";
            QuizTime = "30";
            LoadCategories();

            // Get list of QuestionCategoriesSelect to a string
            LoadCategories();

            // Load Quizzes
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());

        }

        public DelegateCommand SaveQuizCommand { get; set; }
        public void SaveQuiz()
        {
            BuildNewQuiz();
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
                SelectedCategory = EditQuiz.QuizCategory;
                QuizTime = CurrentQuiz.QuizTime;
            }
        }
        public DelegateCommand DeleteCommand { get; set; }
        public void DeleteQuiz()
        {
            if (EditQuiz != null)
            {
                QuizIOManager.Instance.Quiz = EditQuiz;
                QuizIOManager.Instance.DeleteQuestionFromFile(EditQuiz.QuizName);

                // Clear GUI
                ClearGUI();
            }
            QuizList = new ObservableCollection<Quiz>(QuizIOManager.Instance.LoadQuizzesFromFile());
        }

        public DelegateCommand TakeQuizCommand { get; set; }
        public void TakeQuiz()
        {
            QuizIOManager.Instance.Quiz = EditQuiz;
            _eventAggregator.GetEvent<TakeQuizEvent>().Publish(CurrentQuiz);
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
            CurrentQuiz.QuizTime = "5";
        }

        public void BuildSaveQuiz()
        {
            CurrentQuiz.QuizName = QuizName;
            CurrentQuiz.CreationDate = DateTime.Now;
            CurrentQuiz.QuizCategory = SelectedCategory;
            CurrentQuiz.QuizTime = QuizTime;
        }

        private int ConvertQuestionCount(string count)
        {
            int result;
            int.TryParse(count, out result);
            return result;
        }

        void ClearGUI()
        {
            QuizName = "";
            QuestionCount = "";
            QuizTime = "";
            SelectedCategory = "";
        }

        void LoadCategories()
        {
            // Clear current Categories
            QuestionCategoriesSelect = new ObservableCollection<string>();
            if (CategoriesIOManager.Instance.GetCategoriesFromFile())
            {
                foreach (var name in CategoriesIOManager.Instance.LoadCategoriesFromFile())
                {
                    QuestionCategoriesSelect.Add(name.CategoryName);
                };
            }
            else
                QuestionCategoriesSelect.Add("No Current Categories");
        }
        #endregion
    }
}
