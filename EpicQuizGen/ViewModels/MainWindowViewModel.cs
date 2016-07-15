using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private BindableBase _currentView;
        public BindableBase CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }

        public MainWindowViewModel()
        {
            CurrentView = new QuizQuestionSelectViewModel();
        }
    }
}
