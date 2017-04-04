/// <summary>
/// Class used to deal with Users and User Data
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Threading.Tasks;
using TechTool.Models;

namespace TechTool.Utils
{
    public class UsersControl
    {
        public ObservableCollection<User> UserList { get; set; }
        public int MyProperty { get; set; }
        private string DomainPath { get; set; }
        DirectoryEntry searchRoot;
        DirectorySearcher search;
        public User CurrentUser { get; set; }

        public UsersControl()
        {
            CurrentUser = new User();
            DomainPath = @"us.chs.net";
            searchRoot = new DirectoryEntry(DomainPath, "elmorrow", "0132Ermo1");
            
            search = new DirectorySearcher(searchRoot);
        }

        public ObservableCollection<User> ReturnADUsers()
        {
            UserList = new ObservableCollection<User>();

            using (var context = new PrincipalContext(ContextType.Domain, DomainPath, "OU=Users,OU=Deming,OU=NM,DC=US,DC=chs,DC=net"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        CurrentUser = new User();
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        CurrentUser.Username = de.Properties["samAccountName"].Value.ToString();
                        if (de.Properties["givenName"].Value != null)
                        {
                            CurrentUser.FirstName = de.Properties["givenName"].Value.ToString();
                        }
                        if (de.Properties["sn"].Value != null)
                        {
                            CurrentUser.LastName = de.Properties["sn"].Value.ToString();
                        }
                        if (de.Properties["mailAddress"].Value != null)
                        {
                            CurrentUser.Email = de.Properties["mailAddress"].Value.ToString();
                        }
                        if (de.Properties["displayName"].Value != null)
                        {
                            CurrentUser.DisplayName = de.Properties["displayName"].Value.ToString();
                        }
                        UserList.Add(CurrentUser);
                    }
                }
            }

            return UserList;
        }

        public Task<ObservableCollection<User>> ReturnADUsersAsync()
        {
            return Task.Factory.StartNew(() => ReturnADUsers());
        }
    }
}
