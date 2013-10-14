using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchHoldTriggerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			touch = Resolve<Touch>() as MockTouch;
			if (touch != null)
				touch.SetTouchState(0, State.Released, Vector2D.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockTouch touch;

		[Test]
		public void HoldOnRectangle()
		{
			var drawArea = new Rectangle(0.25f, 0.25f, 0.5f, 0.25f);
			new FilledRect(drawArea, Color.Blue);
			var trigger = new TouchHoldTrigger(drawArea);
			var counter = 0;
			var text = new FontText(Font.Default, "", drawArea.Move(new Vector2D(0.0f, 0.25f)));
			new Command(() => text.Text = "TouchHold Triggered " + ++counter + " times.").Add(trigger);
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new TouchHoldTrigger(Rectangle.One, 0.5f);
			Assert.AreEqual(Rectangle.One, trigger.HoldArea);
			Assert.AreEqual(0.5f, trigger.HoldTime);
		}

		[Test, CloseAfterFirstFrame]
		public void IsHovering()
		{
			var trigger = new TouchHoldTrigger(Rectangle.One, 0.5f);
			Assert.IsFalse(trigger.IsHovering());
			trigger.Elapsed = 1f;
			Assert.IsFalse(trigger.IsHovering());
		}

		[Test, CloseAfterFirstFrame]
		public void PessingInTheSamePositionWillMakeTriggerHover()
		{
			var trigger = new TouchHoldTrigger(Rectangle.One, 0.0001f);
			new Command(() => { }).Add(trigger);
			SetTouchState(State.Pressing, Vector2D.Half);
			SetTouchState(State.Pressed, Vector2D.Half);
			SetTouchState(State.Releasing, Vector2D.Half);
			Assert.IsTrue(trigger.IsHovering());
		}

		private void SetTouchState(State state, Vector2D position)
		{
			if (touch == null)
				return; //ncrunch: no coverage
			touch.SetTouchState(0, state, position);
			AdvanceTimeAndUpdateEntities();
		}
	}
}