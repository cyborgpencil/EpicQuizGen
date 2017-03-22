using EpicQuizGen.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace EpicQuizGen.Utils
{
    public class CategoriesIOManager
    {
        
        private XmlSerializer XmlSerializer { get; set; }
        private StreamWriter StreamWriter { get; set; }
        private FileStream FStream { get; set; }
        private List<QuestionCategories> CategoriesFromFile { get; set; }
        public QuestionCategories CategoryModels { get; set; }

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
        public bool GetCategoriesFromFile()
        {
            IEnumerable<string> categoryNames = Directory.EnumerateFileSystemEntries(DirectoryManager.Instance.CategoryDirectoryPath, "*.xml");

            if (categoryNames.GetEnumerator().Current != null)
            {

                CategoriesFromFile = new List<QuestionCategories>();

                // clear out xml object using uknown node and attr events
                XmlSerializer = new XmlSerializer(typeof(QuestionCategories));
                XmlSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                XmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                foreach (var item in categoryNames)
                {
                    FStream = new FileStream(item, FileMode.Open);
                    var newCategory = (QuestionCategories)XmlSerializer.Deserialize(FStream);
                    CategoriesFromFile.Add(newCategory);
                    FStream.Close();
                }

                FStream.Close();
                return true;
            }
            return false;
        }

        public List<QuestionCategories> LoadCategoriesFromFile()
        {

            if (GetCategoriesFromFile())
            {
                return CategoriesFromFile;
            }
            return new List<QuestionCategories>();
        }


        public void SaveCategoryModel()
        {
            
            // varify the model is ok
            if(CategoryModels != null && !string.IsNullOrWhiteSpace(CategoryModels.CategoryName))
            {
                // save
                XmlSerializer = new XmlSerializer(typeof(QuestionCategories));
                StreamWriter = new StreamWriter(DirectoryManager.Instance.CategoryDirectoryPath + "\\" + CategoryModels.CategoryName + ".xml");
                XmlSerializer.Serialize(StreamWriter, CategoryModels);
                StreamWriter.Close();
                CategoryModels = null;
            }
        }
        public void DeleteCategoriesFromFile(string categoryName)
        {
            // fresh copy of categories
            CategoriesFromFile = LoadCategoriesFromFile();

            var CategoryToDelete = from category in LoadCategoriesFromFile()
                                   where category.CategoryName.Contains(categoryName)
                                   select categoryName;

            File.Delete($"{DirectoryManager.Instance.CategoryDirectoryPath}\\{CategoryToDelete.FirstOrDefault()}.xml");

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

        #endregion
    }
}
