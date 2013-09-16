using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.Rendering.Models;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Shapes3D.Tests
{
	public class BoxNormalTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowNormalBox()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = 1.5f * Vector.One;
			Resolve<Window>().BackgroundColor = Color.Gray;
			new Model(new ModelData(new BoxNormal(Vector.One)), Vector.Zero);
		}
	}
}