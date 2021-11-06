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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var instance = AddGradeViewModel.Instance;
            return instance is null ? string.Empty : instance.SelectedMonth.MonthName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if (parameter is not AddGradeViewModel viewModel)
            //     throw new ArgumentException("The type of the `parameter` argument should be `AddGradeViewModel`.");

            if (value is not string monthName) 
                return ConversionFailed();
            
            // ReSharper disable once InlineOutVariableDeclaration
            int monthNumber;
            if (int.TryParse(monthName, out monthNumber))
            {
                if (!Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear, out var protocol)
                    && protocol.CustomHasFlag(DateType.Month))
                {
                    return ConversionFailed();
                }
                // viewModel.SelectedMonth.Month = monthNumber;
                // return viewModel.SelectedMonth;
                return new MonthRepresentation(monthNumber);
            } 
            else if (TryConvertMonth(monthName, out monthNumber) &&
                     Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear, out _))
            {
                // viewModel.SelectedMonth.MonthName = monthName;
                // return viewModel.SelectedMonth;
                return new MonthRepresentation(monthName);
            }

            return ConversionFailed();
        }

        internal static string ConvertMonth(int month)
        {
            var date = new DateTime(Utilities.TodaysYear, month, Utilities.TodaysDay);
            // TODO: Use currently selected language as culture once it has been implemented
            return date.ToString("MMMM", CultureInfo.CurrentCulture);
        }

        internal static bool TryConvertMonth(string month, out int monthNumber)
        {
            monthNumber = 0;
            // TODO: Use currently selected language as culture once it has been implemented
            if (!DateTime.TryParseExact(month, "MMMM", CultureInfo.CurrentCulture, 
                DateTimeStyles.None, out var dateTime)) 
                return false;
            
            monthNumber = dateTime.Month;
            return true;
        }

        private static MonthRepresentation ConversionFailed()
        {
            // TODO: Red outline/border around corresponding box in the view
            System.Diagnostics.Trace.WriteLine("Month is invalid!");

            return new MonthRepresentation(string.Empty);
        }
    }
}