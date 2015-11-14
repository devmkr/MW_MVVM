using GalaSoft.MvvmLight;
using MinesWeeper.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Specialized;
using System.ComponentModel;

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
        MinnerBoard _model;       

        ObservableCollection<ModelViewField> _plates;        
      
        public ObservableCollection<ModelViewField> Plates
        {
            get { return _plates; }
        }

        /// <summary>
        /// The <see cref="SizeX" /> property's name.
        /// </summary>
    

        private int _sizeX;

        /// <summary>
        /// Sets and gets the SizeX property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int SizeX
        {
            get
            {
                return _sizeX;
            }
            set
            {
                Set(nameof(SizeX), ref _sizeX, value);
            }
        }
        /// <summary>
        /// The <see cref="SizeY" /> property's name.
        /// </summary>


        private int _sizeY;

        /// <summary>
        /// Sets and gets the SizeX property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int SizeY
        {
            get
            {
                return _sizeY;
            }
            set
            {
                Set(nameof(SizeY), ref _sizeY, value);
            }
        }
        private RelayCommand<ModelViewField> _disclosePlate;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<ModelViewField> DisclosePlate
        {
            get
            {
                return _disclosePlate
                    ?? (_disclosePlate = new RelayCommand<ModelViewField>(
                    p =>
                    {
                        if (p.IsMinned)
                        {
                            //TO DO stop game
                            DiscloseAllPlates();
                        }                        
                        else if(p.NumberOfMinnedNeighbours == 0)
                        {
                            var tmp = new List<ModelViewField>() { p };                            
                            foreach (var z in FindArea(ref tmp))
                                z.Disclose(); 
                                                           
                        }    
                        //TO DO CHECK IS GAME OVER
                                          
                        p.Disclose();                           

                    }));
            }
        }
        private List<ModelViewField> FindZerosArea(ModelViewField f)
        {      
            return Plates.Where(i => i.IsNeighbour(f) && i.NumberOfMinnedNeighbours == 0).ToList();
        }

        private List<ModelViewField> FindArea(ref List<ModelViewField> z)
        {
            var zz = z;
            foreach (var i in z)
                zz = zz.Union(FindZerosArea(i)).ToList();

            return zz.Except(z).Count() == 0 ? z : FindArea(ref zz);
         

        }
       
        public MainViewModel()
        {
            
            SizeX = SizeY = 20;
            _model = new MinnerBoard(SizeX, SizeY, 40);
            _plates = new ObservableCollection<ModelViewField>(_model.Board.Select(x => new ModelViewField(x) { IsDisclosed = false, IsMarked = false }));            
            RaisePropertyChanged(nameof(Plates));

                        
           
        }       

        private void DiscloseAllPlates()
        {
            foreach (var s in _plates)
                s.IsDisclosed = true;
          //  RaisePropertyChanged(nameof(Plates));
        }

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
            public bool IsMarked { get; set; }
            
            public void Disclose() => IsDisclosed = true;           


            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(String info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
        }

    }
}