
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace EpicQuizGen.Models
{
    public class Question : BindableBase, IDataErrorInfo
    {
        private string _questionName;
        public string QuestionName
        {
            get { return _questionName; }
            set{ SetProperty(ref _questionName, value); }
        }
        private string _mainQuestion;
        public string MainQuestion
        {
            get { return _mainQuestion; }
            set { SetProperty(ref _mainQuestion, value); }
        }
        private string _questionType;
        public string QuestionType
        {
            get { return _questionType; }
            set { SetProperty(ref _questionType, value); }
        }
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }
        private string _questionCategory;
        public string QuestionCategory
        {
            get { return _questionCategory; }
            set { SetProperty(ref _questionCategory, value); }
        }
        private List<string> _multiAnswerList;
        public List<string> MultiAnswerList
        {
            get { return _multiAnswerList; }
            set { SetProperty(ref _multiAnswerList, value); }
        }
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); }
        }
        private bool _falseAnswer;
        public bool FalseAnswer
        {
            get { return _falseAnswer; }
            set { SetProperty(ref _falseAnswer, value); }
        }
        private List<bool> _multiAnswerPositions;
        public List<bool> MultiAnswerPositions
        {
            get { return _multiAnswerPositions; }
            set { SetProperty(ref _multiAnswerPositions, value); }
        }

        #region Validation
        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }
        readonly string[] ValidateProperties =
        {
            "QuestionName"
        };

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return GetPropertyValidationErrror(propertyName);
            }
        }

        public bool IsValid()
        {
            foreach (string property in ValidateProperties)
            {
                if (GetPropertyValidationErrror(property) != null)
                    return false;
            }

            return true;
        }

        string GetPropertyValidationErrror(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case "QuestionName":
                    error = ValidateQuestionName();
                    break;
            }
            return error;
        }

        private string ValidateQuestionName()
        {
            if (string.IsNullOrWhiteSpace(QuestionName))
            {
                return "QuestionName Cannot be Blank";
            }
            return null;
        }
        #endregion
    }
}
