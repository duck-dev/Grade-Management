using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GradeManagement.Converters
{
    public class IntStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.ToString();
        
        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")] // Unfortunately, I can't change it
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && int.TryParse(stringValue, out int intValue))
                return intValue;
            
            return -1;
        }
    }
}