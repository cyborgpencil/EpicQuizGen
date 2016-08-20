using EpicQuizGen.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EpicQuizGen.Utils
{
    public sealed class QuizIOManager
    {
        private XmlSerializer XmlSerializer { get; set; }
        private StreamWriter StreamWriter { get; set; }
        private FileStream FStream { get; set; }
        public Quiz Quiz { get; set; }

        private List<Quiz> QuizzesFromFile { get; set; }

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

        public List<Quiz> LoadQuizzesFromFile()
        {
            IEnumerable<string> quizNames = Directory.EnumerateFileSystemEntries(DirectoryManager.Instance.QuizDirectoryPath, "*.xml");

            if (quizNames.GetEnumerator().Current != null)
            {

                QuizzesFromFile = new List<Quiz>();

                // clear out xml object using uknown node and attr events
                XmlSerializer = new XmlSerializer(typeof(Quiz));
                XmlSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                XmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                foreach (var item in quizNames)
                {
                    FStream = new FileStream(item, FileMode.Open);
                    var newQuiz = (Quiz)XmlSerializer.Deserialize(FStream);
                    QuizzesFromFile.Add(newQuiz);
                    FStream.Close();
                }

                FStream.Close();

                return QuizzesFromFile;
            }
            return new List<Quiz>();
        }

        public void DeleteQuestionFromFile(string quizname)
        {
            QuizzesFromFile = LoadQuizzesFromFile();

            var QuizToDelete = from question in LoadQuizzesFromFile()
                                   where Quiz.QuizName.Contains(quizname)
                                   select quizname;

            File.Delete(DirectoryManager.Instance.QuizDirectoryPath + "\\" + QuizToDelete.FirstOrDefault() + ".xml");

        }

        void serializer_UnknownNode
           (object sender, XmlNodeEventArgs e)
        {
            Debug.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        void serializer_UnknownAttribute
       (object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute attr = e.Attr;
            Debug.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
