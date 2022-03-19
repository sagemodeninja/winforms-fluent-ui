using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI;

public class PersonPicture : Control
{
    private const int DEFAULT_COIN_SIZE = 95;

    private readonly Color _borderColor;
    private readonly Color _backColor;
    private readonly Color _fontColor;

    private int _coinSize;
    private ProfileType _profileType;
    private Image? _profilePicture;
    private string _displayName;
    private string _initials;
    
    public PersonPicture()
    {
        SetStyle(
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true
        );

        _coinSize = DEFAULT_COIN_SIZE;
        _profileType = ProfileType.ProfileImage;
        _displayName = null!;
        _initials = null!;
        _borderColor = Color.FromArgb(218, 218, 218);
        _backColor = Color.FromArgb(220, 220, 220);
        _fontColor = Color.FromArgb(23, 23, 23);
    }

    protected override Size DefaultSize
        => new(DEFAULT_COIN_SIZE, DEFAULT_COIN_SIZE);

    protected override void SetBoundsCore(int x, int y,
        int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, _coinSize, _coinSize, specified);
    }

    [Category("Behavior"),
     Description("Gets or sets the contact's initials.")]
    public ProfileType ProfileType
    {
        get => _profileType;
        set
        {
            if (_profileType == value) 
                return;

            _profileType = value;
            Invalidate();
        }
    }

    [Category("Appearance"),
     Description("The size of this control.")]
    public int CoinSize
    {
        get => _coinSize;
        set
        {
            if (_coinSize == value) 
                return;

            _coinSize = value;

            SetBoundsCore(Left, Top, value, value, BoundsSpecified.All);
            Invalidate();
        }
    }

    [Category("Data"),
     Description("Gets or sets the contact's display name.")]
    public string DisplayName
    {
        get => _displayName;
        set
        {
            if (_displayName == value) 
                return;
            
            _displayName = value;
            Invalidate();
        }
    }

    [Category("Data"),
     Description("Gets or sets the contact's initials.")]
    public string Initials
    {
        get => _initials;
        set
        {
            if (_initials == value) 
                return;

            _initials = value;
            Invalidate();
        }
    }

    [Category("Data"), 
     Description("")]
    public Image? ProfilePicture
    {
        get => _profilePicture;
        set
        {
            if (_profilePicture == value)
                return;

            _profilePicture = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var graphics = GraphicsHelper.PrimeGraphics(e);

        // Coin.
        var coinPen = new Pen(_borderColor);
        var coinBrush = new SolidBrush(_backColor);
        var coinRectangle = new Rectangle(Point.Empty, ClientSize);
        
        var coinPath = new GraphicsPath();
        coinPath.AddEllipse(coinRectangle);

        graphics.FillPath(coinBrush, coinPath);
        Region = new Region(coinPath);

        if (_profileType == ProfileType.ProfileImage && _profilePicture is not null)
        {
            graphics.DrawImage(_profilePicture, coinRectangle);
        }
        else
        {
            // Initials.
            var initials = _profileType == ProfileType.DisplayName && !string.IsNullOrEmpty(_displayName)
                ? GetInitialsFromDisplayName(_displayName)
                : _initials;
            var initialsSize = ClientRectangle.Height * 0.45f;
            var initialsFont = new Font(Font.FontFamily, initialsSize, FontStyle.Bold, GraphicsUnit.Pixel);
            
            if(!string.IsNullOrEmpty(initials))
            {
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                TextRenderer.DrawText(graphics, initials, initialsFont, coinRectangle, _fontColor,
                    TextFormatFlags.NoPadding | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            
            initialsFont.Dispose();
        }

        // Add border.
        graphics.DrawPath(coinPen, coinPath);
        
        coinPen.Dispose();
        coinBrush.Dispose();
    }

    private static string GetInitialsFromDisplayName(string displayName)
    {
        var names = displayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return names.Length switch
        {
            1 => names[0][0].ToString(),
            > 1 => $"{names[0][0]}{names[1][0]}",
            _ => string.Empty
        };
    }
}