using System;
using System.Collections;
using System.Collections.Generic;

namespace EpicQuizGen.Models
{
    public class Quiz
    {
        public string QuizName { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Question> Questions { get; set; }
        public float Grade { get; set; }
    }
}
