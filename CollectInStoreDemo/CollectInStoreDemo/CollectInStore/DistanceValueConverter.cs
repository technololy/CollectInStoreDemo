using System;
using System.Globalization;
using Xamarin.Forms;


namespace TotalPlatformCommon.Shared.CollectInStore
{
    public class DistanceValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double distance)
            {
                var format = ("{0:F2} Miles");
                return string.Format(format, distance);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
