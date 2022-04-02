using System.Runtime.InteropServices;

// ReSharper disable once InconsistentNaming

namespace WinForms.Fluent.UI.Utilities.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public RECT[] rgrc;
        public WINDOWPOS lppos;
    }
}