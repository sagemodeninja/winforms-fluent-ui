using System.Runtime.InteropServices;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Structures;

// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace WinForms.Fluent.UI.Utilities.Classes
{
    public static class WinApi
    {
        #region WINDOW MESSAGES
        
        public const int WM_CREATE = 0x0001;
        public const int WM_DESTROY = 0x0002;
        public const int WM_PAINT = 0x000f;
        public const int WM_TIMER = 0x0113;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_SETTINGCHANGE = 0x001A;
        public const int WM_NCACTIVATE = 0x0086;

        #endregion

        #region IMPORTS

        // DWM API.
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        public static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS colors);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(out bool enabled);

        // User 32.
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(IntPtr hWnd, IntPtr nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(IntPtr hWnd, IntPtr uIDEvent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPos u);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        #endregion

        #region FLAGS

        // WM_NCHITTEST return values.
        public const int HTNOWHERE = 0;
        public const int HT_TOPLEFT = 13;
        public const int HT_TOP = 12;
        public const int HTCAPTION = 2;
        public const int HT_TOPRIGHT = 14;
        public const int HT_RIGHT = 11;
        public const int HT_BOTTOMRIGHT = 17;
        public const int HT_BOTTOM = 15;
        public const int HT_BOTTOMLEFT = 16;
        public const int HT_LEFT = 10;
        public const int HTMINBUTTON = 8;
        public const int HTMAXBUTTON = 9;
        public const int HTCLOSE = 20;

        // Window styles.
        public const long WS_BORDER = 0x00800000L;
        public const int WS_OVERLAPPED = 0x00000000;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_SYSMENU = 0x00080000;
        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_MINIMIZEBOX = 0x00020000;
        public const long WS_MAXIMIZEBOX = 0x00010000L;
        public const int WS_OVERLAPPEDWINDOW =
            WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | (int)WS_MAXIMIZEBOX;

        // Others.

        #endregion

        #region LOCAL METHODS

        public static bool DwmIsCompositionEnabled()
        {
            var result = DwmIsCompositionEnabled(out var enabled);
            return result == 0 && enabled;
        }

        #endregion
    }
}
