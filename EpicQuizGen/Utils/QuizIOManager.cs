using EpicQuizGen.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EpicQuizGen.Utils
{
    public sealed class QuizIOManager
    {
        public XmlSerializer XmlSerializer { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public FileStream FStream { get; set; }
        public Quiz Quiz { get; set; }

        static readonly QuizIOManager _instance = new QuizIOManager();
        public static QuizIOManager Instance
        {
            get { return _instance; }
        }
        public QuizIOManager()
        {

        }
        public void SaveQuiz()
        {
            if(Quiz != null)
            {
                if(!string.IsNullOrWhiteSpace(Quiz.QuizName))
                {
                    XmlSerializer = new XmlSerializer(typeof(Quiz));
                    StreamWriter = new StreamWriter(DirectoryManager.Instance.QuizDirectoryPath + "\\" + Quiz.QuizName + ".xml");
                    XmlSerializer.Serialize(StreamWriter, Quiz);
                    StreamWriter.Close();
                }
            }

        }
    }
}
