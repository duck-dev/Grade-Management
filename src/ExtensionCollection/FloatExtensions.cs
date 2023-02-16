using System;

namespace GradeManagement.ExtensionCollection
{
    public static partial class Extensions
    {
        /// <summary>
        /// Linear interpolation between two float's.
        /// </summary>
        /// <param name="start">Start value (a)</param>
        /// <param name="end">End value (b)</param>
        /// <param name="amount">Amount (t)</param>
        /// <returns>Linearly interpolated value.</returns>
        public static float Lerp(this float start, float end, float amount)
        {
            amount = Math.Clamp(amount, 0, 1);
            return start + (end - start) * amount;
        }

        /// <summary>
        /// Test equality between two nullable floats.
        /// </summary>
        /// <param name="a">Nullable float A</param>
        /// <param name="b">Nullable float B</param>
        /// <param name="tolerance">The tolerance for deviation.</param>
        /// <returns>Returns `true` if the absolute difference is smaller than the tolerance, otherwise it returns `false`.</returns>
        public static bool AlmostEquals(this float? a, float? b, float tolerance)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a is not { } aFloat || b is not { } bFloat)
                return a == b;

            return AlmostEquals(aFloat, bFloat, tolerance);
        }
        
        /// <summary>
        /// Test equality between two floats.
        /// </summary>
        /// <param name="a">Float a</param>
        /// <param name="b">Float b</param>
        /// <param name="tolerance">The tolerance for deviation.</param>
        /// <returns>Returns `true` if the absolute difference is smaller than the tolerance, otherwise it returns `false`.</returns>
        public static bool AlmostEquals(this float a, float b, float tolerance)
            => Math.Abs(a - b) < tolerance;
    }
}