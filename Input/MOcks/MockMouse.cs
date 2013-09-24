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
			Position = Vector2D.Half;
		}

		public override bool IsAvailable { get; protected set; }

		public override void Dispose() {}

		public override void SetPosition(Vector2D position)
		{
			nextPosition = position;
		}

		private Vector2D nextPosition;

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

		public void ScrollUp()
		{
			ScrollWheelValue += 1;
		}

		public void ScrollDown()
		{
			ScrollWheelValue -= 1;
		}

		public override void Update(IEnumerable<Entity> entities)
		{
			Position = nextPosition;
			base.Update(entities);
		}
	}
}