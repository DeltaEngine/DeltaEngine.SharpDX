using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using DXDevice = SharpDX.Direct3D11.Device;
using DXSwapChain = SharpDX.DXGI.SwapChain;
using MapFlags = SharpDX.Direct3D11.MapFlags;
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
				states.CreateSwapChainDescription(Width, Height, (IntPtr)window.Handle), out nativeDevice,
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
			var description = new BlendStateDescription();
			description.RenderTarget[0] = GetRenderTargetBlendDescription(blendMode);
			var blendState = new BlendState(nativeDevice, description);
			Context.OutputMerger.SetBlendState(blendState);
			currentBlendMode = blendMode;
		}

		private BlendMode currentBlendMode = BlendMode.Opaque;

		private static RenderTargetBlendDescription GetRenderTargetBlendDescription(
			BlendMode blendMode)
		{
			var targetDescription = GetDefaultRenderTargetBlendDescription();
			SetupRenderTargetBlendDescriptionAccordingToTheMode(blendMode, ref targetDescription);
			return targetDescription;
		}

		private static RenderTargetBlendDescription GetDefaultRenderTargetBlendDescription()
		{
			return new RenderTargetBlendDescription
			{
				IsBlendEnabled = true,
				SourceAlphaBlend = BlendOption.One,
				DestinationAlphaBlend = BlendOption.Zero,
				AlphaBlendOperation = BlendOperation.Add,
				RenderTargetWriteMask = ColorWriteMaskFlags.All,
				SourceBlend = BlendOption.SourceAlpha,
				DestinationBlend = BlendOption.One,
				BlendOperation = BlendOperation.Add
			};
		}

		private static void SetupRenderTargetBlendDescriptionAccordingToTheMode(BlendMode blendMode,
			ref RenderTargetBlendDescription targetDescription)
		{
			switch (blendMode)
			{
			case BlendMode.Normal:
				targetDescription.DestinationBlend = BlendOption.InverseSourceAlpha;
				break;
			case BlendMode.Additive:
				break;
			case BlendMode.Subtractive:
				targetDescription.BlendOperation = BlendOperation.ReverseSubtract;
				break;
			case BlendMode.LightEffect:
				targetDescription.SourceBlend = BlendOption.DestinationColor;
				targetDescription.BlendOperation = BlendOperation.Add;
				break;
			default:
				targetDescription.IsBlendEnabled = false;
				break;
			}
		}

		public override CircularBuffer CreateCircularBuffer(ShaderWithFormat shader,
			BlendMode blendMode, VerticesMode drawMode = VerticesMode.Triangles)
		{
			return new SharpDXCircularBuffer(this, shader, blendMode, drawMode);
		}
		
		public override bool CullBackFaces
		{
			get { return cullBackFaces; }
			set
			{
				if (cullBackFaces == value)
					return;
				cullBackFaces = value;
				Context.Rasterizer.State = cullBackFaces
					? states.GetCullBackRasterizerState(nativeDevice)
					: states.GetNoCullingRasterizerState(nativeDevice);
			}
		}

		private bool cullBackFaces;

		public override void DisableDepthTest()
		{
			var description = new DepthStencilStateDescription { IsDepthEnabled = false };
			var depthState = new DepthStencilState(NativeDevice, description);
			nativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(depthState, 1);
		}

		public override void EnableDepthTest()
		{
			Context.Rasterizer.State = states.GetNoCullingRasterizerState(nativeDevice);
			var description = new DepthStencilStateDescription
			{
				IsDepthEnabled = true,
				DepthWriteMask = DepthWriteMask.All,
				DepthComparison = Comparison.Less,
				IsStencilEnabled = false,
				StencilReadMask = 0xFF,
				StencilWriteMask = 0xFF
			};
			var depthState = new DepthStencilState(NativeDevice, description);
			nativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(depthState, 1);
		}

		public void EnableTexturing() {}

		public static Image currentTexture;

		public void DisableTexturing()
		{
			currentTexture = null;
		}

		public override void Dispose()
		{
			states.Dispose();
			backBuffer.Dispose();
			swapChain.Dispose();
			surface.Dispose();
			if (nativeDevice.IsDisposed == false)
			{
				nativeDevice.ImmediateContext.Dispose();
#if DEBUG
				// Helps finding any remaining unreleased references via console output, which is NOT empty,
				// but contains several Refcount: 0 lines. This cannot be avoided, but is still be useful to
				// find memory leaks (Refcount>0): http://sharpdx.org/forum/4-general/1241-reportliveobjects
				var deviceDebug = new DeviceDebug(nativeDevice);
				deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Detail);
#endif
			}
			nativeDevice.Dispose();
		}

		public BlendState GetAlphaBlendStateLazy()
		{
			return alphaBlend ??
				(alphaBlend = new BlendState(nativeDevice, SharpDXStates.GetBlendStateDescription()));
		}

		private BlendState alphaBlend;

		public void SetData<T>(Buffer buffer, T[] data, int count = 0) where T : struct
		{
			count = count == 0 ? data.Length : count;
			DataStream dataStream;
			Context.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out dataStream);
			dataStream.WriteRange(data, 0, count);
			Context.UnmapSubresource(buffer, 0);
		}
	}
}