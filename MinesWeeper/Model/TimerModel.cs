using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MinesWeeper.Model
{
    class TimerModel : INotifyPropertyChanged
    {
        public delegate void ElapsedTimeEventHandler (object source, EventArgs e);

        public event PropertyChangedEventHandler PropertyChanged;
        public event ElapsedTimeEventHandler TimeElapsed;

        private DispatcherTimer _dt;    
        private TimeSpan _currentTime;
        private TimeSpan _stopTime = new TimeSpan(0, 0, 0);


        public string CurrentTime
        {
            get
            {
                return new DateTime(_currentTime.Ticks).ToString("mm:ss");
            }
        }
        public TimerModel(int seconds)
        {
            _currentTime = new TimeSpan(0,0, seconds); 

            _dt = new DispatcherTimer();
            
            _dt.Tick += new EventHandler(OnDispatchersTick);
            _dt.Interval = new TimeSpan(0, 0, 1);
            _dt.Start();

            TimeElapsed += OnTimeElapsed;
        }

        public void StartTimer() => _dt.Start();

        public void StopTimer()
        {
            if (_dt.IsEnabled)            
                _dt.Stop();
            
         
        }

        private void OnTimeElapsed(object source, EventArgs e) => _dt.Stop();      
      

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void OnDispatchersTick(object sender, EventArgs e)
        {
            _currentTime -= _dt.Interval;

            if (_currentTime <= _stopTime)
                TimeElapsed(this, EventArgs.Empty);

            NotifyPropertyChanged(nameof(CurrentTime));
        }

        

    }

    
}
