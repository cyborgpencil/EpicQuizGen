
using System;
using System.Collections.Generic;

namespace EpicQuizGen.Models
{
    public class Question
    {
        public string QuestionName { get; set; }
        public string MainQuestion { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public DateTime CreationDate { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
        public bool TrueFalseAnswer { get; set; }
        public List<string> MultiAnswerList { get; set; }
        public List<bool> MultiAnswerPositions { get; set; }
    }
}
