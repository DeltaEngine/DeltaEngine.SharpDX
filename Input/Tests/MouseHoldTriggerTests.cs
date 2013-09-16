using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseHoldTriggerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Point.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockMouse mouse;

		[Test]
		public void HoldLeftClickOnRectangle()
		{
			var drawArea = new Rectangle(0.25f, 0.25f, 0.5f, 0.25f);
			new FilledRect(drawArea, Color.Blue);
			var trigger = new MouseHoldTrigger(drawArea);
			var counter = 0;
			var text = new FontText(Font.Default, "", drawArea.Move(new Point(0.0f, 0.25f)));
			new Command(() => text.Text = "MouseHold Triggered " + ++counter + " times.").Add(trigger);
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new MouseHoldTrigger(Rectangle.One, 0.5f, MouseButton.Right);
			Assert.AreEqual(Rectangle.One, trigger.HoldArea);
			Assert.AreEqual(0.5f, trigger.HoldTime);
			Assert.AreEqual(MouseButton.Right, trigger.Button);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateFromString()
		{
			var trigger = new MouseHoldTrigger("1 2 3 4 5.5 Right");
			Assert.AreEqual(new Rectangle(1, 2, 3, 4), trigger.HoldArea);
			Assert.AreEqual(5.5f, trigger.HoldTime);
			Assert.AreEqual(MouseButton.Right, trigger.Button);
			Assert.Throws<MouseHoldTrigger.CannotCreateMouseHoldTriggerWithoutHoldArea>(
				() => new MouseHoldTrigger("1 2 3"));
		}

		[Test, CloseAfterFirstFrame]
		public void HoldMouseOutsideHoldArea()
		{
			Point mousePosition = -Point.One;
			new Command(position => { mousePosition = position; }).Add(
				new MouseHoldTrigger(Rectangle.HalfCentered));
			SetMouseState(State.Pressing, Point.Zero);
			SetMouseState(State.Pressed, Point.Zero);
			AdvanceTimeAndUpdateEntities(1.05f);
			Assert.AreEqual(-Point.One, mousePosition);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void HoldMouseInsideHoldArea()
		{
			Point mousePosition = -Point.One;
			new Command(position => { mousePosition = position; }).Add(
				new MouseHoldTrigger(Rectangle.HalfCentered));
			SetMouseState(State.Pressing, Point.Half);
			SetMouseState(State.Pressed, Point.Half);
			AdvanceTimeAndUpdateEntities(1.05f);
			Assert.AreEqual(Point.Half, mousePosition);
		}

		[Test, CloseAfterFirstFrame]
		public void MoveMouseInsideHoldArea()
		{
			Point mousePosition = -Point.One;
			new Command(position => { mousePosition = position; }).Add(
				new MouseHoldTrigger(Rectangle.HalfCentered));
			SetMouseState(State.Pressing, Point.Half);
			SetMouseState(State.Pressed, Point.Half);
			AdvanceTimeAndUpdateEntities(0.5f);
			SetMouseState(State.Pressed, new Point(0.6f, 0.6f));
			AdvanceTimeAndUpdateEntities(0.5f);
			Assert.AreEqual(Point.Half, mousePosition);
		}
	}
}