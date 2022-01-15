using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;

namespace GradeManagement.ExtensionCollection
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial class Extensions
    {
        /// <summary>
        /// Linear interpolation between two colors.
        /// </summary>
        /// <param name="color">Start value (a)</param>
        /// <param name="to">End value (b)</param>
        /// <param name="amount">Amount (t)</param>
        /// <returns>Linearly interpolated value.</returns>
        public static Color Lerp(this Color color, Color to, float amount)
        {
            byte a = (byte) ((float) color.A).Lerp(to.A, amount),
                r = (byte) ((float) color.R).Lerp(to.R, amount),
                g = (byte) ((float) color.G).Lerp(to.G, amount),
                b = (byte) ((float) color.B).Lerp(to.B, amount);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Darkens the specified color by <see cref="amount"/>
        /// </summary>
        /// <param name="color">The color to be darkened.</param>
        /// <param name="amount">How much darker should the color become?</param>
        /// <returns>The adjusted color.</returns>
        public static Color DarkenColor(this Color color, float amount) => AdjustTint(color, Colors.Black, amount);
        
        /// <summary>
        /// Brightens the specified color by <see cref="amount"/>
        /// </summary>
        /// <param name="color">The color to be brightened.</param>
        /// <param name="amount">How much brighter should the color become?</param>
        /// <returns>The adjusted color.</returns>
        public static Color BrightenColor(this Color color, float amount) => AdjustTint(color, Colors.White, amount);
        
        /// <summary>
        /// Adjust the specified color by <see cref="amount"/> to be closer to the <see cref="goal"/> color.
        /// </summary>
        /// <param name="color">The color to be adjusted.</param>
        /// <param name="goal">The goal color, to which the <see cref="color"/> tends to go.</param>
        /// <param name="amount">How close should the <see cref="color"/> be to the <see cref="goal"/> color?</param>
        /// <returns>The adjusted color.</returns>
        public static Color AdjustTint(this Color color, Color goal, float amount) => color.Lerp(goal, amount);

        /// <summary>
        /// Return the hexadecimal representation of a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color to use for the hexadecimal representation.</param>
        /// <returns>The hexadecimal representation of this <see cref="Color"/></returns>
        public static string ToHexString(this Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}";

        public static Color WithR(this Color color, byte r) => 
            new (color.A, r, color.G, color.B);

        public static Color WithRG(this Color color, byte r, byte g) 
            => new(color.A, r, g, color.B);

        public static Color WithRB(this Color color, byte r, byte b) 
            => new(color.A, r, color.G, b);

        public static Color WithRA(this Color color, byte r, byte a) 
            => new(a, r, color.G, color.B);

        public static Color WithRGB(this Color color, byte r, byte g, byte b) 
            => new(color.A, r, g, b);

        public static Color WithRGA(this Color color, byte r, byte g, byte a) 
            => new(a, r, g, color.B);

        public static Color WithRBA(this Color color, byte r, byte b, byte a) 
            => new(a, r, color.G, b);

        public static Color WithG(this Color color, byte g) 
            => new(color.A, color.R, g, color.B);

        public static Color WithGB(this Color color, byte g, byte b) 
            => new(color.A, color.R, g, b);

        public static Color WithGA(this Color color, byte g, byte a) 
            => new(a, color.R, g, color.B);

        public static Color WithGBA(this Color color, byte g, byte b, byte a) 
            => new(a, color.R, g, b);

        public static Color WithB(this Color color, byte b) 
            => new(color.A, color.R, color.G, b);

        public static Color WithBA(this Color color, byte b, byte a) 
            => new(a, color.R, color.G, b);

        public static Color WithA(this Color color, byte a) 
            => new(a, color.R, color.G, color.B);

        public static Color With(this Color color, byte r, byte g, byte b, byte a) 
            => new(a, r, g, b);
    }
}