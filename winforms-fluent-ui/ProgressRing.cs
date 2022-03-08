using System.ComponentModel;
using System.Drawing.Drawing2D;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Helpers;
using Timer = WinForms.Fluent.UI.Utilities.Classes.Timer;

namespace WinForms.Fluent.UI
{
    public class ProgressRing : Control
    {
        private const float START_ANGLE = -90F;
        private const ulong TOTAL_DURATION = 2000;
        private const ulong HALF_DURATION = TOTAL_DURATION / 2;

        private readonly Color _color;
        private readonly Timer _timer;

        private RectangleF _arcBounds;
        private Pen _pen;

        private bool _isIndeterminate;
        private float _maxValue;
        private float _value;
        private float _ellipseWidth;

        private float _startAngle;
        private float _sweepAngle;

        public ProgressRing()
        {
            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            DoubleBuffered = true;

            _value = 0;
            _maxValue = 100;
            _ellipseWidth = 8;

            _startAngle = START_ANGLE;
            _sweepAngle = 90f;

            _color = GraphicsHelper.GetWindowsAccentColor();
            _arcBounds = CreateArcBounds(_ellipseWidth, ClientSize);
            _pen = CreatePen(_color, _ellipseWidth);

            _timer = new Timer(TimerElapse);
        }

        [Category("Appearance")]
        [Description("Set the width of ellipse.")]
        public float EllipseWidth
        {
            get => _ellipseWidth;
            set
            {
                _ellipseWidth = value;
                _arcBounds = CreateArcBounds(_ellipseWidth, ClientSize);
                _pen = CreatePen(_color, _ellipseWidth);
                
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
                if (_isIndeterminate && !DesignMode)
                {
                    _startAngle = -90f;
                    _sweepAngle = 0f;
                    _timer.Start();
                }
                else
                {
                    _startAngle = -90f;
                    _sweepAngle = ValueToAngle(_value, _maxValue);
                    _timer.Stop();
                }

                Invalidate();
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
                _sweepAngle = ValueToAngle(_value, _maxValue);
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
                _sweepAngle = ValueToAngle(_value, _maxValue);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Debug lines.
            if(DesignMode)
                e.Graphics.DrawRectangle(Pens.Black, ClientRectangle);

            e.Graphics.DrawArc(_pen, _arcBounds, _startAngle, _sweepAngle);
            
            base.OnPaint(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_CREATE:
                    if(_isIndeterminate && !DesignMode)
                    {
                        _startAngle = -90f;
                        _sweepAngle = 0f;
                        _timer.Start();
                    }
                    break;
                case WinApi.WM_DESTROY:
                    if(_isIndeterminate)
                    {
                    }
                    break;
            }

            base.WndProc(ref m);
        }
        
        protected override void OnSizeChanged(EventArgs e)
        {
            _arcBounds = CreateArcBounds(_ellipseWidth, ClientSize);
            base.OnSizeChanged(e);
        }

        private void TimerElapse(ulong milliSinceBeginning)
        {
            if (milliSinceBeginning < TOTAL_DURATION)
            {
                var sweepTime = milliSinceBeginning <= HALF_DURATION ? milliSinceBeginning : TOTAL_DURATION - milliSinceBeginning;
                
                _startAngle = EasingFunctions.Linear(milliSinceBeginning, -90f, 990f - -90f, TOTAL_DURATION);
                _sweepAngle = EasingFunctions.Linear(sweepTime, 0, 180f, HALF_DURATION);

                Invalidate();
            }
            else
            {
                _timer.ResetClock();
            }
        }

        private static RectangleF CreateArcBounds(float arcWidth, Size clientSize)
        {
            var offset = arcWidth / 2;
            var point = new PointF(offset, offset);
            var size = AddSizeF(clientSize, -arcWidth);
            
            return new RectangleF(point, size);
        }

        private static Pen CreatePen(Color color, float width)
        {
            var pen = new Pen(color, width)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            return pen;
        }

        private static float ValueToAngle(float value, float max)
        {
            var percentage = (value * max) / 100;
            return (percentage / 100) * 360;
        }

        private static SizeF AddSizeF(SizeF s, float v)
        {
            s.Width += v;
            s.Height += v;

            return s;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pen.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
