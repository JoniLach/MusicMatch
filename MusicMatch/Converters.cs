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
            if (value is bool isBooked&&isBooked)
            {
                return "BOOKED";
            }
            return "AVAILABLE";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }   

    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string path = value as string;
                if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                {
                    // Return null or a fallback image if you prefer
                    return null;
                }

                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
