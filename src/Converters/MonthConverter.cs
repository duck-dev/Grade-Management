using System;
using System.Globalization;
using Avalonia.Data.Converters;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.AddPages;

namespace GradeManagement.Converters
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var instance = AddGradeViewModel.Instance;
            return instance is null ? string.Empty : instance.SelectedMonth.MonthName;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string monthName) 
                return ConversionFailed();
            
            // ReSharper disable once InlineOutVariableDeclaration
            int monthNumber;
            if (int.TryParse(monthName, out monthNumber))
            {
                if (!Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear, out var protocol) // Day and year aren't important
                    && protocol.CustomHasFlag(DateType.Month))
                {
                    return ConversionFailed();
                }
                return new MonthRepresentation(monthNumber);
            } 
            else if (TryParseMonth(monthName, out monthNumber) &&
                     Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear, out _)) // Day and year aren't important
                return new MonthRepresentation(monthName);

            return ConversionFailed();
        }

        internal static string ConvertMonth(int month)
        {
            var date = new DateTime(Utilities.TodaysYear, month, 1); // Day and year aren't important
            // TODO: Use currently selected language as culture once it has been implemented
            return date.ToString("MMMM", CultureInfo.CurrentCulture);
        }

        internal static bool TryParseMonth(string month, out int monthNumber)
        {
            monthNumber = 0;
            // TODO: Use currently selected language as culture once it has been implemented
            if (!DateTime.TryParseExact(month, "MMMM", CultureInfo.CurrentCulture, 
                DateTimeStyles.None, out var dateTime)) 
                return false;
            
            monthNumber = dateTime.Month;
            return true;
        }

        private static MonthRepresentation ConversionFailed() => new(string.Empty);
    }
}