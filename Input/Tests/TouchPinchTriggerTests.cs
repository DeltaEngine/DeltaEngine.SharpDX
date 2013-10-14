using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
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
			var trigger = new TouchPinchTrigger();
			var touch = Resolve<Touch>();
			new Command(() =>
			//ncrunch: no coverage start
			{
				ellipse.Center = touch.GetPosition(0);
				ellipse.Size = new Size(trigger.Distance * 0.5f);
			}).Add(trigger);
			//ncrunch: no coverage end
		}

		[Test, CloseAfterFirstFrame]
		public void PinchDistance()
		{
			var trigger = new TouchPinchTrigger();
			var touch = (MockTouch)Resolve<Touch>();
			touch.SetTouchState(0, State.Pressing, new Vector2D(0.4f, 0.1f));
			touch.SetTouchState(1, State.Pressed, new Vector2D(0.3f, 0.7f));
			touch.Update(new[] { trigger });
			Assert.AreEqual(0.608276188f, trigger.Distance);
		}

		[Test, CloseAfterFirstFrame]
		public void Creation()
		{
			Assert.DoesNotThrow(() => new TouchPinchTrigger(""));
			Assert.DoesNotThrow(() => new TouchPinchTrigger(null));
			Assert.Throws<TouchPinchTrigger.TouchPinchTriggerHasNoParameters>(
				() => new TouchPinchTrigger("Left"));
		}
	}
}