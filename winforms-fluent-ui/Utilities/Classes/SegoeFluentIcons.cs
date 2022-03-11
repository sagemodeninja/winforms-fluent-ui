namespace WinForms.Fluent.UI.Utilities.Classes
{
    public static class SegoeFluentIcons
    {
        private const string FONT_FAMILY = "Segoe Fluent Icons";

        public const string CHECKBOX_INDETERMINATE = "\ue73c";
        public const string CHECK_MARK = "\ue73e";

        public static Font CreateFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(FONT_FAMILY, size, style, GraphicsUnit.Pixel);
        }
    }
}
