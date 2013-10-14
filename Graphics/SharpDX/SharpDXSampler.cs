using SharpDX.Direct3D11;
using DXDevice = SharpDX.Direct3D11.Device;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Simplifies DirectX sampler creation
	/// </summary>
	public class SharpDXSampler : SamplerState
	{
		public SharpDXSampler(DXDevice nativeDevice, Filter filter, TextureAddressMode textureAddressMode)
			: base(nativeDevice, BuildFilterDescription(filter, textureAddressMode)) {}

		protected static SamplerStateDescription BuildFilterDescription(Filter filter,
			TextureAddressMode textureAddressMode)
		{
			var samplerDescriptor = SamplerStateDescription.Default();
			samplerDescriptor.Filter = filter;
			samplerDescriptor.AddressU = textureAddressMode;
			samplerDescriptor.AddressV = textureAddressMode;
			samplerDescriptor.AddressW = textureAddressMode;
			return samplerDescriptor;
		}
	}
}