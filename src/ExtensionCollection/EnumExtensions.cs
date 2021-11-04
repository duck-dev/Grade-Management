using GradeManagement.Enums;

namespace GradeManagement.ExtensionCollection
{
    public static partial class Extensions
    {
        /// <summary>
        /// Faster version of HasFlag
        /// </summary>
        /// <param name="enumeration">The extended enum.</param>
        /// <param name="value">The flag value.</param>
        /// <returns>Has flag?</returns>
        public static bool CustomHasFlag(this DateType enumeration, DateType value)
            => (enumeration & value) == value;
    }
}