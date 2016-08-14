using EpicQuizGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.Utils
{
    public sealed class QuizIOManager
    {
        public Quiz Quiz { get; set; }

        static readonly QuizIOManager _instance = new QuizIOManager();
        public static QuizIOManager Instance
        {
            get { return _instance; }
        }
    }
}
