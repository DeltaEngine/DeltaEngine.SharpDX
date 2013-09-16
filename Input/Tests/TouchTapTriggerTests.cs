using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchTapTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnTap()
		{
			new FontText(Font.Default, "Tap screen to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Point.Half).Add(new TouchTapTrigger());
			new Command(() => ellipse.Center = Point.Zero).Add(new TouchPressTrigger(State.Released));
		}
	}
}