using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchDoubleTapTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnTouch()
		{
			new FontText(Font.Default, "Touch screen to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Vector2D.Half).Add(new TouchDoubleTapTrigger());
			new Command(() => ellipse.Center = Vector2D.Zero).Add(new TouchPressTrigger(State.Released));
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			Assert.DoesNotThrow(() => new TouchDoubleTapTrigger());
		}
	}
}