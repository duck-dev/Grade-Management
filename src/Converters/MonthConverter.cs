using System;
using System.Globalization;
using Avalonia.Data.Converters;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
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
            if (parameter is not AddGradeViewModel viewModel)
                throw new ArgumentException("The type of the `parameter` argument should be `AddGradeViewModel`.");

            switch (value)
            {
                case int monthNumber:
                {
                    if (!Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear, out var protocol))
                    {
                        if (protocol.CustomHasFlag(DateType.Month))
                            goto default;
                    }
                    viewModel.SelectedMonth.Month = monthNumber;
                    return viewModel.SelectedMonth;
                }
                case string monthName when TryConvertMonth(monthName, out int monthNumber) &&
                                           Utilities.ValidateDate(1, monthNumber, Utilities.TodaysYear,
                                               out var protocol):
                {
                    viewModel.SelectedMonth.MonthName = monthName;
                    return viewModel.SelectedMonth;
                }
                default:
                    // TODO: Red outline/border around corresponding box in the view
                    System.Diagnostics.Trace.WriteLine("Month is invalid!");

                    return value;
            }
        }

        internal static string ConvertMonth(int month)
        {
            var date = new DateTime(Utilities.TodaysYear, month, Utilities.TodaysDay);
            // TODO: Use currently selected language as culture once it has been implemented
            return date.ToString("MMM", CultureInfo.CurrentCulture);
        }

        internal static bool TryConvertMonth(string month, out int monthNumber)
        {
            monthNumber = 0;
            // TODO: Use currently selected language as culture once it has been implemented
            if (!DateTime.TryParseExact(month, "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, 
                out var dateTime)) 
                return false;
            
            monthNumber = dateTime.Month;
            return true;
        }
    }
}