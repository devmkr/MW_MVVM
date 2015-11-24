using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimerControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Timer : UserControl
    {
        public Timer()
        {
           // TimeFormat = new DataFormat();
           InitializeComponent();
        }

        //public static readonly DependencyProperty TimeFormatProperty =  DependencyProperty.Register(nameof(TimeFormat), typeof(DataFormat), typeof(Timer), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(Timer));

        public string TimeString
        {
            get { return Time.ToString("mm:ss"); }
        }

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }

        }
        //public DataFormat TimeFormat
        //{
        //    get { return GetValue(TimeFormatProperty) as DataFormat; }
        //    set { SetValue(TimeFormatProperty, value); }
        //}





    }
}
