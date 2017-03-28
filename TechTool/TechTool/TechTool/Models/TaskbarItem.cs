using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
namespace TechTool.Models
{
    public class TaskbarItem
    {
        public NotifyIcon NotifyIcon { get; set; }
        public Icon Icon { get; set; }
        public bool Maximize { get; set; }

        public TaskbarItem()
        {
            NotifyIcon = new NotifyIcon();
            Icon = new Icon(@"E:\Apps\Repos\TechTool\TechTool\TechTool\ProjectImages\techtool.ico");
            NotifyIcon.Icon = Icon;

            NotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Maximize = true;
        }

        public void Test(Window wnd)
        {
            wnd.WindowState = WindowState.Normal;
        }

    }
}
