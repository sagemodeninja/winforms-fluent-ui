using System;
using System.Drawing;

namespace WinForms.Fluent.UI.Extensions
{
    public static class ColorExtensions
	{
        public static bool IsDark(this Color color)
        {
			var luminance = (color.R * 0.2126) + (color.G * 0.7152) + (color.B * 0.0722);
			var threshold = 255 / 2;

			return luminance < threshold;
        }

        public static Color AlphaBlend(this Color baseColor, Color overlayColor, float alpha)
        {
            var a = (byte)(255 * alpha);
            var red = Blend(baseColor.R, overlayColor.R, a);
            var blue = Blend(baseColor.R, overlayColor.R, a);
            var green = Blend(baseColor.R, overlayColor.R, a);

            return Color.FromArgb(red, blue, green);
        }

		private static int Blend(byte baseValue, byte overlayValue, float alpha)
            => (int) Math.Round((overlayValue * alpha / 255) + (baseValue * (255 - alpha) / 255));
	}
}
