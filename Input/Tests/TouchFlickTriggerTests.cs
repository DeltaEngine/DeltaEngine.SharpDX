using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchFlickTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnFlick()
		{
			new FontText(Font.Default, "Flick screen to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Point.Half).Add(new TouchFlickTrigger());
			new Command(() => ellipse.Center = Point.Zero).Add(new TouchPressTrigger(State.Released));
		}
	}
}