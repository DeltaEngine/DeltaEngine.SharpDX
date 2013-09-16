using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.Rendering.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Models.Tests
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
				() => new Model("InvalidModel", Vector.Zero));
		}

		[Test]
		public void RenderCubeModel()
		{
			window.BackgroundColor = Color.Gray;
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 2 * Vector.One;
			new Model("Cube", Vector.Zero);
		}

		[Test]
		public void RenderLightmapSceneModel()
		{
			window.BackgroundColor = Color.White;
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 5 * Vector.One;
			new Model("LightmapSimpleScene", Vector.Zero);
		}

		[Test]
		public void RayPick()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 5 * Vector.One;
			var cube = new Model(new ModelData(new Box(Vector.One, Color.Red)), Vector.Zero);
			var floor = new Plane(Vector.UnitY, 0.0f);
			new Command(point =>
			{
				var ray = camera.ScreenPointToRay(point);
				Vector? intersect = floor.Intersect(ray);
				if (intersect != null)
					cube.Position = (Vector)intersect;
			}).Add(new MouseButtonTrigger(MouseButton.Left, State.Pressed));
		}
	}
}