using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Input.Mocks
{
	/// <summary>
	/// Mock mouse for unit testing.
	/// </summary>
	public sealed class MockMouse : Mouse
	{
		public MockMouse()
		{
			IsAvailable = true;
			Position = Point.Half;
		}

		public override bool IsAvailable { get; protected set; }

		public override void Dispose() {}

		public override void SetPosition(Point newPosition)
		{
			nextPosition = newPosition;
		}

		private Point nextPosition;

		public void SetButtonState(MouseButton button, State state)
		{
			if (button == MouseButton.Right)
				RightButton = state;
			else if (button == MouseButton.Middle)
				MiddleButton = state;
			else if (button == MouseButton.X1)
				X1Button = state;
			else if (button == MouseButton.X2)
				X2Button = state;
			else
				LeftButton = state;
		}

		public void SetScrollWheelValue(int value)
		{
			ScrollWheelValue = value;
		}

		public override void Update(IEnumerable<Entity> entities)
		{
			Position = nextPosition;
			base.Update(entities);
		}
	}
}