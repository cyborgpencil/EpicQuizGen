using EpicQuizGen.ViewModels;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace EpicQuizGen.Utils
{
    public class QuizTimeManager : BindableBase
    {
        private DispatcherTimer DispatcherTimer { get; set; }
        public int QuizTimerIntevalHours { get; set; }
        public int QuizTimerIntervalMinutes { get; set; }
        public int QuizTimeIntervalSeconds { get; set; }
        private int _quizTimerCounter;

        private int _quizTimerCountet;
        public int QuizTimerCounter
        {
            get { return _quizTimerCountet; }
            set { SetProperty(ref _quizTimerCountet, value); _quizTimerCounter = value;
                if (value < 0)
                {
                    DispatcherTimer.Stop();
                }
            }
        }
        
        private QuizTakeViewModel _quizTakeViewModel;

        private delegate void Display();

        public QuizTimeManager(QuizTakeViewModel quizTakeViewModel)
        {
            _quizTakeViewModel = quizTakeViewModel;
            // each quiz will be at lease 5 secs
            QuizTimeIntervalSeconds = 1;
            QuizTimerCounter = 5;
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += _quizTakeViewModel.QuizTimerEvent;
            DispatcherTimer.Interval = new TimeSpan(QuizTimerIntevalHours, QuizTimerIntervalMinutes, QuizTimeIntervalSeconds);
        }

        public void StartTimer()
        {
            DispatcherTimer.Start();
        }

        public void StopTimer()
        {
            DispatcherTimer.Stop();
        }

        public int SendCurrentSecound()
        {
            int dec = QuizTimerCounter;
            QuizTimerCounter -= 1;
            return dec;
        }

        public bool GetTimerEnabled()
        {
            return DispatcherTimer.IsEnabled;

        }
    }
}
