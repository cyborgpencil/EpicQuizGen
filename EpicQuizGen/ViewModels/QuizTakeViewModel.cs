﻿using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class QuizTakeViewModel : BindableBase
    {
        #region Properties
        private string _timeLeftString  ;
        public string TimeLeftString
        {
            get { return _timeLeftString; }
            set { value = $"Time Left: {Timer}"; SetProperty(ref _timeLeftString, value); }
        }
        private int _timer;
        public int Timer
        {
            get { return _timer; }
            set { SetProperty(ref _timer, value); }
        }
        private Quiz _quiz;
        public Quiz Quiz
        {
            get { return _quiz; }
            set { SetProperty(ref _quiz, value); }
        }
        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { SetProperty(ref _currentQuestion, value); }
        }
        private string _currentQuestionType;
        public string CurrentQuestionType
        {
            get { return _currentQuestionType; }
            set { SetProperty(ref _currentQuestionType, value); }
        }

        private QuizTimeManager _quizTimer;
        public QuizTimeManager QuizTimer
        {
            get { return _quizTimer; }
            set { SetProperty(ref _quizTimer, value); }
        }
        private IRegionManager _regionManager { get; set; }
        #endregion

        #region Constructor
        public QuizTakeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Quiz = QuizIOManager.Instance.Quiz;

            TimeLeftString = "";
            CurrentQuestion = Quiz.Questions[0];
            CurrentQuestionType = CurrentQuestion.QuestionType;
            NavigateCommand = new DelegateCommand<string>(Navigate);

            
        }
        #endregion

        #region Commands
        public DelegateCommand<string> NavigateCommand { get; set; }
        public void Navigate(string uri)
        {
            // Timer
            QuizTimer = new QuizTimeManager(this);
            QuizTimer.StartTimer();
            Timer = QuizTimer.SendCurrentSecound();
            _regionManager.RequestNavigate("CurrentQuestionType", uri);
        }
        #endregion

        #region Events
        internal void QuizTimerEvent(object sender, EventArgs e)
        {
            Timer = QuizTimer.SendCurrentSecound();
        }
        #endregion

        #region Methods
        #endregion
    }
}
