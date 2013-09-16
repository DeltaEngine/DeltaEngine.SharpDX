using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Graphics.Mocks
{
	/// <summary>
	/// Mock device used in unit tests.
	/// </summary>
	public class MockDevice : Device
	{
		public MockDevice(Window window)
			: base(window) {}

		public override void Dispose() { }
		public override void Clear() {}
		public override void Present() {}
		public override void EnableDepthTest() {}
		public override void SetViewport(Size viewportSize) {}
		public override void DisableDepthTest() {}
		public override void SetBlendMode(BlendMode blendMode) {}

		public override CircularBuffer CreateCircularBuffer(ShaderWithFormat shader,
			BlendMode blendMode, VerticesMode drawMode = VerticesMode.Triangles)
		{
			return new MockCircularBuffer(this, shader, blendMode, drawMode);
		}
	}
}