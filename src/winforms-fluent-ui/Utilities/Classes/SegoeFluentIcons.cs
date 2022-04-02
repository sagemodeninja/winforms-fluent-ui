using System.Drawing;

namespace WinForms.Fluent.UI.Utilities.Classes
{
    public static class SegoeFluentIcons
    {
        private const string FONT_FAMILY = "Segoe Fluent Icons";
        private const string FALLBACK_FONT_FAMILY = "Segoe MDL2 Assets";
        
        public const string CHECK_MARK = "\ue73e";
        public const string CHROME_CLOSE = "\ue8bb";
        public const string CHROME_MINIMIZE = "\ue921";
        public const string CHROME_MAXIMIZE = "\ue922";
        public const string CHROME_RESTORE = "\ue923";

        public static Font CreateFont(float size, FontStyle style = FontStyle.Regular)
        {
            var font = new Font(FONT_FAMILY, size, style, GraphicsUnit.Pixel);

            if(font.Name != FONT_FAMILY)
                font = new Font(FALLBACK_FONT_FAMILY, size, style, GraphicsUnit.Pixel);

            return font;
        }
    }
}
