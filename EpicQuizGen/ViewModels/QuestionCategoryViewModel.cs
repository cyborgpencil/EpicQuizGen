using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class QuestionCategoryViewModel :BindableBase
    {
        private ObservableCollection<string> _currentList;

        public ObservableCollection<string> CurrentList
        {
            get { return this._currentList; }
            set { SetProperty(ref _currentList, value); }
        }

        public QuestionCategoryViewModel()
        {
            CurrentList = new ObservableCollection<string>(new List<string>(){ "test1", "test2"});
        }
        

    }
}
