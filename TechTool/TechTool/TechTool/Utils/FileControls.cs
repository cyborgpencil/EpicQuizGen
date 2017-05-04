using System;
using System.IO;
using Microsoft.Win32;

namespace TechTool.Utils
{
    public class FileControls
    {
        public OpenFileDialog dialogHandler;
        public string FileName { get; set; }
        public string TextFileExt { get; set; }
        public string Filters { get; set; }
        public string EmainSigsFolder;

        public FileControls()
        {
            dialogHandler = new OpenFileDialog();
            EmainSigsFolder = "Default Sigs";
        }

        public void SetDefaultDialog()
        {
            dialogHandler.FileName = FileName;
            dialogHandler.Filter = Filters;
            dialogHandler.DefaultExt = TextFileExt;

            // create Email sigs folder
            CreateDefaultSigFolder();
        }

        public void CreateDefaultSigFolder()
        {
            if (!Directory.Exists(EmainSigsFolder))
            {
                Directory.CreateDirectory(EmainSigsFolder);
            }
        }
    }
}
