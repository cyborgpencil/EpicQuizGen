
using System;

namespace EpicQuizGen.Models
{
    public class Question
    {
        public string QuestionName { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public DateTime CreationDate { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
    }
}
