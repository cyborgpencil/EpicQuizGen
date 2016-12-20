using EpicQuizGen.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace EpicQuizGen.Utils
{
    public class CategoriesIOManager
    {
        
        private XmlSerializer XmlSerializer { get; set; }
        private StreamWriter StreamWriter { get; set; }
        private FileStream FStream { get; set; }
        private List<QuestionCategory> CategoriesFromFile { get; set; }

        static readonly CategoriesIOManager _instance = new CategoriesIOManager();
        public static CategoriesIOManager Instance
        {
            get { return _instance; }
        }

        #region Methods
        public void LoadCategories()
        {
            // Check if Folder is empty
        }
        
        #endregion
    }
}
