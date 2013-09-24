using DeltaEngine.Datatypes;
using DeltaEngine.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Cameras.Tests
{
	public class OrthoCameraTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void InitializeEntityRunner()
		{
			new MockEntitiesRunner(typeof(MockUpdateBehavior));
			camera = Camera.Use<OrthoCamera>();
			camera.Size = Size.One * 4.0f;
		}

		private OrthoCamera camera;

		[Test]
		public void CameraPosition()
		{
			Assert.AreEqual(Vector3D.Zero, camera.Position);
		}

		[Test]
		public void CameraSize()
		{
			Assert.AreEqual(4.0f, camera.Size.Width);
		}

		[Test]
		public void UpdateCameraSize()
		{
			camera.Size = new Size(6.0f);
			Assert.AreEqual(6.0f, camera.Size.Width);
			Assert.AreEqual(6.0f, camera.Size.Height);
		}

		[Test]
		public void UpdateCameraSizeAfterTimeInterval()
		{
			Assert.AreEqual(4.0f, camera.Size.Width);
			AdvanceTimeAndUpdateEntities(3.0f);
			camera.Size = new Size(6.0f);
			Assert.AreEqual(6.0f, camera.Size.Width);
		}
	}
}