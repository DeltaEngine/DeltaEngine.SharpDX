using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using DXDevice = SharpDX.Direct3D11.Device;
using DXSwapChain = SharpDX.DXGI.SwapChain;
using Resource = SharpDX.Direct3D11.Resource;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Provides DirectX 11 support with extended range of features like Geometry shader, Hardware
	/// tessellation and compute shaders. Currently just used to support SharpDXDrawing.
	/// </summary>
	public class SharpDXDevice : Device
	{
		public SharpDXDevice(Window window, Settings settings)
			: base(window)
		{
			states = new SharpDXStates(settings);
			DXDevice.CreateWithSwapChain(DriverType.Hardware, SharpDXStates.CreationFlags,
				states.CreateSwapChainDescription(Width, Height, window.Handle), out nativeDevice,
				out swapChain);
			window.ViewportSizeChanged += ResetDeviceToNewViewportSize;
			ResetDeviceToNewViewportSize(window.ViewportPixelSize);
			window.FullscreenChanged += OnFullscreenChanged;
		}

		private readonly SharpDXStates states;
		private int Width
		{
			get { return (int)window.ViewportPixelSize.Width; }
		}
		private int Height
		{
			get { return (int)window.ViewportPixelSize.Height; }
		}
		private readonly DXDevice nativeDevice;
		public DXDevice NativeDevice
		{
			get { return nativeDevice; }
		}
		private readonly DXSwapChain swapChain;

		private void ResetDeviceToNewViewportSize(Size newSizeInPixel)
		{
			ResizeBackBufferIfItExistedBefore();
			backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);
			backBufferView = new RenderTargetView(nativeDevice, backBuffer);
			surface = backBuffer.QueryInterface<Surface>();
		}

		private void ResizeBackBufferIfItExistedBefore()
		{
			if (backBuffer == null)
				return;
			backBuffer.Dispose();
			backBufferView.Dispose();
			surface.Dispose();
			swapChain.ResizeBuffers(SharpDXStates.BackBufferCount, Width, Height,
				SharpDXStates.BackBufferFormat, SharpDXStates.BackBufferFlags);
		}

		public override void SetViewport(Size viewportSize)
		{
			SetModelViewProjectionMatrixFor2D();
		}

		protected override void OnFullscreenChanged(Size displaySize, bool wantFullscreen)
		{
			swapChain.SetFullscreenState(wantFullscreen, null);
			base.OnFullscreenChanged(displaySize, wantFullscreen);
		}

		public DeviceContext Context
		{
			get { return nativeDevice.ImmediateContext; }
		}
		private Texture2D backBuffer;
		private RenderTargetView backBufferView;
		private Surface surface;

		public Texture2D BackBuffer
		{
			get { return backBuffer; }
			set { backBuffer = value; }
		}

		public override void Clear()
		{
			Context.OutputMerger.SetTargets(backBufferView);
			Context.Rasterizer.SetViewport(new ViewportF(0, 0, Width, Height, 0.0f, 1.0f));
			if (window.BackgroundColor.A > 0)
				Context.ClearRenderTargetView(backBufferView, new Color4(window.BackgroundColor.PackedRgba));
		}

		public override void Present()
		{
			if (!nativeDevice.IsDisposed)
				swapChain.Present(0, PresentFlags.None);
		}

		public override void SetBlendMode(BlendMode blendMode)
		{
			if (currentBlendMode == blendMode)
				return;
			var blendState = states.GetBlendState(nativeDevice, blendMode);
			Context.OutputMerger.SetBlendState(blendState);
			currentBlendMode = blendMode;
		}

		private BlendMode currentBlendMode = BlendMode.Opaque;
		
		public override CircularBuffer CreateCircularBuffer(ShaderWithFormat shader,
			BlendMode blendMode, VerticesMode drawMode = VerticesMode.Triangles)
		{
			return new SharpDXCircularBuffer(this, shader, blendMode, drawMode);
		}

		protected override void EnableClockwiseBackfaceCulling()
		{
			if (isCullingEnabled)
				return;
			isCullingEnabled = true;
			UpdateCullModeOnRasterizer(CullMode.Back);
		}

		/// <summary>
		/// CullMode.Back is enabled by default in DirectX, but it still need to set it once in SharpDX
		/// </summary>
		private bool isCullingEnabled;

		protected override void DisableCulling()
		{
			if (!isCullingEnabled)
				return;
			isCullingEnabled = false;
			UpdateCullModeOnRasterizer(CullMode.None);
		}

		private void UpdateCullModeOnRasterizer(CullMode cullMode)
		{
			Context.Rasterizer.State = states.GetCullBackRasterizerState(nativeDevice, cullMode);
		}

		public override void DisableDepthTest()
		{
			var depthState = states.GetDepthStencilState(nativeDevice, false);
			nativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(depthState, 1);
		}

		public override void EnableDepthTest()
		{
			var depthState = states.GetDepthStencilState(nativeDevice, true);
			nativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(depthState, 1);
		}

		public override void Dispose()
		{
			Context.ClearState();
			Context.Flush();
			Context.Dispose();
			states.Dispose();
			backBufferView.Dispose();
			backBuffer.Dispose();
			surface.Dispose();
			swapChain.Dispose();
			OutputRefCountOfLiveObjectsToFindMemoryLeaksInDebugMode();
			nativeDevice.Dispose();
		}

		/// <summary>
		/// Helps finding any remaining unreleased references via console output, which can be
		/// enabled by using Dx Control Panel and adding the app there (DX11) and also enabling
		/// native debugging output in the properties of the project to debug. Then see Output window.
		/// </summary>
		private void OutputRefCountOfLiveObjectsToFindMemoryLeaksInDebugMode()
		{
#if DEBUG
			var deviceDebug = new DeviceDebug(nativeDevice);
			// http://sharpdx.org/forum/4-general/1241-reportliveobjects
			// Contains several Refcount: 0 lines. This cannot be avoided, but is still be useful to
			// find memory leaks (all objects should have Refcount=0, the device still has RefCount=3)
			deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Detail);
			deviceDebug.Dispose();
#endif
		}
	}
}