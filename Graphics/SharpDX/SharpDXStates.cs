using System;
using System.Collections.Generic;
using DeltaEngine.Core;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using D2dFactory = SharpDX.Direct2D1.Factory;
using DXDevice = SharpDX.Direct3D11.Device;
using CreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;
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

		public RasterizerState GetCullBackRasterizerState(DXDevice device, CullMode cullMode)
		{
			RasterizerState state;
			if (rasterizerStates.TryGetValue(cullMode, out state))
				return state;
			var newState = new RasterizerState(device, GetRasterizer(cullMode));
			rasterizerStates.Add(cullMode, newState);
			return newState;
		}

		private readonly Dictionary<CullMode, RasterizerState> rasterizerStates =
			new Dictionary<CullMode, RasterizerState>();

		private static RasterizerStateDescription GetRasterizer(CullMode cullMode)
		{
			var rasterizerDescription = new RasterizerStateDescription
			{
				CullMode = cullMode,
				FillMode = FillMode.Solid,
				IsFrontCounterClockwise = true,
				DepthBias = 0,
				DepthBiasClamp = 0.0f,
				SlopeScaledDepthBias = 0.0f,
				IsDepthClipEnabled = true,
				IsScissorEnabled = false,
				//IsMultisampleEnabled = true,
				//IsAntialiasedLineEnabled = false
			};
			return rasterizerDescription;
		}

		public BlendState GetBlendState(DXDevice device, BlendMode blendMode)
		{
			BlendState state;
			if (blendStates.TryGetValue(blendMode, out state))
				return state;
			var description = new BlendStateDescription();
			description.RenderTarget[0] = GetRenderTargetBlendDescription(blendMode);
			var blendState = new BlendState(device, description);
			blendStates.Add(blendMode, blendState);
			return blendState;
		}

		private readonly Dictionary<BlendMode, BlendState> blendStates =
			new Dictionary<BlendMode, BlendState>();

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

		public DepthStencilState GetDepthStencilState(DXDevice device, bool depthEnabled)
		{
			DepthStencilState state;
			if (depthStencilStates.TryGetValue(depthEnabled, out state))
				return state;
			var description = new DepthStencilStateDescription
			{
				IsDepthEnabled = depthEnabled,
				DepthWriteMask = DepthWriteMask.All,
				DepthComparison = Comparison.Less,
				IsStencilEnabled = false,
				StencilReadMask = 0xFF,
				StencilWriteMask = 0xFF
			};
			var depthStencilState = new DepthStencilState(device, description);
			depthStencilStates.Add(depthEnabled, depthStencilState);
			return depthStencilState;
		}

		private readonly Dictionary<bool, DepthStencilState> depthStencilStates =
			new Dictionary<bool, DepthStencilState>();

		public virtual void Dispose()
		{
			foreach (var state in rasterizerStates)
				state.Value.Dispose();
			foreach (var state in blendStates)
				state.Value.Dispose();
			foreach (var state in depthStencilStates)
				state.Value.Dispose();
			d2DFactory.Dispose();
		}
	}
}