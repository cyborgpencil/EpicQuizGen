using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        List<string> unsortedUserList = new List<string>();

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

        private List<string> SortByFirstName(string name, List<string> currentList, List<string> sortingList)
        {
            // Check status of Users
            if(UserList != null && UserList.Count > 0 && !string.IsNullOrWhiteSpace(name))
            {
                UserListBind.Clear();
                // get current list
                for (int i = 0; i < UserList.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(currentList[i]))
                    {
                       if(currentList[i].StartsWith(name,true, System.Globalization.CultureInfo.CurrentCulture))
                        {
                            sortingList.Add(UserList[i].DisplayName);
                        }
                    }
                }

            }

            return sortingList;
        }

        private void SortByFirstName()
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
