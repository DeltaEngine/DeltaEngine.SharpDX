using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchPositionTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnTouchWithMovement()
		{
			var ellipse = new Ellipse(new Rectangle(-0.1f, -0.1f, 0.1f, 0.1f), Color.Red);
			new Command(pos => ellipse.Center = pos).Add(new TouchPositionTrigger(State.Pressed));
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new TouchPositionTrigger(State.Pressed);
			Assert.AreEqual(State.Pressed, trigger.State);
		}
	}
}