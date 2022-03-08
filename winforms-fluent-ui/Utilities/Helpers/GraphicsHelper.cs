using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Structures;

namespace WinForms.Fluent.UI.Utilities.Helpers
{
    public static class GraphicsHelper
    {
        public static Color GetWindowsAccentColor()
        {
            var colors = new DWMCOLORIZATIONPARAMS();
            WinApi.DwmGetColorizationParameters(ref colors);

            return Environment.OSVersion.Version.Major >= 10
                ? ParseDwmColorization((int) colors.ColorizationColor)
                : Color.CadetBlue;
        }

        public static short GetHighWord(IntPtr param)
        {
            return unchecked((short)((long)param >> 16));
        }

        public static short GetLowWord(IntPtr param)
        {
            return unchecked((short)(long)param);
        }

        private static Color ParseDwmColorization(int color)
        {
            var alpha = (byte) ((color >> 24) & 0xff);
            var red = (byte) ((color >> 16) & 0xff);
            var green = (byte) ((color >> 8) & 0xff);
            var blue = (byte) (color & 0xff);

            return Color.FromArgb(alpha, red, green, blue);
        }
    }
}
