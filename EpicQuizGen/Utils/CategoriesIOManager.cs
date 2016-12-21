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
        private List<QuestionCategories> CategoriesFromFile { get; set; }

        static readonly CategoriesIOManager _instance = new CategoriesIOManager();
        public static CategoriesIOManager Instance
        {
            get { return _instance; }
        }
        CategoriesIOManager()
        {
            CategoriesFromFile = new List<QuestionCategories>();
        }

        #region Methods
        public List<string> LoadCategories()
        {
            IEnumerable<string> categoryNames = Directory.EnumerateFileSystemEntries(DirectoryManager.Instance.CategoryDirectoryPath, "*.xml");

            return new List<string>(categoryNames);
        }
        #endregion
    }
}
