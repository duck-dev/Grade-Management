using System.Text.RegularExpressions;

namespace GradeManagement.ExtensionCollection
{
    public static partial class Extensions
    {
        /// <summary>
        /// Split a string with a camelCase (or PascalCase) pattern into each word and join them with a space together.
        /// </summary>
        /// <param name="input">The input string that's supposed to be split.</param>
        /// <param name="separator">The separator, which will be used to separate each word in the final string.
        ///                         Default: Single Space.</param>
        /// <returns>An array of all separate words.</returns>
        public static string SplitCamelCase(this string input, string separator = " ")
        {
            var words = Regex.Split(input, "([A-Z][a-z]+)");
            return string.Join(separator, words).Trim();
        }
    }
}