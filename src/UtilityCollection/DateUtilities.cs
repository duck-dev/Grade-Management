using System;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Enums;

namespace GradeManagement.UtilityCollection
{
    public static partial class Utilities
    {
        public static int TodaysDay => DateTime.Today.Day;
        public static string TodaysMonth => DateTime.Today.ToString("MMMM");
        public static int TodaysYear => DateTime.Today.Year;

        /// <summary>
        /// Check whether a specific date (day, month, year) is valid.
        /// </summary>
        /// <param name="validationProtocol">Returns an enum with a flag of the invalid parameters.</param>
        /// <returns>Whether the date is valid or not.</returns>
        [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
        public static bool ValidateDate(int day, int month, int year, out DateType validationProtocol)
        {
            validationProtocol = DateType.None;

            bool monthValid = month is > 0 and <= 12;
            if (!monthValid)
            {
                validationProtocol = DateType.Month;
                return false;
            }
            
            bool yearValid = year is >= 1 and <= 9999;
            if (!yearValid)
            {
                validationProtocol = DateType.Year;
                return false;
            }

            bool dayValid = (day > 0) && (day <= DateTime.DaysInMonth(year, month));
            if (!dayValid)
                validationProtocol = DateType.Day;

            return dayValid;
        }

        /// <inheritdoc cref="ValidateDate(int,int,int,out GradeManagement.Enums.DateType)"/>
        /// <summary>
        /// Check whether a specific date (DateTime) is valid.
        /// </summary>
        public static bool ValidateDate(DateTime? date, out DateType validationProtocol)
        {
            validationProtocol = DateType.None;
            
            if (date is null)
                return false;
            
            var newDate = date.Value;
            return ValidateDate(newDate.Day, newDate.Month, newDate.Year, out validationProtocol);
        }
    }
}