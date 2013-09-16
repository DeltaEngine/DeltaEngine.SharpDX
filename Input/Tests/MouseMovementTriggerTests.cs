using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseMovementTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void MoveMouseToUpdatePositionOfCircle()
		{
			var ellipse = new Ellipse(new Rectangle(-0.1f, -0.1f, 0.1f, 0.1f), Color.Red);
			new Command(pos => ellipse.Center = pos).Add(new MouseMovementTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void UpdatePosition()
		{
			float xPosition = 0.0f;
			new Command(pos => xPosition += pos.X).Add(new MouseMovementTrigger());
			Assert.AreEqual(0.0f, xPosition);
			var mockMouse = Resolve<Mouse>() as MockMouse;
			if (mockMouse == null)
				return; //ncrunch: no coverage
			AdvanceTimeAndUpdateEntities();
			mockMouse.SetPosition(new Point(1.5f, 1.5f));
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(1.5f, xPosition);
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new MouseMovementTrigger();
			Assert.AreEqual(Point.Zero, trigger.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateFromString()
		{
			var trigger = new MouseMovementTrigger("");
			Assert.AreEqual(Point.Zero, trigger.Position);
			Assert.Throws<MouseMovementTrigger.MouseMovementTriggerHasNoParameters>(
				() => new MouseMovementTrigger("a"));
		}
	}
}