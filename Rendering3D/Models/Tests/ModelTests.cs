using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Models.Tests
{
	public class ModelTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			window = Resolve<Window>();
		}

		private Window window;

		[Test]
		public void LoadInvalidModel()
		{
			Assert.Throws<ModelData.NoMeshesGivenNeedAtLeastOne>(
				() => new Model("InvalidModel", Vector3D.Zero));
		}

		[Test]
		public void RenderCubeModel()
		{
			window.BackgroundColor = Color.Gray;
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 2 * Vector3D.One;
			new Model("Cube", Vector3D.Zero);
		}

		[Test]
		public void RenderLightmapSceneModel()
		{
			window.BackgroundColor = Color.White;
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 5 * Vector3D.One;
			new Model("LightmapSimpleScene", Vector3D.Zero);
		}

		[Test]
		public void RayPick()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 5 * Vector3D.One;
			var cube = new Model(new ModelData(new Box(Vector3D.One, Color.Red)), Vector3D.Zero);
			var floor = new Plane(Vector3D.UnitZ, 0.0f);
			new Command(point =>
			{
				var ray = camera.ScreenPointToRay(point);
				Vector3D? intersect = floor.Intersect(ray);
				if (intersect != null)
					cube.Position = (Vector3D)intersect;
			}).Add(new MouseButtonTrigger(MouseButton.Left, State.Pressed));
		}
	}
}