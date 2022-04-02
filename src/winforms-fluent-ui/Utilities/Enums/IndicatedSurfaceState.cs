using System;

namespace WinForms.Fluent.UI.Utilities.Enums
{
    [Flags]
    public enum IndicatedSurfaceState
    {
        Normal = 0x001,
        Active = 0x002,
        Hovered = 0x004,
        Clicked = 0x008
    }
}
