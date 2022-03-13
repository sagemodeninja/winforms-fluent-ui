using System.Drawing.Drawing2D;
using System.Drawing.Text;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Structures;

namespace WinForms.Fluent.UI.Utilities.Helpers
{
    public static class GraphicsHelper
    {
        #region Graphics

        public static Graphics PrimeGraphics(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            return e.Graphics;
        }

        public static GraphicsPath CreateRoundedRectangle(RectangleF rectangle, float radius)
        {
            var diameter = radius * 2;
            var size = new SizeF(diameter, diameter);
            var arc = new RectangleF(rectangle.Location, size);
            var path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            // Top-left arc  
            path.AddArc(arc, 180, 90);

            // Top-right arc  
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom-right arc  
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static Point GetCursorPosition(IntPtr lParam)
        {
            return new Point(GetLowWord(lParam), GetHighWord(lParam));
        }

        public static Color DarkenColor(Color color, float percent)
        {
            var r = (int)(color.R * percent);
            var g = (int)(color.G * percent);
            var b = (int)(color.B * percent);

            return Color.FromArgb(255, r, g, b);
        }

        #endregion

        #region PInvoke

        public static short GetHighWord(IntPtr param)
        {
            return unchecked((short)((long)param >> 16));
        }

        public static short GetLowWord(IntPtr param)
        {
            return unchecked((short)(long)param);
        }

        public static Color GetWindowsAccentColor()
            => GetWindowsAccentColor(false);

        public static Color GetWindowsAccentColor(bool ignoreAlpha)
        {
            var colors = new DWMCOLORIZATIONPARAMS();
            WinApi.DwmGetColorizationParameters(ref colors);

            return Environment.OSVersion.Version.Major >= 10
                ? ParseDwmColorization((int) colors.ColorizationColor, ignoreAlpha)
                : Color.CadetBlue;
        }

        private static Color ParseDwmColorization(int color, bool ignoreAlpha)
        {
            var alpha = ignoreAlpha ? 255 : (byte) ((color >> 24) & 0xff);
            var red = (byte) ((color >> 16) & 0xff);
            var green = (byte) ((color >> 8) & 0xff);
            var blue = (byte) (color & 0xff);

            return Color.FromArgb(alpha, red, green, blue);
        }

        #endregion
    }
}
