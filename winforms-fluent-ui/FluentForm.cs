using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
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

    private Rectangle _minimizeBounds;
    private Rectangle _maximizeBounds;
    private Rectangle _closeBounds;

    private Color _osAccentColor;

    private int _lastKnownHitResult;
    private int _lastKnownWidth;

    public FluentForm()
    {
        _osAccentColor = GraphicsHelper.GetWindowsAccentColor(true);
    }
    
    protected override void WndProc(ref Message m)
    {
        if (WinApi.DwmIsCompositionEnabled())
        {
            // Creation/Trigger WM_NCCALCSIZE.
            if (m.Msg == WinApi.WM_CREATE)
            {
                // Remove border?
                //var lStyle = (int)WinApi.GetWindowLongPtr(m.HWnd, WinApi.GWL_STYLE);
                //lStyle &= ~(WinApi.WS_CAPTION | WinApi.WS_THICKFRAME | WinApi.WS_MINIMIZEBOX | WinApi.WS_MAXIMIZEBOX | WinApi.WS_SYSMENU);
                //WinApi.SetWindowLongPtr(new HandleRef(this, m.HWnd), WinApi.GWL_STYLE, (IntPtr)lStyle);

                WinApi.GetWindowRect(m.HWnd, out var rect);

                WinApi.SetWindowPos(m.HWnd,
                    IntPtr.Zero,
                    rect.Left,
                    rect.Top,
                    rect.Width,
                    rect.Height,
                    SetWindowPos.FrameChanged);
                //SetWindowPos.FrameChanged | SetWindowPos.NoMove | SetWindowPos.NoSize | SetWindowPos.NoZOrder | SetWindowPos.NoOwnerZOrder);

                m.Result = IntPtr.Zero;
            }

            // Activation/Extend window frame.
            if (m.Msg == WinApi.WM_ACTIVATE)
            {
                var margins = new MARGINS
                {
                    leftWidth = 0,
                    rightWidth = 0,
                    bottomHeight = 0,
                    topHeight = 0
                };

                WinApi.DwmExtendFrameIntoClientArea(m.HWnd, ref margins);

                m.Result = IntPtr.Zero;
            }
            
            // Calculate size/Remove standard frame.
            if (m.Msg == WinApi.WM_NCCALCSIZE && m.WParam != IntPtr.Zero)
            {
                // Window position.
                var wPos = WINDOWPLACEMENT.Default;
                WinApi.GetWindowPlacement(m.HWnd, ref wPos);

                var sizeParams = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));

                if (wPos.ShowCmd == ShowWindowCommands.ShowMaximized)
                    sizeParams.rgrc[0].Top += 8;
                
                sizeParams.rgrc[0].Left += 8;
                sizeParams.rgrc[0].Right -= 8;
                sizeParams.rgrc[0].Bottom -= 8;

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

            // Painting/Paint custom caption.
            if (m.Msg == WinApi.WM_PAINT)
            {
                WinApi.BeginPaint(m.HWnd, out var paintStruct);

                PaintCustomCaption();

                WinApi.EndPaint(m.HWnd, ref paintStruct);
            }

            // Non-client click/Handle custom caption.
            if (m.Msg == WinApi.WM_NCLBUTTONDOWN)
            {
                var handle = false;
                
                switch ((int)m.WParam)
                {
                    case WinApi.HTMINBUTTON:
                        WindowState = FormWindowState.Minimized;
                        handle = true;
                        break;
                    case WinApi.HTMAXBUTTON:
                        WindowState = WindowState == FormWindowState.Maximized
                            ? FormWindowState.Normal
                            : FormWindowState.Maximized;

                        handle = true;
                        break;
                    case WinApi.HTCLOSE:
                        Close();
                        handle = true;
                        break;
                }

                if (handle)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }

            // Hit test.
            var result = IntPtr.Zero;
            if (WinApi.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref result))
            {
                m.Result = result;
                return;
            }

            if (m.Msg == WinApi.WM_NCHITTEST && result == IntPtr.Zero)
            {
                var hitResult = HitTestNca(m.HWnd, m.LParam);

                m.Result = (IntPtr)hitResult;

                if (hitResult != WinApi.HTNOWHERE)
                    return;
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

                if (_minimizeBounds.Contains(cursorLocalPosition))
                    hitResult = WinApi.HTMINBUTTON;

                if (_maximizeBounds.Contains(cursorLocalPosition))
                    hitResult = WinApi.HTMAXBUTTON;

                if (_closeBounds.Contains(cursorLocalPosition))
                    hitResult = WinApi.HTCLOSE;

                if (hitResult != 0)
                {
                    if (_lastKnownHitResult != hitResult)
                    {
                        var paintBounds = new Rectangle(_minimizeBounds.Left, 3,
                            _closeBounds.Right - _minimizeBounds.Left, DEFAULT_CAPTION_HEIGHT);
                        Invalidate(paintBounds);
                    }

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
            Invalidate();
        }

        return hitTestGrid[row, column];
    }

    private void CreateCaptionButtonBounds(int clientRight)
    {
        var captionSize = new Size(CAPTION_BUTTON_WIDTH, DEFAULT_CAPTION_HEIGHT);
        
        var minimizeLocation = new POINT(clientRight - CAPTION_BUTTON_WIDTH * 3, 0);
        var maximizeLocation = new Point(clientRight - CAPTION_BUTTON_WIDTH * 2, 0);
        var closeLocation = new Point(clientRight - CAPTION_BUTTON_WIDTH, 0);

        _minimizeBounds = new Rectangle(minimizeLocation, captionSize);
        _maximizeBounds = new Rectangle(maximizeLocation, captionSize);
        _closeBounds = new Rectangle(closeLocation, captionSize);
    }

    private void PaintCustomCaption()
    {
        var graphics = CreateGraphics();

        graphics.FillRectangle(Brushes.White, new Rectangle(Point.Empty, new Size(ClientRectangle.Width, DEFAULT_CAPTION_HEIGHT)));

        //var osBuild = Environment.OSVersion.Version.Build;
        //if (osBuild >= 2200) // Note: Windows 11 build no starts @ 2200.
        //{
        //    var borderPen = new Pen(_osAccentColor, 1);
        //    var borderY = DesignMode ? 0 : 2;
        //    graphics.DrawLine(borderPen, 0, borderY, ClientRectangle.Right, borderY);
        //}

        if (DesignMode)
        {
            var guidePen = new Pen(Color.LightGray, 1);
            guidePen.DashStyle = DashStyle.Dash;

            graphics.DrawLine(guidePen, new Point(0, DEFAULT_CAPTION_HEIGHT), new Point(ClientRectangle.Right, DEFAULT_CAPTION_HEIGHT));
            guidePen.Dispose();
        }

        //graphics.DrawRectangle(Pens.Red, _minimizeBounds);
        //graphics.DrawRectangle(Pens.Red, _maximizeBounds);
        //graphics.DrawRectangle(Pens.Red, _closeBounds);

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

        var glyphFont = SegoeFluentIcons.CreateFont(CAPTION_ICON_SIZE);
        var glyphColor = Color.FromArgb(23, 23, 23);

        var glyphXOffset = (CAPTION_BUTTON_WIDTH - CAPTION_ICON_SIZE) / 2;
        var glyphYOffset = (DEFAULT_CAPTION_HEIGHT - CAPTION_ICON_SIZE) / 2;
        var minimizeOffset = _minimizeBounds.Left + glyphXOffset;
        var maximizeOffset = _maximizeBounds.Left + glyphXOffset;
        var closeOffset = _closeBounds.Left + glyphXOffset;

        var minimizeGlyphLocation = new Point(minimizeOffset, glyphYOffset);
        var maximizeGlyphLocation = new Point(maximizeOffset, glyphYOffset);
        var closeGlyphLocation = new Point(closeOffset, glyphYOffset);

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_MINIMIZE,
            glyphFont,
            minimizeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        TextRenderer.DrawText(graphics,
            WindowState == FormWindowState.Maximized ? SegoeFluentIcons.CHROME_RESTORE : SegoeFluentIcons.CHROME_MAXIMIZE,
            glyphFont,
            maximizeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        // Change color for highlighted close button.
        if (_lastKnownHitResult == WinApi.HTCLOSE)
            glyphColor = Color.White;

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_CLOSE,
            glyphFont,
            closeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        // Cleanup.
        highlightBrush.Dispose();
        glyphFont.Dispose();
        graphics.Dispose();
    }
}