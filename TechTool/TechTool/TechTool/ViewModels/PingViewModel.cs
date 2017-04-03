using Prism.Commands;
using Prism.Mvvm;
using System.Net.NetworkInformation;
using System;
using System.Diagnostics;
using System.Text;

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
        private PingOptions _po;

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
            _po = new PingOptions();
        }

        #region Methods

        private void PingStart()
        {
            _po.DontFragment = true;

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply;
            PingResult = $"Pinging {HostOrIP}....";


            if (string.IsNullOrWhiteSpace(HostOrIP))
            {
                PingResult = "Please Enter a Hostname Or IP address";
            }
            else
            {
                try
                {
                    PingResult = $"Pinging {HostOrIP}....";
                    reply = ping.Send(HostOrIP, timeout, buffer, _po);
                    if (reply.Status == IPStatus.Success)
                    {
                        PingResult = $"Ping to {reply.Address.ToString()} Successful.";
                    }
                    else
                        PingResult = $"could not reach {reply.Address.ToString()}.";
                }
                catch(Exception e)
                {
                    PingResult = $"Pinging {HostOrIP}....";
                    PingResult = "Please Enter a Hostname Or IP address";
                }

                
            }
        }
        #endregion
    }
}
