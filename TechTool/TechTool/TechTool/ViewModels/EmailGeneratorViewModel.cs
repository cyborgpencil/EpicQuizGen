using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            set { SetProperty(ref _firstName, value);
                SortByFirstName(_firstName);
            }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value);
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

            SelectedUser = new User();
        }

        public async void CallReturnADUsersAsync()
        {
            // Get AD users
            UserList = await userControl.ReturnADUsersAsync();
            UserListBind.Clear();
            List<string> unsortedUserList = new List<string>();
            char[] charArray = new char[UserList.Count];
            for (int i = 0; i < UserList.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(UserList[i].DisplayName))
                {
                    unsortedUserList.Add(UserList[i].DisplayName);
                }
            }
            unsortedUserList.Sort();
            UserListBind = new ObservableCollection<string>(unsortedUserList);
        }

        private void ShowSelectedUser()
        {
            if (SelectedUser != null)
            {
                FirstName = SelectedUser.FirstName;
                LastName = SelectedUser.LastName;
                UserName = SelectedUser.Username;
            }
        }

        private void SortByFirstName(string name)
        {
            // Check status of Users
            if(UserList != null && UserList.Count > 0 && !string.IsNullOrWhiteSpace(name))
            {
                // get current list
                ObservableCollection<string> templist = UserListBind;

                // clear bind list
                UserListBind.Clear();

                // get count for amount of chars typed
                int charCounts = name.Length;

                // compare index 
                for (int i = 0; i < templist.Count; i++)
                {
                    
                }
            }
            else
            {
                
            }
        }

        private string AlphabetString(string unordered)
        {
            char[] charS = unordered.ToCharArray();

            Array.Sort(charS);

            return new string(charS);

        }
    }
}
