using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTool.Models;
using TechTool.Utils;

namespace TechTool.ViewModels
{
    public class EmailGeneratorViewModel
    {
        private UsersControl userControl;
        public User Users { get; set; }

        public EmailGeneratorViewModel()
        {
            Users = new User();
            userControl = new UsersControl();

            // Get AD users
            List<string> users = userControl.GetADUserNames();
            
        }
    }
}
