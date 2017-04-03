using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using TechTool.Views;

namespace TechTool
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterTypeForNavigation<PingView>();
            Container.RegisterTypeForNavigation<EmailGeneratorView>();
        }
    }
}
