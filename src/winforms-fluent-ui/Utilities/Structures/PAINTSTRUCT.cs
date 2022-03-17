using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable once InconsistentNaming

namespace WinForms.Fluent.UI.Utilities.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct PAINTSTRUCT
{
    public IntPtr hdc;
    public bool fErase;
    public RECT rcPaint;
    public bool fRestore;
    public bool fIncUpdate;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] 
    public byte[] rgbReserved;
}