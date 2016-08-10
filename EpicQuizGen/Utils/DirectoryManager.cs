/// <summary>
/// User to help manage Directories and Files for Epic Quiz
/// </summary>
///

using System.IO;
namespace EpicQuizGen.Utils
{
    public sealed class DirectoryManager
    {
        public string MainDirectory { get; set; }
        public string QuizDirectory { get; set; }
        public string QuestionDirectory { get; set; }

        static readonly DirectoryManager _instance = new DirectoryManager();
        public static DirectoryManager Instance
        {
            get
            {
                return _instance;
            }
        }

         DirectoryManager()
        {
            MainDirectory = "EpicQuizGen";
            QuizDirectory = "QuizDirectory";
            QuestionDirectory = "QuestionDirectory";
        }

        public void CreateDirectory()
        {
            // Create Root Directory
            Directory.CreateDirectory(MainDirectory);

            // Create Directory for Questions and Quizes
            Directory.CreateDirectory(MainDirectory + "\\" + QuizDirectory);
            Directory.CreateDirectory(MainDirectory + "\\" + QuestionDirectory);
        }
    }
}
