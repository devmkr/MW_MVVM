using GalaSoft.MvvmLight;
using MinesWeeper.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Timers;

namespace MinesWeeper.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private MinnerBoard _model;
        private TimerModel _timermodel;    

        private ObservableCollection<ModelViewField> _plates;    
                
        public ObservableCollection<ModelViewField> Plates
        {
            get { return _plates; }
        }

        /// <summary>
        /// The <see cref="Size" /> property's name.
        /// </summary>
    
        private int _size;

        /// <summary>
        /// Sets and gets the SizeX property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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

        /// <summary>
        /// The <see cref="GameTimer" /> property's name.
        /// </summary>
        
        /// <summary>
        /// The <see cref="MinesPercentage" /> property's name.
        /// </summary>      

        private int _minesPercentage = 40;

        /// <summary>
        /// Sets and gets the MinesProcentage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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

        /// <summary>
        /// Sets and gets the GameTimer property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string GameTimer
        {
            get
            {
                return _timermodel.CurrentTime;
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

        /// <summary>
        /// The <see cref="GameState" /> property's name.
        /// </summary>
        public string GameStateMsg
        {
            get
            {
                return GameState == States.Lost ? "Lost" :
                       GameState == States.Won ? "win" :
                       string.Empty;
                    }
        }

        private States _gameState;

        /// <summary>
        /// Sets and gets the GameState property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public States GameState
        {
            get
            {
                return _gameState;
            }
            set
            {

                Set(nameof(ControlGameState), ref _gameState, value);
                RaisePropertyChanged(nameof(GameStateMsg));
            }
        }
      

        private RelayCommand _startCommand;

        /// <summary>
        /// Gets the StartCommand.
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand
                    ?? (_startCommand = new RelayCommand(
                    () =>
                    {
                        InitNewGame();
                    }));
            }
        }

        private RelayCommand<ModelViewField> _markFieldCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
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

        private List<ModelViewField> FindBlankArea(ref List<ModelViewField> z)
        {
            //temporary copying the list
            var zz = z;
            foreach (var i in z)
                //Find all zero adjacent fields                   
                zz = zz.Union(Plates.Where(j => j.IsAdjacent(i) && j.AdjecentMinnedFields == 0 && !j.IsMinned).ToList()).ToList();

            //recursion 
            return zz.Except(z).Count() == 0 ? z : FindBlankArea(ref zz);
        }
       
        public MainViewModel()
        {          
            //Default values  
            Size = 20;
            MinesPercentage = 40;

            //Init GameTimer
            _timermodel = new TimerModel(5);
         
            
        }
       

        public async void InitNewGame()
        {
           //Task for init board of Game.         
           var t1 =  Task<MinnerBoard>.Factory.StartNew(() => {return new MinnerBoard(Size, Size, (int)(((float)MinesPercentage / 100.0) * Size * Size)); });
           _model = await t1;
            
           var t2 = Task<ObservableCollection<ModelViewField>>.Factory.StartNew((() => { return new ObservableCollection<ModelViewField>(_model.Board.Select(x => new ModelViewField(x) { IsDisclosed = false, IsMarked = false })); }));
           _plates = await t2;
          
           RaisePropertyChanged(nameof(Plates));
           GameState = States.Playing;

            _timermodel = new TimerModel(5);
            _timermodel.StartTimer();
            _timermodel.PropertyChanged += (s, e) => { RaisePropertyChanged(nameof(GameTimer)); }; //To provide binding of timer
            _timermodel.TimeElapsed += (s, e) => { StopGame(States.Lost); }; //Time elapsed = game over

        }

        private void DiscloseEntireBoard()
        {
            foreach (var s in _plates)
            {
                s.IsDisclosed = true;
                s.IsMarked = false;
            }
                
        }

        private void ControlGameState(ModelViewField p)
        {
            if (p.IsMinned)
            {
                //TO DO stop game
                StopGame(States.Lost);
                return;
            }

            p.UnMark();
            p.Disclose();

            if (Plates.Where(x => x.IsMarked == true || x.IsDisclosed == true).Count()  == Plates.Count())
            {
                StopGame(States.Won);             
            }

               
        }

        private void StopGame(States gm)
        {
            GameState = gm;
            DiscloseEntireBoard();
            _timermodel.StopTimer();
        }

        private void OnTimeElapsed(object sender, ElapsedEventArgs args)
        {

        }

        //Inherited class for field; addition of parameters to display 
        public class ModelViewField : Field, INotifyPropertyChanged
        {
            public ModelViewField(Field field) : base(field)
            {

            }

            private bool _isDisclosed;
            private bool _isMarked;

            public bool IsDisclosed
            {
                get { return _isDisclosed; }
                set { if (value != _isDisclosed) { _isDisclosed = value; NotifyPropertyChanged(nameof(IsDisclosed)); } }
            }
            public bool IsMarked {
                get { return _isMarked; }
                set { if (value != _isMarked) { _isMarked = value; NotifyPropertyChanged(nameof(IsMarked)); } }
            }
            
            public void Disclose() => IsDisclosed = true;
            public void MarkField() => IsMarked = !IsMarked;
            public void UnMark() => IsMarked = false;

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
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