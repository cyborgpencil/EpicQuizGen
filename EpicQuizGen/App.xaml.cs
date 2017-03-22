using EpicQuizGen.Utils;
using System.Windows;

namespace EpicQuizGen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Create Directories
            DirectoryManager.Instance.CreateDirectory();

            base.OnStartup(e);

            var bs = new Bootstrapper();
            bs.Run();

            
        }
    }
}
