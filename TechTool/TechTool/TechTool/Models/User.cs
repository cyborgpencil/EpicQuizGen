using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTool.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = $"{FirstName} {LastName}";}
        }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
