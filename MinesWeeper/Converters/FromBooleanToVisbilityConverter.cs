using GalaSoft.MvvmLight;
using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace MinesWeeper
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class FromBooleanToVisbilityConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the FromBooleanToVisbilityConverter class.
        /// </summary>
        public FromBooleanToVisbilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                 throw new ArgumentException();

            return (bool)value == true ? Visibility.Visible : Visibility.Hidden;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}