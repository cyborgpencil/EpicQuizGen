using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    public class QuestionCategoryViewModel :BindableBase
    {
        #region Properties
        private ObservableCollection<string> _currentList;
        public ObservableCollection<string> CurrentList
        {
            get { return _currentList; }
            set { SetProperty(ref _currentList, value); }
        }
        public QuestionCategories CurrentCategory { get; set; }

        private string _currentCategoryName;
        public string CurrentCategoryName
        {
            get { return _currentCategoryName; }
            set { SetProperty(ref _currentCategoryName, value); }
        }
        #endregion
        public QuestionCategoryViewModel()
        {
            CurrentList = new ObservableCollection<string>();
            QuestionCategoryLoadCommand = new DelegateCommand(QuestionCategoryLoad);

            NewCategoryCommand = new DelegateCommand(NewCategory);
            SaveCategoryCommand = new DelegateCommand(SaveCategory, CanSaveCategegory).ObservesProperty(()=> CurrentCategoryName);
            EditCategoryCommand = new DelegateCommand(EditCategory);
            DeleteCategoryCommand = new DelegateCommand(DeleteCategory);

        }

        #region Commands
        public DelegateCommand QuestionCategoryLoadCommand { get; set; }
        public void QuestionCategoryLoad()
        {
            CurrentList = new ObservableCollection<string>(CategoriesIOManager.Instance.LoadCategories());

            if (CurrentList.Count == 0)
            {
                CurrentList.Add("No Categories, Please add a Category");
                Debug.WriteLine("Testing for Current Gategories");
            }
        }

        public DelegateCommand NewCategoryCommand { get; set; }
        public void NewCategory()
        {
            // Clears our Category
            CurrentCategory = new QuestionCategories();
            CurrentCategory.CategoryName = "";
        }
        public DelegateCommand SaveCategoryCommand { get; set; }
        public void SaveCategory()
        {
            //
        }
        public bool CanSaveCategegory()
        {
            if (!string.IsNullOrWhiteSpace(CurrentCategoryName))
                return true;
            else
                return false;
        }

        public DelegateCommand EditCategoryCommand { get; set; }
        public void EditCategory()
        {

        }
        public DelegateCommand DeleteCategoryCommand { get; set; }
        public void DeleteCategory()
        {

        }
        #endregion
    }
}
