using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchDragTriggerTests : TestWithMocksOrVisually
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

		[Test]
		public void DragTouchToCreateRectangles()
		{
			var rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			new Command((start, end, done) =>
			{
				rectangle.DrawArea = Rectangle.FromCorners(start, end);
				if (done)
					rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			}).Add(new TouchDragTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void DragTouch()
		{
			bool isFinished = false;
			new Command((start, end, done) => isFinished = done).Add(new TouchDragTrigger());
			SetTouchState(State.Pressing, Point.Zero);
			SetTouchState(State.Pressed, Point.One);
			SetTouchState(State.Releasing, Point.One);
			Assert.IsTrue(isFinished);
		}

		private void SetTouchState(State state, Point position)
		{
			if (touch == null)
				return; //ncrunch: no coverage
			touch.SetTouchState(0, state, position);
			AdvanceTimeAndUpdateEntities();
		}
	}
}