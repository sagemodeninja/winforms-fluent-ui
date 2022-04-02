using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using DirectN;
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

        /// <summary>
        /// Creates a rounded rectangle of the specified bounds.
        /// </summary>
        /// <param name="rectangle">The bounds of rectangle.</param>
        /// <param name="radius">The radius of the rounded corners.</param>
        /// <returns>An instance of <see cref="GraphicsPath"/>.</returns>
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

        public static void CreateRoundedRectangle(ref ID2D1GeometrySink sink, D2D_SIZE_F bounds, float radius)
        {
            var rightX = bounds.width - radius - 1;
            var bottomY = bounds.height - radius - 1;

            var startPoint = new D2D_POINT_2F(1, radius + 1);
            sink.BeginFigure(startPoint, D2D1_FIGURE_BEGIN.D2D1_FIGURE_BEGIN_FILLED);

            // Top-left arc.
            var arcSegment = CreateArcSegment(radius + 1, 1, radius);
            sink.AddArc(ref arcSegment);

            // Top line.
            sink.AddLine(new D2D_POINT_2F(rightX, 1));

            // Top-right arc.
            arcSegment = CreateArcSegment(bounds.width - 1, radius + 1, radius);
            sink.AddArc(ref arcSegment);

            // Right line.
            sink.AddLine(new D2D_POINT_2F(bounds.width - 1, bottomY));

            // Bottom-right arc.
            arcSegment = CreateArcSegment(rightX, bounds.height - 1, radius);
            sink.AddArc(ref arcSegment);

            // Bottom line.
            sink.AddLine(new D2D_POINT_2F(radius + 1, bounds.height - 1));

            // Bottom-left arc.
            arcSegment = CreateArcSegment(1, bottomY, radius);
            sink.AddArc(ref arcSegment);

            // Left line.
            sink.AddLine(new D2D_POINT_2F(1, radius + 1));

            sink.EndFigure(D2D1_FIGURE_END.D2D1_FIGURE_END_CLOSED);
        }

        private static D2D1_ARC_SEGMENT CreateArcSegment(float x, float y, float radius)
        {
            return new D2D1_ARC_SEGMENT
            {
                point = new D2D_POINT_2F(x, y),
                size = new D2D_SIZE_F(radius, radius),
                rotationAngle = 0,
                sweepDirection =  D2D1_SWEEP_DIRECTION.D2D1_SWEEP_DIRECTION_CLOCKWISE,
                arcSize = D2D1_ARC_SIZE.D2D1_ARC_SIZE_SMALL
            };
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

        public static Color RegistryToColor(object? value)
        {
            var stringValue = value?.ToString();

            if (string.IsNullOrEmpty(stringValue))
                return Color.Red;

            var colors = stringValue.Split(' ')
                                    .Select(int.Parse)
                                    .ToArray();

            return Color.FromArgb(colors[0], colors[1], colors[2]);
        }

        public static _D3DCOLORVALUE ColorToD3dColor(Color originalColor)
        {
            var r = originalColor.R / 255f;
            var g = originalColor.G / 255f;
            var b = originalColor.B / 255f;

            return new _D3DCOLORVALUE(r, g, b);
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
