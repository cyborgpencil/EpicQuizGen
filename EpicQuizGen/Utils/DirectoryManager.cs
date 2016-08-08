/// <summary>
/// User to help manage Directories and Files for Epic Quiz
/// </summary>
///

using System;
using System.IO;
namespace EpicQuizGen.Utils
{
    public class DirectoryManager
    {
        public string MainDirectory { get; set; }
        public string QuizDirectory { get; set; }
        public string QuestionDirectory { get; set; }
        public DirectoryManager( string rootName = "EpicQuizGen", string quizDirectory = "QuizDirectory", string questionDirectory = "QuestionDirctory")
        {
            MainDirectory = rootName;
            QuizDirectory = quizDirectory;
            QuestionDirectory = questionDirectory;

            // Create Root Directory
            Directory.CreateDirectory(MainDirectory);

            // Create Directory for Questions and Quizes
            Directory.CreateDirectory(MainDirectory + "\\" + QuizDirectory);
            Directory.CreateDirectory(MainDirectory + "\\" + QuestionDirectory);
        }
    }
}
