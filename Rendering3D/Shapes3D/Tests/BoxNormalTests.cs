using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Models;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Shapes3D.Tests
{
	public class BoxNormalTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowNormalBox()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 1.5f * Vector3D.One;
			Resolve<Window>().BackgroundColor = Color.Gray;
			new Model(new ModelData(new BoxNormal(Vector3D.One)), Vector3D.Zero);
		}
	}
}