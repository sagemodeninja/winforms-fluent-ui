using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DirectN;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI
{
    private const float DEFAULT_RADIUS = 47.5f;

    private readonly Color _borderColor;
    private readonly Color _backColor;
    private readonly Color _initialsColor;

    private float _radius;
    private ProfileType _profileType;
    private Image? _profilePicture;
    private string _displayName;
    private string _initials;

    // Direct2D
    private ID2D1Factory? _factory;
    private IComObject<IDWriteFactory>? _writeFactory;
    private ID2D1HwndRenderTarget? _renderTarget;
    private IComObject<ID2D1SolidColorBrush> _ellipseBrush;
    private D2D1_ELLIPSE _ellipse;
    private IComObject<ID2D1SolidColorBrush> _initialsBrush;
    private IComObject<IDWriteTextFormat>? _initialsFormat;
    private D2D1_ELLIPSE _maskEllipse;
    private ID2D1EllipseGeometry _ellipseGeometry;
    private D2D_POINT_2F _initialsOrigin;
    private float _initialsSize;

    public PersonPicture()
    {
        SetStyle(
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true
        );

        _radius = DEFAULT_RADIUS;
        _profileType = ProfileType.ProfileImage;
        _displayName = null!;
        _initials = null!;
        _borderColor = Color.FromArgb(218, 218, 218);
        _backColor = Color.FromArgb(220, 220, 220);
        _initialsColor = Color.FromArgb(23, 23, 23);
    }

    protected override Size DefaultSize
    {
        get
        {
            const int SIZE = (int)(DEFAULT_RADIUS * 2);
            return new Size(SIZE, SIZE);
        }
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

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WinApi.WM_PAINT)
            OnPaint();

        base.WndProc(ref m);
    }

    private void OnPaint()
    {
        var result = CreateGraphicsResources();
        if (result == 0 && _renderTarget is not null)
        {
            WinApi.BeginPaint(Handle, out var paintStruct);

            _renderTarget.BeginDraw();
            _renderTarget.Clear(GraphicsHelper.ColorToD3dColor(BackColor));

            if (_profileType == ProfileType.ProfileImage && _profilePicture is not null)
            {
                _renderTarget.GetSize(out var size);

                #region MASK

                _renderTarget.GetFactory(out var factory);
                factory.CreateEllipseGeometry(_maskEllipse, out var ellipseGeometry);

                _renderTarget.CreateLayer(IntPtr.Zero, out var layer);

                var ellipseGeometryPtr = Marshal.GetIUnknownForObject(ellipseGeometry);
                _renderTarget.PushLayer(new D2D1_LAYER_PARAMETERS
                {
                    contentBounds = new D2D_RECT_F(new D2D_POINT_2F(0, 0), size),
                    geometricMask = ellipseGeometryPtr,
                    maskTransform = D2D_MATRIX_3X2_F.Identity(),
                    opacity = 1f
                }, layer);

                #endregion

                #region BITMAP
                // Convert to stream.
                using var stream = new MemoryStream();
                _profilePicture.Save(stream, _profilePicture.RawFormat);

                // Source.
                using var bitmapSource = WICFunctions.LoadBitmapSource(stream);

                // Properties.
                var pixelFormat = new D2D1_PIXEL_FORMAT
                {
                    format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
                    alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE
                };
                var bitmapProperties = new D2D1_BITMAP_PROPERTIES
                {
                    pixelFormat = pixelFormat,
                    dpiX = _profilePicture.HorizontalResolution,
                    dpiY = _profilePicture.VerticalResolution
                };
                var bitmapPropertiesPtr = Marshal.AllocHGlobal(Marshal.SizeOf(bitmapProperties));
                Marshal.StructureToPtr(bitmapProperties, bitmapPropertiesPtr, false);

                // Create bitmap.
                _renderTarget.CreateBitmapFromWicBitmap(bitmapSource.Object, bitmapPropertiesPtr, out var bitmap);

                if (bitmap is null)
                    return;
                #endregion

                var destRect = new D2D_RECT_F(_initialsOrigin, new D2D_SIZE_F(_radius * 2, _radius * 2));
                _renderTarget.DrawBitmap(bitmap, 1.0f, D2D1_BITMAP_INTERPOLATION_MODE.D2D1_BITMAP_INTERPOLATION_MODE_LINEAR, destRect);
                _renderTarget.PopLayer();
            }
            else
            {
                // Ellipse.
                _renderTarget.FillEllipse(ref _ellipse, _ellipseBrush.Object);

                // Initials.
                var initials = _profileType == ProfileType.DisplayName && !string.IsNullOrEmpty(_displayName)
                    ? GetInitialsFromDisplayName(_displayName)
                    : _initials;

                var initialsRect = new D2D_RECT_F(_initialsOrigin, _radius * 2, _radius * 2);
                _renderTarget.DrawText(initials, _initialsFormat?.Object, initialsRect, _initialsBrush.Object);
            }

            _renderTarget.EndDraw();
            WinApi.EndPaint(Handle, ref paintStruct);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        if (_renderTarget != null)
        {
            var size = new D2D_SIZE_U((uint)Width, (uint)Height);

            _renderTarget.Resize(size);
            CalculateLayout();
            
            Invalidate();
        }

        base.OnResize(e);
    }

    private void CalculateLayout()
    {
        if (_renderTarget == null) 
            return;

        _renderTarget.GetSize(out var size);

        // Ellipse.
        var x = size.width / 2;
        var y = size.height / 2;

        _radius = Math.Min(x, y);
        _ellipse = new D2D1_ELLIPSE(x, y, _radius);
        _maskEllipse = new D2D1_ELLIPSE(x, y, _radius);

        // Initials.
        _initialsSize = (_radius * 2) * 0.45f;
        var originX = x - _radius;
        var originY = y - _radius;

        _initialsOrigin = new D2D_POINT_2F(originX, originY);
        CreateInitialsFormat();
    }

    private void CreateInitialsFormat()
    {
        _writeFactory ??= DWriteFunctions.DWriteCreateFactory();

        _initialsFormat?.Dispose();
        _initialsFormat = _writeFactory.Object.CreateTextFormat<IDWriteTextFormat>(
            Font.FontFamily.Name,
            _initialsSize,
            null,
            DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_SEMI_BOLD);
        
        _initialsFormat.Object.SetTextAlignment(DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_CENTER);
        _initialsFormat.Object.SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_CENTER);
    }

    private HRESULT CreateGraphicsResources()
    {
        HRESULT hr = HRESULTS.S_OK;
        
        if (_renderTarget != null) 
            return hr;

        var size = new D2D_SIZE_U((uint)Width, (uint)Height);
        var renderTargetProperties = new D2D1_RENDER_TARGET_PROPERTIES();
        var handleRenderTargetProperties = new D2D1_HWND_RENDER_TARGET_PROPERTIES
        {
            hwnd = Handle,
            pixelSize = size
        };

        _factory ??= D2D1Functions.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED).As<ID2D1Factory>();
        hr = _factory.CreateHwndRenderTarget(ref renderTargetProperties, ref handleRenderTargetProperties, out _renderTarget);

        if (hr != HRESULTS.S_OK)
            return hr;

        // Pre calculate layout.
        CalculateLayout();

        // Brushes.
        var backColor = GraphicsHelper.ColorToD3dColor(_backColor);
        _ellipseBrush = _renderTarget.CreateSolidColorBrush<ID2D1SolidColorBrush>(backColor);

        var initialsColor = GraphicsHelper.ColorToD3dColor(_initialsColor);
        _initialsBrush = _renderTarget.CreateSolidColorBrush<ID2D1SolidColorBrush>(initialsColor);

        _factory.CreateEllipseGeometry(_ellipse, out _ellipseGeometry);

        return hr;
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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _writeFactory?.Dispose();
            _ellipseBrush.Dispose();
            _initialsBrush.Dispose();
            _initialsFormat?.Dispose();
        }

        base.Dispose(disposing);
    }
}