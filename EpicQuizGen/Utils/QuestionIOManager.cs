/// <summary>
/// User to manager all IO functions of the Question Model
/// </summary>
/// 

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace EpicQuizGen.Utils
{
    public class QuestionIOManager
    {
        public XmlSerializer XmlSerializer { get; set; }
        public StreamWriter writer { get; set; }

        public object CurrentModel { get; set; }

        public QuestionIOManager(object model, string filename)
        {
            CurrentModel = model;
            XmlSerializer = new XmlSerializer(model.GetType());
            writer = new StreamWriter(filename);
        }

        public void SaveModel()
        {
            XmlSerializer.Serialize(writer, CurrentModel);
            writer.Close();
        }
    }
}
