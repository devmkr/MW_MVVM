using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace MinesWeeper.Model
{
    class TimerModel : DispatcherTimer, INotifyPropertyChanged
    {
        public delegate void ElapsedTimeEventHandler (object source, EventArgs e);

        public event PropertyChangedEventHandler PropertyChanged;
        public event ElapsedTimeEventHandler TimeElapsed ;
 
        private TimeSpan _currentTime;
        private TimeSpan _stopTime = new TimeSpan(0, 0, 0);

        public string CurrentTime
        {
            get
            {
                return new DateTime(_currentTime.Ticks).ToString("mm:ss");
            }
        }
        public TimerModel(int seconds) : base()
        {
            SetCountdownTime(seconds);

            Interval = new TimeSpan(0, 0, 1);

            Tick += OnDispatchersTick;
            TimeElapsed += OnTimeElapsed;
        }       

        public void SetCountdownTime(int seconds)
        {
            _currentTime = new TimeSpan(0, 0, seconds);
        }

        private void OnTimeElapsed(object source, EventArgs e) => Stop();      
      

        private void NotifyPropertyChanged(string info) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info)); 
      

        private void OnDispatchersTick(object sender, EventArgs e)
        {
            _currentTime -= Interval;

            if (_currentTime <= _stopTime)
                TimeElapsed(this, EventArgs.Empty);

            NotifyPropertyChanged(nameof(CurrentTime));
        }       

    }
    
}
