using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace WinForms.Fluent.UI
{
    public class ProgressRing : Control
    {
        private const float START_ANGLE = -90F;
        private const int BASE_ELLIPSE_OFFSET = 1;
        private const float POINT_OFFSET = 1.2225F;

        private float _value;
        private float _maxValue;
        private float _percentage;
        private float _ellipseWidth;

        public ProgressRing()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            _value = 0;
            _maxValue = 100;
            _percentage = 0;
            _ellipseWidth = 5;
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
                    var offset = _ellipseWidth / 2 + BASE_ELLIPSE_OFFSET;
                    _percentage = (_value * _maxValue) / 100;
                    var sweepAngle = (_percentage / 100) * 360;

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //
                    // Arc/ellipse.
                    //
                    DrawArc(e.Graphics, offset, sweepAngle);
                    //
                    // Static rounded point.
                    //
                    DrawStaticRoundedPoint(e.Graphics, offset);
                    // 
                    // Moving rounded point.
                    //
                    DrawMovingRoundedPoint(e.Graphics, sweepAngle);
                }
            }
            catch
            {
                // ignored.
            }
        }

        private void DrawArc(Graphics g, float o, float a)
        {
            //
            // Pen
            //
            var pen = new Pen(ForeColor, _ellipseWidth);
            //
            // Rectangle
            //
            var point = AddPointF(PointF.Empty, o);
            var size = AddSizeF(Size, -2 * o);
            var rect = new RectangleF(point, size);
            //
            // Draw arc to screen.
            //
            g.DrawArc(pen, rect, START_ANGLE, a);
        }

        private void DrawStaticRoundedPoint(Graphics g, float o)
        {
            // 
            // Brush
            //
            var pointBrush = new SolidBrush(ForeColor);
            // 
            // Size
            // 
            var pointSize = new SizeF(_ellipseWidth, _ellipseWidth);
            //
            // Rectangle
            // 
            var pointPoint = new PointF((Size.Width - o * 2) / 2, POINT_OFFSET);
            var pointRect = new RectangleF(pointPoint, pointSize);
            //
            // Draw point to screen.
            //
            g.FillEllipse(pointBrush, pointRect);
        }

        private void DrawMovingRoundedPoint(Graphics g, float a)
        {
            // 
            // Brush
            //
            var pointBrush = new SolidBrush(ForeColor);
            // 
            // Size
            // 
            var pointSize = new SizeF(_ellipseWidth, _ellipseWidth);
            // Points
            var ellipseRadius = (Size.Width - POINT_OFFSET * 2) / 2;
            var pointRadius = _ellipseWidth / 2;
            var pointAngle = a - 90F;
            var ellipseX = GetPointX(ellipseRadius, pointAngle) + POINT_OFFSET;
            var ellipseY = GetPointY(ellipseRadius, pointAngle) + POINT_OFFSET;
            var pointX = GetPointX(pointRadius, pointAngle);
            var pointY = GetPointY(pointRadius, pointAngle);
            //
            // Rectangle
            //
            var pointPoint = new PointF(ellipseX - pointX, ellipseY - pointY);
            var pointRect = new RectangleF(pointPoint, pointSize);
            //
            // Draw point to screen.
            //
            g.FillEllipse(pointBrush, pointRect);
        }
    }
}
