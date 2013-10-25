using System.IO;

namespace DeltaEngine.Graphics.Mocks
{
	/// <summary>
	/// Mock geometry used in unit tests.
	/// </summary>
	public class MockGeometry : Geometry
	{
		//ncrunch: no coverage start
		public MockGeometry(string contentName, Device device)
			: base(contentName) {}

		protected MockGeometry(GeometryCreationData creationData, Device device)
			: base(creationData) {}

		protected override void LoadData(Stream fileData) {}
		public override void Draw() {}
		protected override void SetNativeData(byte[] vertexData, short[] indices) {}
		protected override void DisposeData() {}
	}
}