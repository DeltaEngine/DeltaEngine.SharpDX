using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
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
				touch.SetTouchState(0, State.Released, Vector2D.Zero);
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
			SetTouchState(State.Pressing, Vector2D.Zero);
			SetTouchState(State.Pressed, Vector2D.One);
			SetTouchState(State.Releasing, Vector2D.One);
			Assert.IsTrue(isFinished);
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