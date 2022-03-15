using System.ComponentModel;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI;

public class CaptionButton : Control
{
    private const int CAPTION_BUTTON_WIDTH = 46;
    private const float ICON_SIZE = 10f;

    private Rectangle _minimizeBounds;
    private Rectangle _maximizeBounds;
    private Rectangle _closeBounds;

    public CaptionButton()
    {
        SetStyle(
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true
        );

        var captionSize = new Size(CAPTION_BUTTON_WIDTH, FluentForm.DEFAULT_CAPTION_HEIGHT);
        var maximizeLocation = new Point(CAPTION_BUTTON_WIDTH, 0);
        var closeLocation = new Point(CAPTION_BUTTON_WIDTH * 2, 0);

        _minimizeBounds = new Rectangle(Point.Empty, captionSize);
        _maximizeBounds = new Rectangle(maximizeLocation, captionSize);
        _closeBounds = new Rectangle(closeLocation, captionSize);
    }

    protected override Size DefaultSize
        => new(CAPTION_BUTTON_WIDTH * 3, FluentForm.DEFAULT_CAPTION_HEIGHT);

    protected override void SetBoundsCore(int x, int y,
        int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, CAPTION_BUTTON_WIDTH * 3, FluentForm.DEFAULT_CAPTION_HEIGHT, specified);
    }

    public delegate void HitTestTriggeredDelegate(object sender, MouseEventArgs e);

    [Category("Behavior"),
     Description("Occurs when WM_NCHITTEST message is sent to this control.")]
    public event HitTestTriggeredDelegate HitTestTriggered;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var graphics = GraphicsHelper.PrimeGraphics(e);
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        if(DesignMode)
            graphics.DrawRectangle(Pens.LightGray, ClientRectangle);

        graphics.DrawRectangle(Pens.Red, _minimizeBounds);
        graphics.DrawRectangle(Pens.Red, _maximizeBounds);
        graphics.DrawRectangle(Pens.Red, _closeBounds);

        var glyphFont = SegoeFluentIcons.CreateFont(ICON_SIZE);
        var glyphColor = Color.FromArgb(23, 23, 23);

        var glyphOffset = (FluentForm.DEFAULT_CAPTION_HEIGHT - (int)ICON_SIZE) / 2;
        var maximizeOffset = glyphOffset * 3 + (int)ICON_SIZE;
        var closeOffset = maximizeOffset * 2;

        var minimizeGlyphLocation = new Point(glyphOffset, glyphOffset);
        var maximizeGlyphLocation = new Point(maximizeOffset, glyphOffset);
        var closeGlyphLocation = new Point(closeOffset, glyphOffset);

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_MINIMIZE,
            glyphFont,
            minimizeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_MAXIMIZE,
            glyphFont,
            maximizeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        TextRenderer.DrawText(graphics,
            SegoeFluentIcons.CHROME_CLOSE,
            glyphFont,
            closeGlyphLocation,
            glyphColor,
            TextFormatFlags.NoPadding);

        // Cleanup.
        glyphFont.Dispose();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WinApi.WM_NCHITTEST)
        {
            var cursorLocation = GraphicsHelper.GetCursorPosition(m.LParam);
            
            var eventArgs = new MouseEventArgs(MouseButtons.None, 0, cursorLocation.X, cursorLocation.Y, 0);
            HitTestTriggered(this, eventArgs);

            //if (_maximizeBounds.Contains(cursorLocation))
            //{
            //    m.Result = (IntPtr)WinApi.HTMAXBUTTON;

                
            //    return;
            //}
            return;
        }

        base.WndProc(ref m);
    }
}