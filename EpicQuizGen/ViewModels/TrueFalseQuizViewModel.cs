using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicQuizGen.ViewModels
{
    class TrueFalseQuizViewModel : BindableBase
    {
        #region Properties
        private bool _trueAnswer;
        public bool TrueAnswer
        {
            get { return _trueAnswer; }
            set { SetProperty(ref _trueAnswer, value); }
        }
        private bool _falseAnswer;
        public bool FalseAnswer
        {
            get { return _falseAnswer; }
            set { SetProperty(ref _falseAnswer, value); }
        }
        #endregion

        #region Contructors
        public TrueFalseQuizViewModel()
        {


            TFQuizViewLoadCommand = new DelegateCommand(TFQuizViewLoad);
        }
        #endregion

        #region Commands
        public DelegateCommand TFQuizViewLoadCommand { get; set; }
        public void TFQuizViewLoad()
        {

        }
        #endregion

        #region Events
        #endregion

        #region Methods
        #endregion



    }
}
