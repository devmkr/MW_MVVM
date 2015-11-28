using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using MinesWeeper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinesWeeper.ViewModel
{
 
    public class MainViewModel : ViewModelBase
    {
        private MinnerBoard _model;
        private TimerModel _timermodel;    

        private ObservableCollection<ModelViewField> _plates;                   
        public ObservableCollection<ModelViewField> Plates
        {
            get { return _plates; }
            set { Set(nameof(Plates), ref _plates, value); }
        }
        
        private int _size = 20;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                Set(nameof(Size), ref _size, value);
            }
        } 
        
        private int _minesPercentage = 40;
        public int MinesPercentage
        {
            get
            {
                return _minesPercentage;
            }
            set
            {
                Set(nameof(MinesPercentage), ref _minesPercentage, value);
            }
        }
       
        public string GameTimer
        {
            get
            {
                return _timermodel != null ? _timermodel.CurrentTime : string.Empty;
            }
           
        }

        private RelayCommand<ModelViewField> _disclosePlate;
        public RelayCommand<ModelViewField> DisclosePlate
        {
            get
            {
                return _disclosePlate
                    ?? (_disclosePlate = new RelayCommand<ModelViewField>(
                    p =>
                    {
                        ControlGameState(p);

                        if (p.AdjecentMinnedFields == 0)
                        {                            
                            var tmp = new List<ModelViewField>() { p };

                            //Finding zeros area(blank area) and disclose all these fields
                            foreach (var z in FindBlankArea(ref tmp))
                                z.Disclose();                                                            
                        }                                                                                       

                    }));
            }
        }

        public string GameStateMsg
        {
            get
            {
                return GameState == States.Lost ? "Lost" :
                       GameState == States.Won ? "Win" :
                       string.Empty;
                    }
        }

        private States _gameState;       
        public States GameState
        {
            get
            {
                return _gameState;
            }
            set
            {

                Set(nameof(GameState), ref _gameState, value);
                RaisePropertyChanged(nameof(GameStateMsg));
            }
        }

        public int GameTimeInSecond { get; set; }

        private RelayCommand _startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand
                    ?? (_startCommand = new RelayCommand(
                    async () =>
                    {                      
                        await Task.Run(()=> InitNewGame());          

                    }));
            }
        }

        private RelayCommand<ModelViewField> _markFieldCommand;
        public RelayCommand<ModelViewField> MarkFieldCommand
        {
            get
            {
                return _markFieldCommand
                    ?? (_markFieldCommand = new RelayCommand<ModelViewField>(
                    p =>
                    {
                        if (!p.IsDisclosed)                        
                            p.MarkField();
                         ControlGameState(p);
                    }));
            }
        }
       
        public MainViewModel()
        {

        }           
  
        public void InitNewGame()
        {
            Thread.Sleep(3000);     
            _model = MinnerBoard.Create(Size, Size, (int)(((float)MinesPercentage / 100.0) * Size * Size));
            _plates = new ObservableCollection<ModelViewField>(_model.Board.Select(x => new ModelViewField(x)));
            
            DispatcherHelper.CheckBeginInvokeOnUI(              
               () =>   RaisePropertyChanged(nameof(Plates)));                         
            StartGame();
           
        }
    

        public void InitTimer()
        {
            int _limitTimeInSeconds = (int)Math.Sqrt(Size * 1.5)*20;

            //First init
            _timermodel = _timermodel ?? new TimerModel(_limitTimeInSeconds);
                   
            _timermodel.SetCountdownTime(_limitTimeInSeconds);
            _timermodel.Start();

            //Register subsribers
            _timermodel.PropertyChanged += (s, e) => { RaisePropertyChanged(nameof(GameTimer)); }; //To provide binding of timer
            _timermodel.TimeElapsed += (s, e) => { StopGame(States.Lost); }; //Time elapsed = game over           
        }

        private void DiscloseEntireBoard()
        {
            foreach (var s in _plates)
            {
                s.Disclose();
                s.UnMark();               
            }                
        }

        private void ControlGameState(ModelViewField p)
        {
            if (p.IsMinned)
            {
                StopGame(States.Lost);
                return;
            }

            p.UnMark();
            p.Disclose();

            if (Plates.Where(x => x.IsMarked == true || x.IsDisclosed == true).Count()  == Plates.Count())
               StopGame(States.Won);            
                           
        }
        private void StartGame()
        {
            GameState = States.Playing;
            InitTimer();
        }

        private void StopGame(States gm)
        {
            GameState = gm;
            DiscloseEntireBoard();
            _timermodel.Stop();
        }

        private List<ModelViewField> FindBlankArea(ref List<ModelViewField> z)
        {
            //temporary copying the list
            var zz = z;
            foreach (var i in z)
                //Find all zeros adjacent fields                   
                zz = zz.Union(Plates.Where(j => j.IsAdjacent(i) && j.AdjecentMinnedFields == 0 && !j.IsMinned)).ToList();

            //recursion 
            return zz.Except(z).Count() == 0 ? z : FindBlankArea(ref zz);
        }
   
        //Inherited class for field; addition parameters to display 
        public class ModelViewField : Field, INotifyPropertyChanged
        {
            public ModelViewField(Field field) : base(field)
            {
              
            }

            private bool _isDisclosed = false;
            private bool _isMarked = false;

            public bool IsDisclosed
            {
                get { return _isDisclosed; }
                private set { if (value != _isDisclosed) { _isDisclosed = value; NotifyPropertyChanged(nameof(IsDisclosed)); } }
            }
            public bool IsMarked {
                get { return _isMarked; }
                private set { if (value != _isMarked) { _isMarked = value; NotifyPropertyChanged(nameof(IsMarked)); } }
            }
            
            public void Disclose() => IsDisclosed = true;
            public void MarkField() => IsMarked = !IsMarked;
            public void UnMark() => IsMarked = false;

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string info) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
   
    public enum States
    {
        Playing,
        Lost,
        Won,
        Paused
    }

}