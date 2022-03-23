using System.Diagnostics;
using DirectN;
using WinForms.Fluent.UI.Utilities.Enums;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI;

public class IndicatedSurfaceBase : Control
{
    private IComObject<ID2D1Factory>? _factory;
    private IComObject<ID2D1HwndRenderTarget>? _renderTarget;
    private IComObject<ID2D1PathGeometry> _pathGeometry;
    private IComObject<ID2D1GeometrySink> _geometrySink;

    public IndicatedSurfaceBase()
    {
        // Lighter/darker color.
        // Normal-hovered/active/Active-clicked: 233
        // Active-hovered/Normal-clicked: 236
    }

    public HRESULT CreateResources(D2D_SIZE_U renderSize)
    {
        HRESULT hr = HRESULTS.S_OK;

        if (_renderTarget != null)
            return hr;
        
        var renderTargetProperties = new D2D1_RENDER_TARGET_PROPERTIES();
        var handleRenderTargetProperties = new D2D1_HWND_RENDER_TARGET_PROPERTIES
        {
            hwnd = Handle,
            pixelSize = renderSize
        };
        
        _factory ??= D2D1Functions.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED);
        hr = _factory.Object.CreateHwndRenderTarget(ref renderTargetProperties, ref handleRenderTargetProperties, out var renderTarget);

        if (hr != HRESULTS.S_OK) 
            return hr;

        _renderTarget = new ComObject<ID2D1HwndRenderTarget>(renderTarget);
        hr = _factory.Object.CreatePathGeometry(out var pathGeometry);

        if (hr != HRESULTS.S_OK)
            return hr;

        _pathGeometry = new ComObject<ID2D1PathGeometry>(pathGeometry);
        return hr;
    }

    public void ResizeRenderTarget(D2D_SIZE_U size)
        => _renderTarget?.Object.Resize(size);

    public HRESULT BeginDraw()
    {
        if(_renderTarget is null)
            return -1;

        _renderTarget.BeginDraw();
        _renderTarget.Clear(GraphicsHelper.ColorToD3dColor(BackColor)); // TODO: Check if correct.

        var hr = _pathGeometry.Object.Open(out var geometrySink);

        if (hr != HRESULTS.S_OK)
        {
            Debug.Fail($"Unable to open path geometry. (0x{hr:X8})");
            return hr;
        }

        _geometrySink = new ComObject<ID2D1GeometrySink>(geometrySink);
        return HRESULTS.S_OK;
    }

    public void EndDraw()
    {
        _geometrySink.Object.Close();

        // Do rendering here.

        _renderTarget.EndDraw();
    }

    public void DrawSurface(D2D_SIZE_F bounds, IndicatedSurfaceState state)
    {

    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _geometrySink.Dispose();
            _pathGeometry.Dispose();
            _renderTarget?.Dispose();
            _factory?.Dispose();
        }

        base.Dispose(disposing);
    }
}