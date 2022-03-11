using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI
{
    public class CheckBox : Control
    {
        private const int SIZE = 24;
        private const float BORDER_RADIUS = 3f;

        private readonly Color _borderColor;
        private readonly Color _backColor;
        private readonly Color _hoveredBackColor;
        private readonly Color _accentColor;

        // States.
        private bool _isHovered;
        private bool _threeState;
        private CheckState _state = CheckState.Unchecked;

        // Events.
        private EventHandler? _checkedChange;
        private EventHandler? _checkStateChanged;

        public CheckBox()
        {
            SetStyle(
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true
            );

            // Border color.
            _borderColor = Color.FromArgb(23, 23, 23);

            // Back color.
            _backColor = Color.FromArgb(237, 237, 237);
            _hoveredBackColor = Color.FromArgb(229, 229, 229);

            // Accent color.
            _accentColor = GraphicsHelper.GetWindowsAccentColor();
        }

        protected override Size DefaultSize
            => new(SIZE, SIZE);

        protected override void SetBoundsCore(int x, int y,
            int width, int height, BoundsSpecified specified)
        {
            var maxWidth = SIZE;

            if (!string.IsNullOrEmpty(Text))
            {
                using var g = this.CreateGraphics();
                var textSize = TextRenderer.MeasureText(g, Text, Font);

                maxWidth += 8 + textSize.Width;
            }
            
            base.SetBoundsCore(x, y, maxWidth, SIZE, specified);
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                AdjustSize();
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                AdjustSize();
            }
        }

        [Category("Behavior"),
         Description("Used to indicate that an option for some, but not all, child options.")]
        public bool ThreeState
        {
            get => _threeState;
            set
            {
                _threeState = value;
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("Indicates whether the component is in the checked state.")]
        public bool Checked
        {
            get => _state != CheckState.Unchecked;
            set
            {
                _state = value ? CheckState.Checked : CheckState.Unchecked;

                _checkedChange?.Invoke(this, EventArgs.Empty);
                _checkStateChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("Indicates the state of component.")]
        public CheckState CheckState
        {
            get => _state;
            set
            {
                _state = value;
                _checkedChange?.Invoke(this, EventArgs.Empty);
                _checkStateChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        [Description("Occurs when control is checked or unchecked."),
         Category("Action")]
        public event EventHandler CheckedChange
        {
            add => _checkedChange += value;
            remove => _checkedChange -= value;
        }

        [Description("Occurs when control's check state is changed."),
         Category("Action")]
        public event EventHandler CheckStateChanged
        {
            add => _checkStateChanged += value;
            remove => _checkStateChanged -= value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = GraphicsHelper.PrimeGraphics(e);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var borderColor = _state == CheckState.Unchecked ? _borderColor : _accentColor;
            var backColor = !_isHovered ? _backColor : _hoveredBackColor;

            if (_state != CheckState.Unchecked)
            {
                backColor = _accentColor;
            }

            var borderPen = new Pen(borderColor, 0);
            var baseBrush = new SolidBrush(backColor);

            var checkBoxSize = ClientSize.Height - 4;
            var checkBoxRectangle = new Rectangle(new Point(2, 2), new Size(checkBoxSize, checkBoxSize));
            var checkBoxPath = GraphicsHelper.CreateRoundedRectangle(checkBoxRectangle, BORDER_RADIUS);

            graphics.DrawPath(borderPen, checkBoxPath);
            graphics.FillPath(baseBrush, checkBoxPath);

            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var textSize = TextRenderer.MeasureText(
                graphics,
                Text,
                Font);

            TextRenderer.DrawText(
                graphics,
                Text,
                Font,
                new Point(SIZE + 4, (SIZE - textSize.Height) / 2),
                ForeColor);

            if (_state == CheckState.Unchecked)
                return;
            

            var glyphFont = SegoeFluentIcons.CreateFont(13f, FontStyle.Bold);
            var glyphIcon = _state switch
            {
                CheckState.Checked => SegoeFluentIcons.CHECK_MARK,
                CheckState.Indeterminate => SegoeFluentIcons.CHECKBOX_INDETERMINATE,
                _ => null
            };
            
            var glyphOrigin = (int)(SIZE - 13f) / 2;
            var glyphLocation = new Point(glyphOrigin, glyphOrigin);
            
            TextRenderer.DrawText(
                graphics,
                glyphIcon,
                glyphFont,
                glyphLocation,
                Color.White,
                TextFormatFlags.NoPadding);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_MOUSEMOVE:
                    
                    if (!_isHovered)
                    {
                        _isHovered = true;
                        Invalidate();
                    }

                    break;
                case WinApi.WM_MOUSELEAVE:

                    _isHovered = false;
                    Invalidate();
                    
                    break;
                case WinApi.WM_LBUTTONDOWN:
                case WinApi.WM_LBUTTONDBLCLK:

                    _state = _state switch
                    {
                        CheckState.Unchecked => CheckState.Checked,
                        CheckState.Checked when _threeState => CheckState.Indeterminate,
                        _ => CheckState.Unchecked
                    };

                    _checkedChange?.Invoke(this, EventArgs.Empty);
                    _checkStateChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                    break;
            }
            base.WndProc(ref m);
        }

        private void AdjustSize()
        {
            SetBoundsCore(Left, Top, SIZE, SIZE, BoundsSpecified.All);
            Invalidate();
        }
    }
}
