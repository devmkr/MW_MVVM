using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MinesWeeper.ViewModel;
using System.Windows.Media;
using System.Windows;

namespace MinesWeeper
{
    class FromStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is States))
                throw new ArgumentException($"{value}");
            var v = (States)value;

            return v == States.Lost ? Brushes.Red :
                   v == States.Won ? Brushes.Green :
                   (SolidColorBrush)Application.Current.FindResource("WindowTitleColorBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
