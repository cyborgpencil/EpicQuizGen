using EpicQuizGen.Models;
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
            set { SetProperty(ref _currentQuiz, value); }
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
            Debug.WriteLine("Saveing Quiz");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Utility to build a Quiz
        /// </summary>
        public void BuildQuiz()
        {
            
        }
        #endregion
    }
}
