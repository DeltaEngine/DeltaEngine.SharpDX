using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchDragDropTriggerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			touch = Resolve<Touch>() as MockTouch;
			if (touch != null)
				touch.SetTouchState(0, State.Released, Point.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockTouch touch;

		[Test, CloseAfterFirstFrame]
		public void DragDropTouchOutsideStartArea()
		{
			Point startPoint = -Point.One;
			new Command(position =>
			{
				startPoint = position;
			}).Add(new TouchDragDropTrigger(Rectangle.HalfCentered));
			SetTouchState(State.Pressing, Point.Zero);
			SetTouchState(State.Pressed, Point.One);
			SetTouchState(State.Releasing, Point.One);
			SetTouchState(State.Released, Point.One);
			Assert.AreEqual(-Point.One, startPoint);
		}

		private void SetTouchState(State state, Point position)
		{
			if (touch == null)
				return; //ncrunch: no coverage
			touch.SetTouchState(0, state, position);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void DragDropTouchInsideStartArea()
		{
			Point startPoint = -Point.One;
			new Command(position =>
			{
				startPoint = position;
			}).Add(new TouchDragDropTrigger(Rectangle.HalfCentered));
			SetTouchState(State.Pressing, Point.Half);
			SetTouchState(State.Pressed, Point.One);
			SetTouchState(State.Releasing, Point.One);
			Assert.AreEqual(Point.Half, startPoint);
		}
			
		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new TouchDragDropTrigger(Rectangle.One);
			Assert.AreEqual(Rectangle.One, trigger.StartArea);
			Assert.AreEqual(Point.Unused, trigger.StartDragPosition);
		}
	}
}