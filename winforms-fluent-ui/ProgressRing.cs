using System.ComponentModel;
using System.Drawing.Drawing2D;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI
{
    public class ProgressRing : Control
    {
        private const float START_ANGLE = -90F;
        private const int ANIMATION_TIMER_ID = 0x001;

        private readonly Color _color;

        private float _value;
        private bool _isIndeterminate;
        private float _maxValue;
        private float _percentage;
        private float _ellipseWidth;
        private float _startAngle;

        public ProgressRing()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            _value = 0;
            _maxValue = 100;
            _percentage = 0;
            _ellipseWidth = 10;
            _startAngle = START_ANGLE;

            // Windows accent color.
            _color = GraphicsHelper.GetWindowsAccentColor();
        }

        [Category("Appearance")]
        [Description("Set the width of ellipse.")]
        public float EllipseWidth
        {
            get => _ellipseWidth;
            set
            {
                _ellipseWidth = value;
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Indicates whether progress represents a known amount of work.")]
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set
            {
                _isIndeterminate = value;
                if (IsIndeterminate)
                {
                    WinApi.SetTimer(Handle, (IntPtr)ANIMATION_TIMER_ID, 1, IntPtr.Zero);
                }
                else
                {
                    WinApi.KillTimer(Handle, (IntPtr) ANIMATION_TIMER_ID);
                }
            }
        }

        [Category("Behavior")]
        [Description("Set the progress max value.")]
        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Set the progress value.")]
        public float Value
        {
            get => _value;
            set
            {
                _value = Math.Max(Math.Min(value, _maxValue), 0);
                Invalidate();
            }
        }

        [Browsable(false)]
        [Description("Gets progress percentage.")]
        public float Percentage => _percentage;

        private static PointF AddPointF(PointF p, float v)
        {
            p.X += v;
            p.Y += v;

            return p;
        }

        private static SizeF AddSizeF(SizeF s, float v)
        {
            s.Width += v;
            s.Height += v;

            return s;
        }

        private static double AngleToDegrees(float a)
        {
            return a * Math.PI / 180;
        }

        private static float GetPointX(float r, float a)
        {
            var degrees = AngleToDegrees(a);
            return r * (float)Math.Cos(degrees) + r;
        }

        private static float GetPointY(float r, float a)
        {
            var degrees = AngleToDegrees(a);
            return r * (float)Math.Sin(degrees) + r;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
                lock (this)
                {
                    // Calculations.
                    var offset = _ellipseWidth / 2;
                    _percentage = (_value * _maxValue) / 100;
                    var sweepAngle = (_percentage / 100) * 360;

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //
                    // Arc/ellipse.
                    //
                    DrawArc(e.Graphics, offset, _startAngle, sweepAngle);
                }
            }
            catch
            {
                // ignored.
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_CREATE:
                    if(_isIndeterminate)
                    {
                        WinApi.SetTimer(m.HWnd, (IntPtr) ANIMATION_TIMER_ID, 1, IntPtr.Zero);
                    }
                    break;
                case WinApi.WM_DESTROY:
                    if(_isIndeterminate)
                    {
                        WinApi.KillTimer(m.HWnd, (IntPtr) ANIMATION_TIMER_ID);
                    }
                    break;
                case WinApi.WM_TIMER:
                    switch ((int) m.WParam)
                    {
                        case ANIMATION_TIMER_ID:
                            if (DesignMode || !_isIndeterminate)
                                break;

                            _percentage = (_value * _maxValue) / 100;
                            var sweepAngle = (_percentage / 100) * 360;

                            _startAngle += 2;
                            sweepAngle += 5;

                            if (_startAngle > 270)
                                _startAngle = START_ANGLE;

                            if (sweepAngle > 270)
                                sweepAngle = START_ANGLE;

                            _value = ((sweepAngle / 360) * 100) * 100 / _maxValue;

                            Invalidate();
                            break;
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        private void DrawArc(Graphics g, float o, float startAngle, float sweepAngle)
        {
            //
            // Pen
            //
            var pen = new Pen(_color, _ellipseWidth);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            //
            // Rectangle
            //
            var point = AddPointF(PointF.Empty, o);
            var size = AddSizeF(Size, -2 * o);
            var rect = new RectangleF(point, size);
            //
            // Draw arc to screen.
            //
            g.DrawArc(pen, rect, startAngle, sweepAngle);
        }
    }
}
