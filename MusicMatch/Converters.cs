using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicMatch
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBooked)
            {
                return isBooked ? new SolidColorBrush(Color.FromRgb(255, 85, 85)) : new SolidColorBrush(Color.FromRgb(29, 185, 84));
            }
            return new SolidColorBrush(Color.FromRgb(29, 185, 84));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBooked)
            {
                return isBooked ? "BOOKED" : "AVAILABLE";
            }
            return "AVAILABLE";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
