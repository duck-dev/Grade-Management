using Avalonia.Media;

namespace GradeManagement
{
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
            byte a = (byte)((float)color.A).Lerp(to.A, amount),
                 r = (byte)((float)color.R).Lerp(to.R, amount),
                 g = (byte)((float)color.G).Lerp(to.G, amount),
                 b = (byte)((float)color.B).Lerp(to.B, amount);
            return Color.FromArgb(a, r, g, b);
        }
    }
}