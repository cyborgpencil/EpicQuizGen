using Prism.Mvvm;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace TechTool.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private bool _toTray;
        public bool ToTray
        {
            get { return _toTray; }
            set { SetProperty(ref _toTray, value); }
        }
        public MainWindowViewModel()
        {
        }
    }
}
