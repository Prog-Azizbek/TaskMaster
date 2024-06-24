// Converters/BoolToColorConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskMaster.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Gray);

            string priority = value.ToString().ToLower();
            switch (priority)
            {
                case "high":
                    return new SolidColorBrush(Colors.Red); // Red
                case "medium":
                    return new SolidColorBrush(Colors.Orange); // Yellow
                case "low":
                    return new SolidColorBrush(Colors.Green); // Green
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

//                    return new SolidColorBrush(Color.FromRgb(197, 255, 208)); // Light green
