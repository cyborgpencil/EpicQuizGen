using EpicQuizGen.Views;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using EpicQuizGen.Views;

namespace EpicQuizGen
{
    public class Bootstrapper : UnityBootstrapper
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

            Container.RegisterTypeForNavigation<QuestionsShowView>();
            Container.RegisterTypeForNavigation<QuizTakeView>();
            Container.RegisterTypeForNavigation<QuizzesShowView>();
            Container.RegisterTypeForNavigation<QuestionView>();
            Container.RegisterTypeForNavigation<TrueFalseView>();
        }
    }
}
