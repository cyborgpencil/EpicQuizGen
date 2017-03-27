using Prism.Mvvm;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Diagnostics;
using System.Windows;
using TechTool.Views;
using Prism.Events;
using Prism.Commands;
using TechTool.Models;

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

        private Window _mainWindow;
        public Window MainWindow
        {
            get { return _mainWindow; }
            set { SetProperty(ref _mainWindow, value); }
        }

        private IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set { SetProperty(ref _eventAggregator, value); }
        }
        private EventAggregator _click;
        public EventAggregator Click
        {
            get { return _click; }
            set { SetProperty(ref _click, value); }
        }

        private DelegateCommand _minimizeCommand;
        public DelegateCommand MinimizeCommand
        {
            get { return _minimizeCommand; }
            set { SetProperty(ref _minimizeCommand, value); }
        }

        private WindowState _windowsStateBind;
        public WindowState WindowsStateBind
        {
            get { return _windowsStateBind; }
            set { SetProperty(ref _windowsStateBind, value); }
        }

        private bool _showInTaskBarBind;
        public bool ShowInTaskBarBind
        {
            get { return _showInTaskBarBind; }
            set { SetProperty(ref _showInTaskBarBind, value); }
        }
        private TaskbarItem _tbi;
        public TaskbarItem TBI
        {
            get { return _tbi; }
            set { SetProperty(ref _tbi, value); }
        }

        public MainWindowViewModel()
        {
            TBI = new TaskbarItem();
            TBI.NotifyIcon.Icon = new Icon(@"E:\Apps\Repos\TechTool\TechTool\TechTool\Resources\techtool.ico");
            TBI.NotifyIcon.Visible = true;
            ShowInTaskBarBind = true;

        }

        public void Minimize()
        {
            Debug.WriteLine("Testing!");
        }
        
    }
}
