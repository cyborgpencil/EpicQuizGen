using Prism.Mvvm;
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
            set { SetProperty(ref _firstName, value); }
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
                ShowSelectedUser();
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

        public EmailGeneratorViewModel()
        {
            // List from AD
            UserList = new ObservableCollection<User>(new List<User>());

            // Binding List
            UserListBind = new ObservableCollection<string>(new List<string>());

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
            for (int i = 0; i < UserList.Count; i++)
            {
                UserListBind.Add(UserList[i].DisplayName);
            }
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
    }
}
