using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Mocks
{
	public class MockShader : ShaderWithFormat
	{
		public MockShader(string contentName, Device device)
			: base(contentName)
		{
			this.device = device;
		}

		private readonly Device device;

		public MockShader(ShaderCreationData customShader, Device device)
			: base(customShader)
		{
			this.device = device;
			CallPublicImplementationMethodsToFixCoverage();
		}

		private void CallPublicImplementationMethodsToFixCoverage()
		{
			SetModelViewProjectionMatrix(Matrix.Identity);
			SetJointMatrices(new Matrix[0]);
			SetDiffuseTexture(null);
			SetLightmapTexture(null);
			SetLightPosition(Vector3D.Zero);
			SetViewPosition(Vector3D.Zero);
			Bind();
			BindVertexDeclaration();
		}

		protected override void DisposeData() {}
		public override void SetModelViewProjectionMatrix(Matrix matrix) {}
		public override void SetJointMatrices(Matrix[] jointMatrices) {}
		public override void SetDiffuseTexture(Image texture) {}
		public override void SetLightmapTexture(Image texture) {}
		public override void SetLightPosition(Vector3D vector) {}
		public override void SetViewPosition(Vector3D vector) {}

		public override void Bind()
		{
			device.Shader = this;
		}

		public override void BindVertexDeclaration() {}
		protected override void Create() {}
	}
}