using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TechTool.Converters;
using TechTool.Models;
using TechTool.Utils;

namespace TechTool.ViewModels
{
    public class EmailGeneratorViewModel : BindableBase
    {
        private UsersControl userControl;
        public User Users { get; set; }
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set {
                SetProperty(ref _firstName, value);
                CheckAllSearchCriteria();
               
            }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value);
                CheckAllSearchCriteria();

            }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value);
                CheckAllSearchCriteria();
            }
        }
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set {
                SetProperty(ref _selectedUser, value);
                SetSelectedUser();
            }
        }
        private ObservableCollection<User> _userList;
        public ObservableCollection<User> UserList
        {
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }
        private ObservableCollection<string> _userListBind;
        public ObservableCollection<string> UserListBind
        {
            get { return _userListBind; }
            set { SetProperty(ref _userListBind, value); }
        }
        private ObservableCollection<string> _sortedUsers;
        public ObservableCollection<string> SortedUsers
        {
            get { return _sortedUsers; }
            set { SetProperty(ref _sortedUsers, value); }
        }
        List<string> unsortedUserList = new List<string>();
        private FileControls _fileControls;
        private DelegateCommand _loadDefaultSigCommand;
        public DelegateCommand LoadDefaultSigCommand
        {
            get { return _loadDefaultSigCommand; }
            set { SetProperty(ref _loadDefaultSigCommand, value); }
        }
        private string _sigTextBind;
        public string SigTextBind
        {
            get { return _sigTextBind; }
            set { SetProperty(ref _sigTextBind, value); }
        }

        public EmailGeneratorViewModel()
        {
            // List from AD
            UserList = new ObservableCollection<User>(new List<User>());

            // Binding List
            UserListBind = new ObservableCollection<string>(new List<string>());

            // Sorted List
            SortedUsers = new ObservableCollection<string>(new List<string>());

            userControl = new UsersControl();

            CallReturnADUsersAsync();

            // check for empty UserList
            if (UserList.Count == 0)
            {
                UserListBind.Add("getting AD users...");
            }

            // File stuff
            _fileControls = new FileControls();
            LoadDefaultSigCommand = new DelegateCommand(LoadDefaultSig);
        }

        private void LoadDefaultSig()
        {
            _fileControls.FileName = "DefaultFileSig";
            _fileControls.TextFileExt = ".txt";
            _fileControls.Filters = "Text documents (.txt)|*.txt";

            _fileControls.SetDefaultDialog();

            bool? result = _fileControls.dialogHandler.ShowDialog();

            if (result == true)
            {
                // save file to App Sig Folder
                if (File.Exists($"{_fileControls.EmainSigsFolder}/{_fileControls.dialogHandler.FileName}"))
                {
                    File.Copy(_fileControls.dialogHandler.FileName, $"{_fileControls.EmainSigsFolder}/{_fileControls.dialogHandler.FileName}");
                }
                // Set text of file to display for editing
                SigTextBind =  File.ReadAllText($"{ _fileControls.EmainSigsFolder}/{ _fileControls.dialogHandler.SafeFileName}");
            }
        }

        public async void CallReturnADUsersAsync()
        {   
            // Get AD users
            UserList = await userControl.ReturnADUsersAsync();
            UserListBind.Clear();
            
            char[] charArray = new char[UserList.Count];
            for (int i = 0; i < UserList.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(UserList[i].DisplayName))
                {
                    unsortedUserList.Add($"{UserList[i].DisplayName},   {UserList[i].Username}");
                }
            }
            unsortedUserList.Sort();
            UserListBind = new ObservableCollection<string>(unsortedUserList);
        }

        //Compare string with strings in a list, then display based on a matching display
        private List<string> SortByInput(string compare, List<string> FullList, List<string> displayByList)
        {
            var matchedList = new List<string>();

            // get current list
            for (int i = 0; i < FullList.Count; i++)
            {
                // Check status of inputs
                if (FullList[i] != null && FullList.Count > 0 && !string.IsNullOrWhiteSpace(compare) && displayByList[i] != null)
                {
                    // compare a string against each item in the string then ser machted List.
                    if (FullList[i].StartsWith(compare,true, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        matchedList.Add(displayByList[i]);
                    }
                }
            }

            return matchedList;
        }

        private void CheckAllSearchCriteria()
        {
            // List to display
            List<string> displayByList = new List<string>();

            // check for empty inputs 
            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                //Working first name List<string> Items in userList.FirstName
                List<string> workingFirstNameList = new List<string>();

                for (int i = 0; i < UserList.Count; i++)
                {
                    workingFirstNameList.Add(UserList[i].FirstName);
                    displayByList.Add($"{UserList[i].DisplayName},  {UserList[i].Username}");
                }
                UserListBind = new ObservableCollection<string>(SortByInput(FirstName, workingFirstNameList,displayByList ));
            }
            // check for empty inputs 
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                //Working first name List<string> Items in userList.FirstName
                List<string> workingLastNameList = new List<string>();
                for (int i = 0; i < UserList.Count; i++)
                {
                    workingLastNameList.Add(UserList[i].LastName);
                }
                UserListBind = new ObservableCollection<string>(SortByInput(LastName, workingLastNameList, displayByList));
            }
            // check for empty inputs 
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                //Working first name List<string> Items in userList.FirstName
                List<string> workingUsernameList = new List<string>();
                for (int i = 0; i < UserList.Count; i++)
                {
                    workingUsernameList.Add(UserList[i].Username);
                }
                UserListBind = new ObservableCollection<string>(SortByInput(UserName, workingUsernameList, displayByList));
            }

        }

        private void SetSelectedUser()
        {

            if (SelectedUser != null )
            {
                if (!string.IsNullOrWhiteSpace(SigTextBind))
                {
                    SigTextBind = SigTextBind.Replace("(name)", SelectedUser.FirstName);
                }
            }
        }

        private void SortByInput()
        {

        }

        private string AlphabetString(string unordered)
        {
            char[] charS = unordered.ToCharArray();

            Array.Sort(charS);

            return new string(charS);

        }
    }
}
