using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GradeManagement.Interfaces;

namespace GradeManagement.UtilityCollection
{
    public static partial class Utilities
    {
        /// <summary>
        /// Calculate the average of several grades with a weighting factor for each grade.
        /// </summary>
        /// <param name="gradables">A collection of <see cref="IGradable">IGradables</see>,
        ///                         whose grades will be used for the average.</param>
        /// <param name="round">Determines whether the returned average should be rounded to 2 decimal digits or not.</param>
        /// <returns>The calculated average, either rounded or exact, based on the passed bool <see cref="round"/></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static float GetAverage(IEnumerable<IGradable> gradables, bool round)
        {
            var enumerable = gradables.Where(x => x.Counts) as IGradable[] ?? gradables.ToArray();
            float result = enumerable.Sum(x => x.GradeValue * x.Weighting) / enumerable.Sum(x => x.Weighting);
            return round ? (float)Math.Round(result, 2) : result;
        }

        /// <summary>
        /// Log a message to the console (for debugging purposes).
        /// </summary>
        /// <param name="message">The message to be logged as a string.</param>
        public static void Log(string message) => System.Diagnostics.Trace.WriteLine(message);
    }
}