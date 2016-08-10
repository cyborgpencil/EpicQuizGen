/// <summary>
/// User to manager all IO functions of the Question Model
/// </summary>
/// 

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using EpicQuizGen.Models;

namespace EpicQuizGen.Utils
{
    public sealed class QuestionIOManager
    {
        public XmlSerializer XmlSerializer { get; set; }
        public StreamWriter StreamWriter { get; set; }

        public Question QuestionModel { get; set; }

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
    }
}
