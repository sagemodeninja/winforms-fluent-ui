using System;
using System.Runtime.InteropServices;

// ReSharper disable once InconsistentNaming

namespace WinForms.Fluent.UI.Utilities.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }
}