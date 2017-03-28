using Prism.Commands;
using Prism.Mvvm;
using System.Net.NetworkInformation;
using System;
using System.Diagnostics;

namespace TechTool.ViewModels
{
    class PingViewModel : BindableBase
    {
#region Properties
        private string _pingResult;
        public string PingResult
        {
            get { return _pingResult; }
            set { SetProperty(ref _pingResult, value); }
        }
        private Ping ping;

        private string _hostOrIP;
        public string HostOrIP
        {
            get { return _hostOrIP; }
            set { SetProperty(ref _hostOrIP, value); }
        }

        public DelegateCommand PingCommand { get; set; }
        #endregion

        public PingViewModel()
        {
            PingCommand = new DelegateCommand(PingStart);
            ping = new Ping();
        }

        #region Methods

        private void PingStart()
        {
            
        }
        #endregion
    }
}
