using System.Windows;

namespace MyCSharpLib.Wpf.Controls
{
    public class SpinnerChangedRoutedEventArgs : RoutedEventArgs
    {
        public double Interval { get; set; }

        public SpinnerChangedRoutedEventArgs(RoutedEvent routedEvent, double interval) : base(routedEvent)
        {
            Interval = interval;
        }
    }
}
