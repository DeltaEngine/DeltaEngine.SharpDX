﻿using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using DXDevice = SharpDX.Direct3D11.Device;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Simplifies DirectX buffer creation
	/// </summary>
	public class SharpDXBuffer : Buffer
	{
		public SharpDXBuffer(DXDevice nativeDevice, int sizeInBytes, BindFlags flags)
			: base(nativeDevice, sizeInBytes, ResourceUsage.Dynamic, flags, CpuAccessFlags.Write,
				ResourceOptionFlags.None, 0) {}
	}
}