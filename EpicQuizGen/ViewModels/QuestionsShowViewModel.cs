using EpicQuizGen.Models;
using EpicQuizGen.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EpicQuizGen.ViewModels
{
    public class QuestionsShowViewModel : BindableBase
    {
        private QuestionView _questionView;
        public QuestionView QuestionView
        {
            get { return _questionView; }
            set { SetProperty(ref _questionView, value); }
        }
        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }

        public QuestionsShowViewModel()
        {
            QuestionView = new QuestionView();
            Questions = new ObservableCollection<Question>();
            for (int i = 0; i < 20; i++)
            {
                Question question = new Question();
                question.QuestionName = "Test" + i;
                question.QuestionCategory = new QuestionCategory();
                question.QuestionCategory = QuestionCategory.OPHTHALMIC;
                question.CreationDate = DateTime.Now;
                question.QuestionType = new QuestionTypes();
                question.QuestionType = QuestionTypes.TRUEFALSE;
                Questions.Add(question);
            }
        }
    }
}
