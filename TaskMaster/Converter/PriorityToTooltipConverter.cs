using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskMaster.Converters
{
    public class PriorityToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Без приоритета";

            string priority = value.ToString().ToLower();
            switch (priority)
            {
                case "high":
                    return "Высокий приоритет";
                case "medium":
                    return "Средний приоритет";
                case "low":
                    return "Низкий приоритет";
                default:
                    return "Без приоритета";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
