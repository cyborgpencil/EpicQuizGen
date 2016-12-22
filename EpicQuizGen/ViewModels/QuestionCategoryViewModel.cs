using EpicQuizGen.Models;
using EpicQuizGen.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
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
        private ObservableCollection<QuestionCategories> _currentList;
        public ObservableCollection<QuestionCategories> CurrentList
        {
            get { return _currentList; }
            set { SetProperty(ref _currentList, value); }
        }
        private QuestionCategories _currentCategory;
        public QuestionCategories CurrentCategory
        {
            get { return _currentCategory; }
            set { SetProperty(ref _currentCategory, value);
                CurrentCategoryName = CurrentCategory.CategoryName; }
        }

        private string _currentCategoryName;
        public string CurrentCategoryName
        {
            get { return _currentCategoryName; }
            set { SetProperty(ref _currentCategoryName, value);}
        }
        #endregion
        public QuestionCategoryViewModel()
        {
            CurrentList = new ObservableCollection<QuestionCategories>();
            QuestionCategoryLoadCommand = new DelegateCommand(QuestionCategoryLoad);

            NewCategoryCommand = new DelegateCommand(NewCategory);
            SaveCategoryCommand = new DelegateCommand(SaveCategory, CanSaveCategegory).ObservesProperty(()=> CurrentCategoryName);
            EditCategoryCommand = new DelegateCommand(EditCategory);
            DeleteCategoryCommand = new DelegateCommand(DeleteCategory, CanExecuteDelete).ObservesProperty(()=> CurrentCategory).ObservesProperty(()=>CurrentCategory.CategoryName);

            SetCategoryModel();
        }

        #region Commands
        public DelegateCommand QuestionCategoryLoadCommand { get; set; }
        public void QuestionCategoryLoad()
        {
            SetCategoryModel();
            CurrentList = new ObservableCollection<QuestionCategories>(CategoriesIOManager.Instance.LoadCategoriesFromFile());

            if (CurrentList.Count == 0)
            {
                CurrentList.Add(new QuestionCategories { CategoryName = "No Categories, Please add a Category" });
                Debug.WriteLine("Testing for Current Gategories");
            }
        }

        public DelegateCommand NewCategoryCommand { get; set; }
        public void NewCategory()
        {
            // Clears our Category
            SetCategoryModel();
        }
        public DelegateCommand SaveCategoryCommand { get; set; }
        public void SaveCategory()
        {
            CurrentCategory.CategoryName = CurrentCategoryName;
            CategoriesIOManager.Instance.CategoryModels = CurrentCategory;
            CategoriesIOManager.Instance.SaveCategoryModel();
            SetCategoryModel();
            QuestionCategoryLoad();
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
            if(CurrentCategory != null && string.IsNullOrWhiteSpace(CurrentCategoryName))
            {

            }
        }
        public DelegateCommand DeleteCategoryCommand { get; set; }
        public void DeleteCategory()
        {
            CategoriesIOManager.Instance.DeleteCategoriesFromFile(CurrentCategory.CategoryName);
            SetCategoryModel();
            QuestionCategoryLoad();
        }

        public bool CanExecuteDelete()
        {
            if (CurrentCategory != null && !string.IsNullOrWhiteSpace(CurrentCategory.CategoryName))
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Methods
        private void SetCategoryModel()
        {
            CurrentCategory = new QuestionCategories();
            CurrentCategory.CategoryName = "";
        }
        #endregion
    }
}
