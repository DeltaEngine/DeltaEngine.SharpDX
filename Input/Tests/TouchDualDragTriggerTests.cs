using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class TouchDualDragTriggerTests : TestWithMocksOrVisually
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
			new Command((start, end, done) => //ncrunch: no coverage start
			{
				rectangle.DrawArea = Rectangle.FromCorners(start, end);
				if (done)
					rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			}).Add(new TouchDualDragTrigger());
			//ncrunch: no coverage end
		}

		[Test, CloseAfterFirstFrame]
		public void DragTouch()
		{
			bool isFinished = false;
			new Command((start, end, done) => isFinished = done).Add(new TouchDualDragTrigger());
			SetTouchState(0, State.Pressing, Vector2D.Zero);
			SetTouchState(1, State.Pressing, Vector2D.Zero);
			SetTouchState(0, State.Pressed, Vector2D.One);
			SetTouchState(1, State.Pressed, Vector2D.One);
			SetTouchState(0, State.Releasing, Vector2D.One);
			SetTouchState(1, State.Releasing, Vector2D.One);
			Assert.IsTrue(isFinished);
		}

		private void SetTouchState(int index, State state, Vector2D position)
		{
			if (touch == null)
				return; //ncrunch: no coverage
			touch.SetTouchState(index, state, position);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new TouchDualDragTrigger("");
			Assert.AreEqual(DragDirection.Free, trigger.Direction);
			trigger = new TouchDualDragTrigger("Horizontal");
			Assert.AreEqual(DragDirection.Horizontal, trigger.Direction);
			trigger = new TouchDualDragTrigger("Vertical");
			Assert.AreEqual(DragDirection.Vertical, trigger.Direction);
		}
	}
}