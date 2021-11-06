using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;

namespace GradeManagement.UtilityCollection
{
    public static partial class Utilities
    {
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
            int threshold = 110)
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
        
        /// <summary>
        /// Create a <see cref="LinearGradientBrush"/> with variable start-, endpoints and gradient stops.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="endPoint">The end point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="gradientStopInfos">A collection of <see cref="KeyValuePair"/>, each of them containing
        /// the <see cref="Color"/> and Offset of the <see cref="GradientStop"/> it's intended for.</param>
        /// <returns>LinearGradientBrush</returns>
        public static LinearGradientBrush CreateLinearGradientBrush(RelativePoint startPoint, RelativePoint endPoint, 
            IEnumerable<KeyValuePair<Color, double>> gradientStopInfos)
        {
            var gradientStops = new GradientStops();
            foreach (var (key, value) in gradientStopInfos)
                gradientStops.Add(new GradientStop(key, value));

            return CreateLinearGradientBrush(startPoint, endPoint, gradientStops);
        }
        
        /// <summary>
        /// Create a <see cref="LinearGradientBrush"/> with variable start-, endpoints and gradient stops.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="endPoint">The end point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="colors">The color of each <see cref="GradientStop"/></param>
        /// <param name="offsets">The offset of each <see cref="GradientStop"/></param>
        /// <returns>LinearGradientBrush</returns>
        public static LinearGradientBrush CreateLinearGradientBrush(RelativePoint startPoint, RelativePoint endPoint, 
            Color[] colors, double[] offsets)
        {
            var gradientStops = new GradientStops();
            for (int i = 0; i < colors.Length; i++)
                gradientStops.Add(new GradientStop(colors[i], offsets[i]));

            return CreateLinearGradientBrush(startPoint, endPoint, gradientStops);
        }
        
        /// <summary>
        /// Create a <see cref="LinearGradientBrush"/> with variable start-, endpoints and gradient stops.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="endPoint">The end point of the gradient as a <see cref="RelativePoint"/></param>
        /// <param name="gradientStops">The Gradient Stops used for the gradient.</param>
        /// <returns>LinearGradientBrush</returns>
        public static LinearGradientBrush CreateLinearGradientBrush(RelativePoint startPoint, RelativePoint endPoint, 
            GradientStops gradientStops)
        {
            return new LinearGradientBrush()
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                GradientStops = gradientStops
            };
        }

        public static Color DarkenColor(Color color, float amount) => AdjustTint(color, Colors.Black, amount);
        public static Color BrightenColor(Color color, float amount) => AdjustTint(color, Colors.White, amount);
        public static Color AdjustTint(Color color, Color goal, float amount) => color.Lerp(goal, amount);
    }
}