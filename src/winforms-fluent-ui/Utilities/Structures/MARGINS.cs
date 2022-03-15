using System.Runtime.InteropServices;

// ReSharper disable once InconsistentNaming

namespace WinForms.Fluent.UI.Utilities.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct MARGINS
{
    public int leftWidth;
    public int rightWidth;
    public int topHeight;
    public int bottomHeight;
}