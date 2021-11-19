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