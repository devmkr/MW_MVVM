using GalaSoft.MvvmLight;
using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using MinesWeeper.ViewModel;

namespace MinesWeeper
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class FromStateToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the FromBooleanToVisbilityConverter class.
        /// </summary>
        public FromStateToBooleanConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is States))
                 throw new ArgumentException();

            return (States)value != States.Playing ? false : true;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}