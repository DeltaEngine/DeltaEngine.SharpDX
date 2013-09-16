using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseDragDropTriggerTests : TestWithMocksOrVisually
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

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new MouseDragDropTrigger(Rectangle.One, MouseButton.Right);
			Assert.AreEqual(Rectangle.One, trigger.StartArea);
			Assert.AreEqual(MouseButton.Right, trigger.Button);
			Assert.AreEqual(Point.Unused, trigger.StartDragPosition);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateFromString()
		{
			var trigger = new MouseDragDropTrigger("1.1 2.2 3.3 4.4 Right");
			Assert.AreEqual(new Rectangle(1.1f, 2.2f, 3.3f, 4.4f), trigger.StartArea);
			Assert.AreEqual(MouseButton.Right, trigger.Button);
			Assert.AreEqual(Point.Unused, trigger.StartDragPosition);
			Assert.Throws<MouseDragDropTrigger.CannotCreateMouseDragDropTriggerWithoutStartArea>(
				() => new MouseDragDropTrigger("1 2 3"));
		}

		[Test, CloseAfterFirstFrame]
		public void DragDropOutsideStartArea()
		{
			Point startPoint = -Point.One;
			new Command(position =>
			{
				startPoint = position;
			}).Add(new MouseDragDropTrigger(Rectangle.HalfCentered, MouseButton.Left));
			SetMouseState(State.Pressing, Point.Zero);
			SetMouseState(State.Pressed, Point.One);
			SetMouseState(State.Releasing, Point.One);
			SetMouseState(State.Released, Point.One);
			Assert.AreEqual(-Point.One, startPoint);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetButtonState(MouseButton.Left, state);
			mouse.SetPosition(position);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void DragDropInsideStartArea()
		{
			Point startPoint = -Point.One;
			new Command(position =>
			{
				startPoint = position;
			}).Add(new MouseDragDropTrigger(Rectangle.HalfCentered, MouseButton.Left));
			SetMouseState(State.Pressing, Point.Half);
			SetMouseState(State.Pressed, Point.One);
			SetMouseState(State.Releasing, Point.One);
			Assert.AreEqual(Point.Half, startPoint);
		}
	}
}