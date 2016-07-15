using Prism.Mvvm;

namespace EpicQuizGen.ViewModels
{
    public class QuizQuestionSelectViewModel : BindableBase
    {
        private string _test;
        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        public QuizQuestionSelectViewModel()
        {
            Test = "Test";
        }

    }
}
