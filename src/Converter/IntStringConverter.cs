using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GradeManagement.Converter
{
    public class IntStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.ToString();
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && int.TryParse(stringValue, out int intValue))
                return intValue;
            
            return 0;
        }
    }
}