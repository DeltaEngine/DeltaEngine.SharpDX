using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
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
			new Command(() => ellipse.Center = Vector2D.Half).Add(new TouchFlickTrigger());
			new Command(() => ellipse.Center = Vector2D.Zero).Add(new TouchPressTrigger(State.Released));
		}
	}
}