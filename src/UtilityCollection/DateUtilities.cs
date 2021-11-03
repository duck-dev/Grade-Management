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
        /// <param name="validationProtocol">Returns an enum with a flag of the invalid paramters.</param>
        /// <returns>Whether the date is valid or not.</returns>
        [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
        public static bool ValidateDate(int day, int month, int year, out DateType validationProtocol)
        {
            bool dayValid = (day > 0) && (day <= DateTime.DaysInMonth(year, month));
            bool monthValid = month is > 0 and <= 12;
            
            validationProtocol = DateType.None;
            if (!dayValid)
                validationProtocol += 1;
            else if (!monthValid)
                validationProtocol += 2;

            return dayValid && monthValid;
        }

        /// <inheritdoc cref="ValidateDate(int,int,int,out GradeManagement.Enums.DateType)"/>
        /// <summary>
        /// Check whether a specific date (DateTime) is valid.
        /// </summary>
        public static bool ValidateDate(DateTime date, out DateType validationProtocol) 
            => ValidateDate(date.Day, date.Month, date.Year, out validationProtocol);
    }
}