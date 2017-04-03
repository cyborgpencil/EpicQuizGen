/// <summary>
/// Class used to deal with Users and User Data
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;

namespace TechTool.Utils
{
    public class UsersControl
    {
        public List<string> UsersName { get; set; }
        private string DomainPath { get; set; }
        DirectoryEntry searchRoot;
        DirectorySearcher search;
        SearchResult result;
        SearchResultCollection resultCol;

        public UsersControl()
        {
            DomainPath = "LDAP://CN=NM0142INFWGC01,DC=US.chs.net";
            searchRoot = new DirectoryEntry(DomainPath, "elmorrow", "0132Ermo1");
            
            search = new DirectorySearcher(searchRoot);
        }

        public List<string> GetADUserNames()
        {
            try
            {
                UsersName = new List<string>();
                search.Filter = "(&(objectCategory=person)(objectClass=user))";
                resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int i = 0; i < resultCol.Count; i++)
                    {
                        result = resultCol[i];
                        if (result.Properties.Contains("sameaccountname"))
                        {
                            UsersName.Add((string)result.Properties["samaccountname"][0]);
                        }

                    }
                }

            }
            catch (Exception e)
            {

                return null;
            }

            return UsersName;
        }
    }
}
