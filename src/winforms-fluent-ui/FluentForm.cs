using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Helpers;
using WinForms.Fluent.UI.Utilities.Structures;

// ReSharper disable CommentTypo

namespace WinForms.Fluent.UI;

public class FluentForm : Form
{
    public const int DEFAULT_CAPTION_HEIGHT = 30;
    private const int CAPTION_BUTTON_WIDTH = 46;
    private const int CAPTION_ICON_SIZE = 10;

    private readonly bool _isOsWin10;
    private readonly Color _defaultActiveBorderColor;
    private readonly Color _defaultInactiveBorderColor;

    private Rectangle _captionBounds;
    private Rectangle _minimizeBounds;
    private Rectangle _maximizeBounds;
    private Rectangle _closeBounds;

    private Color _osAccentColor;

    private int _lastKnownHitResult;
    private int _lastKnownWidth;
    private int _topBorderOffset;
    private bool _isColorPrevalent;
    private bool _formIsActive;
    private bool _preventResize;

    protected FluentForm()
    {
        // Note: Windows 11 build no starts @ 22000.
        _isOsWin10 = Environment.OSVersion.Version.Build < 22000;

        // Default border colors.
        _defaultActiveBorderColor = Color.FromArgb(112, 112, 112);
        _defaultInactiveBorderColor = Color.FromArgb(170, 170, 170);

        // Color prevalence settings.
        var windowsDwmSettings = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
        if (windowsDwmSettings != null)
        {
            _isColorPrevalent = (int)(windowsDwmSettings.GetValue("ColorPrevalence") ?? 0) == 1;
            windowsDwmSettings.Close();
        }

        _osAccentColor = GraphicsHelper.GetWindowsAccentColor(true);
        _formIsActive = true;
    }

    protected override void WndProc(ref Message m)
    {
        if (WinApi.DwmIsCompositionEnabled())
        {
            // Remove standard frame.
            if (m.Msg == WinApi.WM_NCCALCSIZE && m.WParam != IntPtr.Zero)
            {
                // Window position.
                var wPos = WINDOWPLACEMENT.Default;
                WinApi.GetWindowPlacement(m.HWnd, ref wPos);
                
                _preventResize = WindowState is FormWindowState.Minimized or FormWindowState.Maximized;
                var isMaximized = wPos.ShowCmd == ShowWindowCommands.ShowMaximized;
                var sizeParams = (NCCALCSIZE_PARAMS) m.GetLParam(typeof(NCCALCSIZE_PARAMS));

                if (isMaximized)
                    sizeParams.rgrc[0].Top += 8;
                
                sizeParams.rgrc[0].Left += 8;
                sizeParams.rgrc[0].Right -= 8;
                sizeParams.rgrc[0].Bottom -= 8;

                _topBorderOffset = _isOsWin10 && !isMaximized ? 1 : 0;

                var width = sizeParams.rgrc[0].Width;
                CreateCaptionButtonBounds(width);

                if (_lastKnownWidth != width)
                    Invalidate();

                _lastKnownWidth = width;
                _lastKnownHitResult = 0;

                Marshal.StructureToPtr(sizeParams, m.LParam, true);
                m.Result = IntPtr.Zero;
                return;
            }

            // Paint custom caption buttons.
            if (m.Msg == WinApi.WM_PAINT)
            {
                WinApi.BeginPaint(m.HWnd, out var paintStruct);

                PaintCustomCaption();

                WinApi.EndPaint(m.HWnd, ref paintStruct);
            }
            
            // Handle custom caption buttons.
            if (m.Msg == WinApi.WM_NCLBUTTONDOWN)
            {
                switch ((int) m.WParam)
                {
                    case WinApi.HTMINBUTTON:
                        WindowState = FormWindowState.Minimized;
                        m.Result = IntPtr.Zero;
                        break;
                    case WinApi.HTMAXBUTTON:
                        WindowState = WindowState == FormWindowState.Maximized
                            ? FormWindowState.Normal
                            : FormWindowState.Maximized;

                        m.Result = IntPtr.Zero;
                        break;
                    case WinApi.HTCLOSE:
                        Close();
                        m.Result = IntPtr.Zero;
                        break;
                }
            }
            
            // Hit test.
            if (m.Msg == WinApi.WM_NCHITTEST)
            {
                var hitResult = HitTestNca(m.HWnd, m.LParam);

                m.Result = (IntPtr) hitResult;

                if (hitResult != WinApi.HTNOWHERE)
                    return;
            }

            // Prevent second-resizing when restoring window.
            if (m.Msg == WinApi.WM_WINDOWPOSCHANGING && _preventResize)
            {
                var winPos = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));

                // Set flags.
                winPos.flags |= 0x0001;
                winPos.flags &= ~(uint)0x0020;

                // Commit.
                Marshal.StructureToPtr(winPos, m.LParam, true);
                m.Result = IntPtr.Zero;
                _preventResize = false;
                return;
            }

            // Respond to windows activation.
            if (m.Msg == WinApi.WM_NCACTIVATE && _isOsWin10)
            {
                _formIsActive = m.WParam != IntPtr.Zero;
                Invalidate();
            }

            // Respond to personalization settings changes.
            if (m.Msg == WinApi.WM_SETTINGCHANGE)
            {
                var windowsDwmSettings = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
                if (windowsDwmSettings != null)
                {
                    _isColorPrevalent = (int) (windowsDwmSettings.GetValue("ColorPrevalence") ?? 0) == 1;
                    windowsDwmSettings.Close();
                }

                _osAccentColor = GraphicsHelper.GetWindowsAccentColor(true);
                Invalidate();
            }
        }

        base.WndProc(ref m);
    }

    private int HitTestNca(IntPtr handle, IntPtr lParam)
    {
        var row = 1;
        var column = 1;
        var isOnResizeBorder = false;
        var rcFrame = new RECT(0);

        // Bounds.
        var cursorPosition = GraphicsHelper.GetCursorPosition(lParam);
        WinApi.GetWindowRect(handle, out var rcWindow);
        WinApi.AdjustWindowRectEx(ref rcFrame, WinApi.WS_OVERLAPPEDWINDOW & ~WinApi.WS_CAPTION, false);

        // Vertical (Y).
        if (cursorPosition.Y >= rcWindow.Top && cursorPosition.Y < rcWindow.Top + DEFAULT_CAPTION_HEIGHT)
        {
            isOnResizeBorder = (cursorPosition.Y < (rcWindow.Top - rcFrame.Top));

            if (!isOnResizeBorder)
            {
                // Caption buttons.
                var cursorLocalPosition = PointToClient(cursorPosition);
                var hitResult = 0;

                if (_minimizeBounds.Contains(cursorLocalPosition) && MinimizeBox)
                    hitResult = WinApi.HTMINBUTTON;

                if (_maximizeBounds.Contains(cursorLocalPosition) && MaximizeBox)
                    hitResult = WinApi.HTMAXBUTTON;

                if (_closeBounds.Contains(cursorLocalPosition))
                    hitResult = WinApi.HTCLOSE;

                if (hitResult != 0)
                {
                    if (_lastKnownHitResult != hitResult)
                        Invalidate(_captionBounds);

                    _lastKnownHitResult = hitResult;
                    return hitResult;
                }
            }

            row = 0;
        }
        else if (cursorPosition.Y < rcWindow.Bottom && cursorPosition.Y >= rcWindow.Bottom)
        {
            row = 2;
        }

        // Horizontal (X).
        if (cursorPosition.X >= rcWindow.Left && cursorPosition.X < rcWindow.Left)
        {
            column = 0;
        }
        else if (cursorPosition.X < rcWindow.Right && cursorPosition.X >= rcWindow.Right)
        {
            column = 2;
        }

        var hitTestGrid = new[,]
        {
            {WinApi.HT_TOPLEFT, isOnResizeBorder ? WinApi.HT_TOP : WinApi.HTCAPTION, WinApi.HT_TOPRIGHT},
            {WinApi.HT_LEFT, WinApi.HTNOWHERE, WinApi.HT_RIGHT},
            {WinApi.HT_BOTTOMLEFT, WinApi.HT_BOTTOM, WinApi.HT_BOTTOMRIGHT}
        };

        // Remove caption button highlight.
        if (_lastKnownHitResult != 0)
        {
            _lastKnownHitResult = 0;
            Invalidate(_captionBounds);
        }

        return hitTestGrid[row, column];
    }

    private void CreateCaptionButtonBounds(int clientRight)
    {
        var captionSize = new Size(CAPTION_BUTTON_WIDTH, DEFAULT_CAPTION_HEIGHT - _topBorderOffset);

        var minimizeLocation = new POINT(clientRight - CAPTION_BUTTON_WIDTH * 3, _topBorderOffset);
        var maximizeLocation = new Point(clientRight - CAPTION_BUTTON_WIDTH * 2, _topBorderOffset);
        var closeLocation = new Point(clientRight - CAPTION_BUTTON_WIDTH, _topBorderOffset);

        _captionBounds = new Rectangle(minimizeLocation.X, _topBorderOffset,
            clientRight, captionSize.Height);
        _minimizeBounds = new Rectangle(minimizeLocation, captionSize);
        _maximizeBounds = new Rectangle(maximizeLocation, captionSize);
        _closeBounds = new Rectangle(closeLocation, captionSize);
    }

    private void PaintCustomCaption()
    {
        var graphics = CreateGraphics();
        
        // Draw background for caption buttons.
        var backgroundBrush = new SolidBrush(BackColor);
        graphics.FillRectangle(backgroundBrush, _captionBounds);

        // Draw top border for Windows 10 devices.
        if (_isOsWin10 && WindowState != FormWindowState.Maximized)
        {
            var borderColor = !_isColorPrevalent ? _defaultActiveBorderColor : _osAccentColor;

            if (!_formIsActive)
                borderColor = _defaultInactiveBorderColor;

            var borderPen = new Pen(borderColor, 1);

            graphics.DrawLine(borderPen, 0, 0, ClientRectangle.Right, 0);
            borderPen.Dispose();
        }

        // Guide line for title bar boundaries during design.
        if (DesignMode)
        {
            var guidePen = new Pen(Color.LightGray, 1);
            guidePen.DashStyle = DashStyle.Dash;

            graphics.DrawLine(guidePen, new Point(0, DEFAULT_CAPTION_HEIGHT), new Point(ClientRectangle.Right, DEFAULT_CAPTION_HEIGHT));
            guidePen.Dispose();
        }

        // Caption highlight.
        var highlightColor = Color.FromArgb(233, 233, 233);
        var highlightBrush = new SolidBrush(highlightColor);

        switch (_lastKnownHitResult)
        {
            case WinApi.HTMINBUTTON:
                graphics.FillRectangle(highlightBrush, _minimizeBounds);
                break;
            case WinApi.HTMAXBUTTON:
                graphics.FillRectangle(highlightBrush, _maximizeBounds);
                break;
            case WinApi.HTCLOSE:
                highlightColor = Color.FromArgb(232, 17, 35);
                highlightBrush = new SolidBrush(highlightColor);

                graphics.FillRectangle(highlightBrush, _closeBounds);
                break;
        }

        // Caption button icons.
        var glyphFont = SegoeFluentIcons.CreateFont(CAPTION_ICON_SIZE);
        var glyphColor = Color.FromArgb(23, 23, 23);
        var disabledGlyphColor = Color.FromArgb(199, 192, 192);

        var glyphXOffset = (CAPTION_BUTTON_WIDTH - CAPTION_ICON_SIZE) / 2;
        var glyphYOffset = (DEFAULT_CAPTION_HEIGHT - CAPTION_ICON_SIZE) / 2;
        var minimizeOffset = _minimizeBounds.Left + glyphXOffset;
        var maximizeOffset = _maximizeBounds.Left + glyphXOffset;
        var closeOffset = _closeBounds.Left + glyphXOffset;

        var minimizeGlyphLocation = new Point(minimizeOffset, glyphYOffset);
        var maximizeGlyphLocation = new Point(maximizeOffset, glyphYOffset);
        var closeGlyphLocation = new Point(closeOffset, glyphYOffset);
        
        if(MinimizeBox || MaximizeBox)
        {
            TextRenderer.DrawText(graphics,
                SegoeFluentIcons.CHROME_MINIMIZE,
                glyphFont,
                minimizeGlyphLocation,
                MinimizeBox ? glyphColor : disabledGlyphColor,
                TextFormatFlags.NoPadding);

            TextRenderer.DrawText(graphics,
                WindowState == FormWindowState.Maximized
                    ? SegoeFluentIcons.CHROME_RESTORE
                    : SegoeFluentIcons.CHROME_MAXIMIZE,
                glyphFont,
                maximizeGlyphLocation,
                MaximizeBox ? glyphColor : disabledGlyphColor,
                TextFormatFlags.NoPadding);
        }

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_CLOSE,
            glyphFont,
            closeGlyphLocation,
            _lastKnownHitResult != WinApi.HTCLOSE ? glyphColor : Color.White,
            TextFormatFlags.NoPadding);

        // Cleanup.
        backgroundBrush.Dispose();
        highlightBrush.Dispose();
        glyphFont.Dispose();
        graphics.Dispose();
    }
}