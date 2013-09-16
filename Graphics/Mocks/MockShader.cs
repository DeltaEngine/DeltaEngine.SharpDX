using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Mocks
{
	public class MockShader : ShaderWithFormat
	{
		protected MockShader(string contentName, Device device)
			: base(contentName)
		{
			if (device == null)
				throw new NeedDeviceForShaderCreation(); //ncrunch: no coverage
		}

		public class NeedDeviceForShaderCreation : Exception { }

		private MockShader(ShaderCreationData customShader, Device device)
			: base(customShader)
		{
			if (device == null)
				throw new NeedDeviceForShaderCreation(); //ncrunch: no coverage
		}

		protected override void DisposeData() { }
		public override void SetModelViewProjectionMatrix(Matrix matrix) { }
		public override void SetJointMatrices(Matrix[] jointMatrices) { }
		public override void SetDiffuseTexture(Image texture) { }
		public override void SetLightmapTexture(Image texture) { }
		public override void Bind() { }
		public override void BindVertexDeclaration() { }
		protected override void Create() { }
	}
}