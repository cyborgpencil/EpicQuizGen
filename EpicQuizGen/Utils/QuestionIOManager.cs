/// <summary>
/// User to manager all IO functions of the Question Model
/// </summary>
/// 

using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using EpicQuizGen.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace EpicQuizGen.Utils
{
    public sealed class QuestionIOManager
    {
        public XmlSerializer XmlSerializer { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public FileStream FStream { get; set; }

        public Question QuestionModel { get; set; }

        public List<Question> QuestionsFromFile { get; set; }

        static readonly QuestionIOManager _instance = new QuestionIOManager();
        public static QuestionIOManager Instance
        {
            get { return _instance; }
        }
        QuestionIOManager()
        {
            
        }

        public void SaveQuestionModel()
        {
            if (QuestionModel != null)
            {
                XmlSerializer = new XmlSerializer(typeof(Question));
                StreamWriter = new StreamWriter(DirectoryManager.Instance.MainDirectory + "\\" + DirectoryManager.Instance.QuestionDirectory + "\\" + QuestionModel.QuestionName + ".xml");
                XmlSerializer.Serialize(StreamWriter, QuestionModel);
                StreamWriter.Close();
            }
        }

        public void DeleteQuestionFromFile(string questionName)
        {
            QuestionsFromFile = GetQuestionsFromFile();

            var QuestionToDelete = from question in GetQuestionsFromFile()
                                   where question.QuestionName.Contains(questionName)
                                   select questionName;

            File.Delete(DirectoryManager.Instance.QuestionDirectoryPath + "\\" + QuestionToDelete.FirstOrDefault() + ".xml");

        }

        public List<Question> GetQuestionsFromFile()
        {

            IEnumerable<string> questionNames = Directory.EnumerateFileSystemEntries(DirectoryManager.Instance.QuestionDirectoryPath, "*.xml");

            if(questionNames.GetEnumerator().Current != null)
            {

                QuestionsFromFile = new List<Question>();

                // clear out xml object using uknown node and attr events
                XmlSerializer = new XmlSerializer(typeof(Question));
                XmlSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                XmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                foreach (var item in questionNames)
                {
                    FStream = new FileStream(item, FileMode.Open);
                    var newQuestion = (Question)XmlSerializer.Deserialize(FStream);
                    QuestionsFromFile.Add(newQuestion);
                    FStream.Close();
                }

                FStream.Close();

                return QuestionsFromFile;
            }
            return new List<Question>();
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
