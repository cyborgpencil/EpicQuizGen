
using System.Collections.Generic;
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
        public string QuestionDirectoryPath { get; set; }
        public string QuizDirectoryPath { get; set; }
        public string CategoryDirectory { get; set; }
        public string CategoryDirectoryPath { get; set; }
        public List<string> DirectoryList { get; set; }

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
            DirectoryList = new List<string>();

            MainDirectory = "EpicQuizGen";
            QuizDirectory = "QuizDirectory";
            QuestionDirectory = "QuestionDirectory";
            CategoryDirectory = "QuestionCategories";

            QuestionDirectoryPath = $"{MainDirectory}\\{QuestionDirectory}";
            DirectoryList.Add(QuestionDirectoryPath);

            QuizDirectoryPath = $"{MainDirectory}\\{QuizDirectory}";
            DirectoryList.Add(QuizDirectoryPath);

            CategoryDirectoryPath = $"{MainDirectory}\\{CategoryDirectory}";
            DirectoryList.Add(CategoryDirectoryPath);
        }

        public void CreateDirectory()
        {
            CheckDirectories();

            // Create Root Directory
            Directory.CreateDirectory(MainDirectory);

            // Create Directory for Questions and Quizes
            Directory.CreateDirectory(MainDirectory + "\\" + QuizDirectory);
            Directory.CreateDirectory(MainDirectory + "\\" + QuestionDirectory);
        }

        // Always check if folder is availible, if not create
        public void CheckDirectories()
        {
            foreach (string dir in DirectoryList)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }
    }
}
