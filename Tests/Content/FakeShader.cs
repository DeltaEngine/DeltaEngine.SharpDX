using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Tests.Content
{
	public class FakeShader : Shader
	{
		//ncrunch: no coverage start
		public FakeShader(string contentName)
			: base(contentName) {}

		protected override void DisposeData() {}
		protected override void LoadData(Stream fileData) {}
		public override void SetModelViewProjectionMatrix(Matrix matrix) {}
		public override void SetJointMatrices(Matrix[] jointMatrices) {}
		public override void SetDiffuseTexture(Image texture) {}
		public override void SetLightmapTexture(Image texture) {}
		public override void SetLightPosition(Vector3D vector) {}
		public override void SetViewPosition(Vector3D vector) {}
		public override void Bind() {}
		public override void BindVertexDeclaration() {}
	}
}