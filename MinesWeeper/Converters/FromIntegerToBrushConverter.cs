using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MinesWeeper
{
    class FromIntegerToBrushConverter : IValueConverter
    {
        static Dictionary<int, Color> DictIntColor = new Dictionary<int, Color>()
        {
            [0] = (Color)ColorConverter.ConvertFromString("#BDBDBD"),
            [1] = (Color)ColorConverter.ConvertFromString("#D0FA58"),
            [2] = (Color)ColorConverter.ConvertFromString("#81F7F3"),
            [3] = (Color)ColorConverter.ConvertFromString("#8181F7"),
            [4] = (Color)ColorConverter.ConvertFromString("#9A2EFE"),
            [5] = (Color)ColorConverter.ConvertFromString("#08088A"),
            [6] = (Color)ColorConverter.ConvertFromString("#610B38"),
            [7] = (Color)ColorConverter.ConvertFromString("#610B0B"),
            [8] = (Color)ColorConverter.ConvertFromString("#FFBF00"),
            [9] = (Color)ColorConverter.ConvertFromString("#FFBF20")

        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int) || (int)value > 9 || (int)value < 0)
                throw new ArgumentException($"{value}");


            return new SolidColorBrush(DictIntColor[(int)(value ?? 0)]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
