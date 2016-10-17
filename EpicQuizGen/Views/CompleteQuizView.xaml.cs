using EpicQuizGen.ViewModels;
using System.Windows;

namespace EpicQuizGen.Views
{
    /// <summary>
    /// Interaction logic for CompleteQuizView.xaml
    /// </summary>
    public partial class CompleteQuizView : Window
    {
        public CompleteQuizView()
        {
            InitializeComponent();
            var d = new QuizTakeViewModel();
            DataContext = d;
        }
    }
}
