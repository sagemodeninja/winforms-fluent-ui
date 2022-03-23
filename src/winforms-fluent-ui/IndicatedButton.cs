using DirectN;
using WinForms.Fluent.UI.Utilities.Classes;
using WinForms.Fluent.UI.Utilities.Helpers;

namespace WinForms.Fluent.UI
{
    public class IndicatedButton : IndicatedSurfaceBase
    {

        private IComObject<ID2D1Factory>? _factory;
        private ID2D1HwndRenderTarget? _renderTarget;

        public IndicatedButton()
        {
            SetStyle(
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true
            );
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WinApi.WM_PAINT)
                OnPaint();

            base.WndProc(ref m);
        }

        protected override void OnResize(EventArgs e)
        {
            if (_renderTarget != null)
            {
                var size = new D2D_SIZE_U((uint)Width, (uint)Height);

                _renderTarget.Resize(size);
                Invalidate();
            }

            base.OnResize(e);
        }

        private void OnPaint()
        {
            var result = CreateGraphicsResources();
            if (result == 0 && _renderTarget is not null)
            {
                WinApi.BeginPaint(Handle, out var paintStruct);

                _renderTarget.BeginDraw();
                _renderTarget.Clear(GraphicsHelper.ColorToD3dColor(BackColor));

                _renderTarget.GetFactory(out var factory);
                factory.CreatePathGeometry(out var pathGeometry);

                pathGeometry.Open(out var geometrySink);
                _renderTarget.GetSize(out var renderSize);

                GraphicsHelper.CreateRoundedRectangle(ref geometrySink, renderSize, 10f);
                geometrySink.Close();

                var backColorValue = 233f / 255f;
                var backColor = new _D3DCOLORVALUE(backColorValue, backColorValue, backColorValue);
                using var brush = _renderTarget.CreateSolidColorBrush<ID2D1SolidColorBrush>(backColor);

                //_renderTarget.DrawGeometry(pathGeometry, brush.Object, 1.0f, null);
                _renderTarget.FillGeometry(pathGeometry, brush.Object, null);

                _renderTarget.EndDraw();
                WinApi.EndPaint(Handle, ref paintStruct);
            }
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

            _factory ??= D2D1Functions.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED);
            hr = _factory.Object.CreateHwndRenderTarget(ref renderTargetProperties, ref handleRenderTargetProperties, out _renderTarget);

            return hr;
        }
    }
}
