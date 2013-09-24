using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchPinchTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnPinch()
		{
			new FontText(Font.Default, "Pinch screen to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Vector2D.Half).Add(new TouchPinchTrigger());
			new Command(() => ellipse.Center = Vector2D.Zero).Add(new TouchPressTrigger(State.Released));
		}
	}
}