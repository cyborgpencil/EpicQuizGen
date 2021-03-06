﻿using EpicQuizGen.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class MultiChoice4QuizViewModel : QuizViewModelbase
    {
        #region Properties
        private bool _multiChoiceAnswer1;
        public bool MultiChoiceAnswer1
        {
            get { return _multiChoiceAnswer1; }
            set { SetProperty(ref _multiChoiceAnswer1, value); SetAnswered(); }
        }
        private bool _multiChoiceAnswer2;
        public bool MultiChoiceAnswer2
        {
            get { return _multiChoiceAnswer2; }
            set { SetProperty(ref _multiChoiceAnswer2, value); SetAnswered(); }
        }
        private bool _multiChoiceAnswer3;
        public bool MultiChoiceAnswer3
        {
            get { return _multiChoiceAnswer3; }
            set { SetProperty(ref _multiChoiceAnswer3, value); SetAnswered(); }
        }
        private bool _multiChoiceAnswer4;
        public bool MultiChoiceAnswer4
        {
            get { return _multiChoiceAnswer4; }
            set { SetProperty(ref _multiChoiceAnswer4, value); SetAnswered(); }
        }
        private string _multiChoiceAnswerQuestion1;
        public string MultiChoiceAnswerQuestion1
        {
            get { return _multiChoiceAnswerQuestion1; }
            set { SetProperty(ref _multiChoiceAnswerQuestion1, value); }
        }
        private string _multiChoiceAnswerQuestion2;
        public string MultiChoiceAnswerQuestion2
        {
            get { return _multiChoiceAnswerQuestion2; }
            set { SetProperty(ref _multiChoiceAnswerQuestion2, value); }
        }
        private string _multiChoiceAnswerQuestion3;
        public string MultiChoiceAnswerQuestion3
        {
            get { return _multiChoiceAnswerQuestion3; }
            set { SetProperty(ref _multiChoiceAnswerQuestion3, value); }
        }
        private string _multiChoiceAnswerQuestion4;
        public string MultiChoiceAnswerQuestion4
        {
            get { return _multiChoiceAnswerQuestion4; }
            set { SetProperty(ref _multiChoiceAnswerQuestion4, value); }
        }
        private bool _answered;
        public bool Answered
        {
            get { return _answered; }
            set { SetProperty(ref _answered, value); }
        }

        #endregion

        #region Contructors
        public MultiChoice4QuizViewModel()
        {
            LoadMulti4QuizViewCommand = new DelegateCommand(LoadMulti4QuizView);
        }
        #endregion

        #region Commands
        public DelegateCommand LoadMulti4QuizViewCommand { get; set; }
        public void LoadMulti4QuizView()
        {

        }
        #endregion

        #region Events

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion

        #region Methods
        private void SetAnswered()
        {
            // Check if any Answers are flase
            if (MultiChoiceAnswer1 == false && MultiChoiceAnswer2 == false && MultiChoiceAnswer3 == false && MultiChoiceAnswer4 == false)
            {
                Answered = false;
            }
            else
                Answered = true;
        }
        private bool CheckAnswer1(Question checkanswer)
        {
            #region 1-4 True
            // answer 1 = true
            if(MultiChoiceAnswer1 == checkanswer.MultiAnswerPositions[0])
            {
                // answer 2 = true
               if(MultiChoiceAnswer2 == checkanswer.MultiAnswerPositions[1])
                {
                    // answer 3 = true
                    if (MultiChoiceAnswer3 == checkanswer.MultiAnswerPositions[2])
                    {
                        // answer 4 = true
                        if (MultiChoiceAnswer4 == checkanswer.MultiAnswerPositions[3])
                        {
                            return true;
                        }
                    }
                }
            }
            #endregion

            #region 1-3 True
            // answer 1 = true
            if (MultiChoiceAnswer1 == checkanswer.MultiAnswerPositions[0])
            {
                // answer 2 = true
                if (MultiChoiceAnswer2 == checkanswer.MultiAnswerPositions[1])
                {
                    // answer 3 = true
                    if (MultiChoiceAnswer3 == checkanswer.MultiAnswerPositions[2])
                    {
                        // answer 4 = true
                        if (MultiChoiceAnswer4 == checkanswer.MultiAnswerPositions[3])
                        {
                            return true;
                        }
                    }
                }
            }
            #endregion

            return false;
        }
        private void CheckAnswer2()
        {

        }
        private void CheckAnswer3()
        {

        }
        private void CheckAnswer4()
        {

        }
        #endregion

    }
}
