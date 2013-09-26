using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseFlickTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedCircleOnFlick()
		{
			new FontText(Font.Default, "Flick screen to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Vector2D.Half).Add(new MouseFlickTrigger());
			new Command(() => ellipse.Center = Vector2D.Zero).Add(new MouseButtonTrigger(State.Released));
		}

		[Test]
		public void FlickDetection()
		{
			var trigger = new MouseFlickTrigger();
			var mouse = (MockMouse)Resolve<Mouse>();
			mouse.SetButtonState(MouseButton.Left, State.Pressing);
			mouse.SetPosition(new Vector2D(0.5f, 0.5f));
			AdvanceTimeAndUpdateEntities();
			mouse.Update(new[] { trigger });
			Assert.IsFalse(trigger.WasInvoked);
			Assert.AreEqual(0, trigger.PressTime);
			Assert.AreEqual(new Vector2D(0.5f, 0.5f), trigger.StartPosition);
			mouse.SetButtonState(MouseButton.Left, State.Pressed);
			mouse.SetPosition(new Vector2D(0.52f, 0.5f));
			AdvanceTimeAndUpdateEntities();
			mouse.Update(new[] { trigger });
			Assert.IsFalse(trigger.WasInvoked);
			mouse.SetButtonState(MouseButton.Left, State.Releasing);
			mouse.SetPosition(new Vector2D(0.6f, 0.5f));
			AdvanceTimeAndUpdateEntities();
			mouse.Update(new[] { trigger });
			Assert.IsTrue(trigger.WasInvoked);
		}
	}
}