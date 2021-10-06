using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Media;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public static class Utilities
    {
        /// <summary>
        /// Calculate the average of several grades with a weighting factor for each grade.
        /// </summary>
        /// <param name="gradables">A collection of <see cref="IGradable"/>, whose grades will be used for the average.</param>
        /// <param name="round">Determines whether the returned average should be rounded to 2 decimal digits or not.</param>
        /// <returns>The calculated average, either rounded or exact, based on the passed bool <see cref="round"/></returns>
        public static float GetAverage(IEnumerable<IGradable> gradables, bool round)
        {
            var enumerable = gradables as IGradable[] ?? gradables.ToArray();
            float result = enumerable.Sum(x => x.GradeValue * x.Weighting) / enumerable.Sum(x => x.Weighting);
            return round ? (float)Math.Round(result, 2) : result;
        }

        /// <summary>
        /// Chooses the specified dark or light tint, based on the background's brightness.
        /// Dark background => Light tint and vice-versa.
        /// </summary>
        /// <param name="backgroundColor">The background color, whose brightness determines the foreground tint.</param>
        /// <param name="darkColor">The dark tint.</param>
        /// <param name="lightColor">The light tint.</param>
        /// <param name="threshold">The threshold, at which the color becomes dark upwards and light downwards.
        ///                         Default value: 130</param>
        /// <returns>The adjusted foreground color.</returns>
        public static Color AdjustForegroundBrightness(Color backgroundColor, Color darkColor, Color lightColor, 
                                                       int threshold = 130)
        {
            return ((PerceivedBrightness(backgroundColor) > threshold) ? darkColor : lightColor);
        }

        /// <summary>
        /// Calculate the brightness of a color.
        /// </summary>
        /// <param name="color">The passed <see cref="Avalonia.Media.Color"/>.</param>
        /// <returns>The brightness represented as an integer.</returns>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static int PerceivedBrightness(Color color)
        {
            return (int)Math.Sqrt(
                color.R * color.R * .299 +
                color.G * color.G * .587 +
                color.B * color.B * .114);
        }

        public static Color DarkenColor(Color color, float amount) => AdjustTint(color, Colors.Black, amount);
        public static Color BrightenColor(Color color, float amount) => AdjustTint(color, Colors.White, amount);
        public static Color AdjustTint(Color color, Color goal, float amount) => color.Lerp(goal, amount);
    }
}