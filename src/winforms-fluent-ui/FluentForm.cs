using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Helpers;
using WinForms.Fluent.UI.Utilities.Structures;

// ReSharper disable CommentTypo

namespace WinForms.Fluent.UI
{
    public class FluentForm : Form
    {
        private const int DEFAULT_CAPTION_HEIGHT = 30;
        private const int ICON_SIZE = 16;
        private const int DEFAULT_CAPTION_BUTTON_HEIGHT = 30;
        private const int CAPTION_BUTTON_WIDTH = 46;
        private const int CAPTION_ICON_SIZE = 10;

        private readonly bool _isOsWin10;
        private readonly Color _defaultActiveBorderColor;
        private readonly Color _defaultInactiveBorderColor;

        private int _captionHeight = DEFAULT_CAPTION_HEIGHT;
        private bool _showCaptionIcon = true;
        private Font? _captionTextFont;
        private Rectangle _iconBounds;
        private Rectangle _titleBounds;
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
        private bool _isFormLoaded;
        private bool _isLoadedMaximized;
        private bool _hasBeenRestored;

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

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                CreateCaptionBounds();
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("The caption height of this form.")]
        public int CaptionHeight
        {
            get => _captionHeight;
            set
            {
                if (value == _captionHeight) 
                    return;

                _captionHeight = value;
                CreateCaptionBounds();
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("Indicates whether to show caption icon.")]
        public bool ShowCaptionIcon
        {
            get => _showCaptionIcon;
            set
            {
                if(value == _showCaptionIcon)
                    return;

                _showCaptionIcon = value;
                CreateCaptionBounds();
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("The font used to display caption text.")]
        public Font CaptionTextFont
        {
            get => _captionTextFont ?? Font;
            set
            {
                _captionTextFont = value;
                CreateCaptionBounds();
                Invalidate();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _isFormLoaded = true;
            base.OnLoad(e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                // Remove styles that affect the border size
                if(DesignMode)
                {
                    cp.Style &= ~WinApi.WS_CAPTION;

                    if (FormBorderStyle is not FormBorderStyle.FixedSingle and not FormBorderStyle.FixedToolWindow)
                        cp.Style |= (int)WinApi.WS_BORDER;
                }

                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (!WinApi.DwmIsCompositionEnabled())
            {
                base.WndProc(ref m);
                return;
            }

            // Creation/Trigger WM_NCCALCSIZE.
            if (m.Msg == WinApi.WM_CREATE)
            {
                WinApi.GetWindowRect(m.HWnd, out var rect);

                WinApi.SetWindowPos(m.HWnd,
                    IntPtr.Zero,
                    rect.Left,
                    rect.Top,
                    rect.Width,
                    rect.Height - 31,
                    SetWindowPos.FrameChanged);

                m.Result = IntPtr.Zero;
            }

            // Remove standard frame.
            if (m.Msg == WinApi.WM_NCCALCSIZE && m.WParam != IntPtr.Zero)
            {
                // Window position.
                var wPos = WINDOWPLACEMENT.Default;
                WinApi.GetWindowPlacement(m.HWnd, ref wPos);

                if(!_isFormLoaded)
                    _isLoadedMaximized = WindowState == FormWindowState.Maximized;

                _preventResize = WindowState == FormWindowState.Minimized || WindowState == FormWindowState.Maximized && !(_isLoadedMaximized && !_hasBeenRestored);
                var isMaximized = wPos.ShowCmd == ShowWindowCommands.ShowMaximized;
                var sizeParams = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));

                if (isMaximized || DesignMode)
                    sizeParams.rgrc[0].Top += 8;

                if (FormBorderStyle.ToString().Contains("Fixed") && DesignMode)
                {
                    sizeParams.rgrc[0].Top -= 8;
                }
                else
                {
                    sizeParams.rgrc[0].Left += 8;
                    sizeParams.rgrc[0].Right -= 8;
                    sizeParams.rgrc[0].Bottom -= 8;
                }

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
                var handle = false;
                switch ((int) m.WParam)
                {
                    case WinApi.HTMINBUTTON:
                        WindowState = FormWindowState.Minimized;
                        handle = true;
                        break;
                    case WinApi.HTMAXBUTTON:
                        if (WindowState == FormWindowState.Maximized)
                        {
                            WinApi.ShowWindow(m.HWnd, WinApi.SW_RESTORE);
                            _hasBeenRestored = true;
                        }
                        else
                        {
                            WindowState = FormWindowState.Maximized;
                        }

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
            if (m.Msg == WinApi.WM_NCACTIVATE)
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
            if (cursorPosition.Y >= rcWindow.Top && cursorPosition.Y < rcWindow.Top + _captionHeight)
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

        private void CreateCaptionBounds()
        {
            // Icon.
            var captionLeftCorner = new Point(ICON_SIZE, (_captionHeight - ICON_SIZE) / 2);
            _iconBounds = new Rectangle(captionLeftCorner, new Size(ICON_SIZE, ICON_SIZE));

            // Title.
            using var graphics = CreateGraphics();
            var titleSize = TextRenderer.MeasureText(graphics, Text, CaptionTextFont, Size.Empty, TextFormatFlags.NoPadding);
            var titleLocation = _showCaptionIcon ? new Point(_iconBounds.Right + ICON_SIZE, (_captionHeight - titleSize.Height) / 2) : captionLeftCorner;
            
            _titleBounds = new Rectangle(titleLocation, titleSize);
        }

        private void CreateCaptionButtonBounds(int clientRight)
        {
            var captionSize = new Size(CAPTION_BUTTON_WIDTH, DEFAULT_CAPTION_BUTTON_HEIGHT - _topBorderOffset);

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

            // Guide line for non-client area boundaries during design.
            if (DesignMode)
            {
                var guidePen = new Pen(Color.Gray, 1);
                guidePen.DashStyle = DashStyle.Dash;

                graphics.DrawRectangle(guidePen, new Rectangle(Point.Empty, new Size(ClientSize.Width - 1, ClientSize.Height - 1)));
                graphics.DrawLine(guidePen, new Point(0, _captionHeight), new Point(ClientRectangle.Right, _captionHeight));
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

            // Caption title.
            if(_showCaptionIcon)
            {
                graphics.DrawIcon(Icon, _iconBounds);
            }
            
            if (!string.IsNullOrEmpty(Text))
            {
                graphics.FillRectangle(backgroundBrush, _titleBounds);
                TextRenderer.DrawText(graphics, Text, CaptionTextFont, _titleBounds.Location, ForeColor, TextFormatFlags.NoPadding);
            }

            // Caption button icons.
            var glyphFont = SegoeFluentIcons.CreateFont(CAPTION_ICON_SIZE);
            var glyphColor = Color.FromArgb(23, 23, 23);
            var disabledGlyphColor = Color.FromArgb(199, 192, 192);

            var glyphXOffset = (CAPTION_BUTTON_WIDTH - CAPTION_ICON_SIZE) / 2;
            var glyphYOffset = (DEFAULT_CAPTION_BUTTON_HEIGHT - CAPTION_ICON_SIZE) / 2;
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
}