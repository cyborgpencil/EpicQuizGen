using EpicQuizGen.Models;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EpicQuizGen.Models
{
    public class Quiz : BindableBase
    {
        private string _quizName;
        public string QuizName
        {
            get { return _quizName; }
            set { SetProperty(ref _quizName, value); }
        }
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }
        private List<Question> _questions;
        public List<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }
        private float _grade;
        public float Grade
        {
            get { return _grade; }
            set { SetProperty(ref _grade, value); }
        }
        private string _quizCategory;
        public string QuizCategory
        {
            get { return _quizCategory; }
            set { SetProperty(ref _quizCategory, value); }
        }
    }
}
