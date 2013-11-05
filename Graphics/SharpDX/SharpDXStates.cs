using System;
using DeltaEngine.Core;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using D2dFactory = SharpDX.Direct2D1.Factory;
using DXDevice = SharpDX.Direct3D11.Device;
using DXSwapChain = SharpDX.DXGI.SwapChain;
using CreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;
using DeltaRect = DeltaEngine.Datatypes.Rectangle;
using FillMode = SharpDX.Direct3D11.FillMode;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Helper class for SharpDXDevice to provide useful states and default initialization objects.
	/// </summary>
	public class SharpDXStates : IDisposable
	{
		public SharpDXStates(Settings settings)
		{
			this.settings = settings;
			d2DFactory = new D2dFactory();
		}

		private readonly Settings settings;
		protected readonly D2dFactory d2DFactory;

		public static CreationFlags CreationFlags
		{
			get
			{
#if DEBUG
				return CreationFlags.BgraSupport | CreationFlags.Debug;
#else
				return CreationFlags.BgraSupport;
#endif
			}
		}

		internal const int BackBufferCount = 1;
		internal const Format BackBufferFormat = Format.R8G8B8A8_UNorm;
		internal const SwapChainFlags BackBufferFlags = SwapChainFlags.AllowModeSwitch;
		protected readonly RenderTargetProperties defaultRenderTargetProperties =
			new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied));

		protected internal SwapChainDescription CreateSwapChainDescription(int width, int height,
			IntPtr handle)
		{
			return new SwapChainDescription
			{
				BufferCount = BackBufferCount,
				ModeDescription = new ModeDescription(width, height, new Rational(60, 1), BackBufferFormat),
				IsWindowed = !settings.StartInFullscreen,
				OutputHandle = handle,
				SampleDescription = new SampleDescription(1, 0),
				//makes some GPUs crash CreateWithSwapChain with E_INVALIDARG
				//settings.AntiAliasingSamples, settings.AntiAliasingSamples > 1 ? 1 : 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput
			};
		}

		public RasterizerState GetCullBackRasterizerState(DXDevice device)
		{
			return rasterizerState2D ??
				(rasterizerState2D = new RasterizerState(device, GetRasterizer(CullMode.Back)));
		}

		private RasterizerState rasterizerState2D;

		private static RasterizerStateDescription GetRasterizer(CullMode cullMode)
		{
			var rasterizerDescription = new RasterizerStateDescription
			{
				CullMode = cullMode,
				FillMode = FillMode.Solid,
				IsFrontCounterClockwise = false,
				DepthBias = 0,
				DepthBiasClamp = 0.0f,
				SlopeScaledDepthBias = 0.0f,
				IsDepthClipEnabled = true,
				IsScissorEnabled = false,
				IsMultisampleEnabled = true,
				IsAntialiasedLineEnabled = false
			};
			return rasterizerDescription;
		}

		public RasterizerState GetNoCullingRasterizerState(DXDevice device)
		{
			return rasterizerState3D ??
				(rasterizerState3D = new RasterizerState(device, GetRasterizer(CullMode.None)));
		}

		private RasterizerState rasterizerState3D;

		public virtual void Dispose()
		{
			d2DFactory.Dispose();
		}

		protected internal static BlendStateDescription GetBlendStateDescription(
			BlendOption source = BlendOption.SourceAlpha,
			BlendOption destination = BlendOption.InverseSourceAlpha)
		{
			var description = new BlendStateDescription();
			description.RenderTarget[0].IsBlendEnabled = (source != BlendOption.One) ||
				(destination != BlendOption.Zero);
			description.RenderTarget[0].SourceBlend =
				description.RenderTarget[0].SourceAlphaBlend = source;
			description.RenderTarget[0].DestinationBlend =
				description.RenderTarget[0].DestinationAlphaBlend = destination;
			description.RenderTarget[0].BlendOperation =
				description.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			description.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			return description;
		}
	}
}