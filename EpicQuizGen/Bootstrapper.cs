using EpicQuizGen.Views;
using Prism.Unity;
using System.Windows;
using Microsoft.Practices.Unity;

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
            Container.RegisterTypeForNavigation<MultiChoice4View>();
            Container.RegisterTypeForNavigation<QuizView>();
            Container.RegisterTypeForNavigation<QuizTakeView>();
            Container.RegisterTypeForNavigation<TrueFalseQuizView>();
            Container.RegisterTypeForNavigation<MultiChoice4QuizView>();
        }
    }
}
