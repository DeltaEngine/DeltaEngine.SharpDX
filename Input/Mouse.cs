using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Provides the mouse position, mouse button states and allows to set the mouse position.
	/// </summary>
	public abstract class Mouse : InputDevice
	{
		public Vector2D Position { get; protected set; }
		public int ScrollWheelValue { get; protected set; }
		public State LeftButton { get; protected set; }
		public State MiddleButton { get; protected set; }
		public State RightButton { get; protected set; }
		public State X1Button { get; protected set; }
		public State X2Button { get; protected set; }

		public State GetButtonState(MouseButton button)
		{
			if (button == MouseButton.Right)
				return RightButton;
			if (button == MouseButton.Middle)
				return MiddleButton;
			if (button == MouseButton.X1)
				return X1Button;
			return button == MouseButton.X2 ? X2Button : LeftButton;
		}

		public abstract void SetPosition(Vector2D position);

		public override void Update(IEnumerable<Entity> entities)
		{
			if (IsAvailable)
				foreach (Entity entity in entities)
					InvokeTriggersForEntity(entity);
		}

		private void InvokeTriggersForEntity(Entity entity)
		{
			HandleTriggerOfType<MouseButtonTrigger>(entity);
			HandleTriggerOfType<MouseDragTrigger>(entity);
			HandleTriggerOfType<MouseDragDropTrigger>(entity);
			HandleTriggerOfType<MouseHoldTrigger>(entity);
			HandleTriggerOfType<MouseHoverTrigger>(entity);
			HandleTriggerOfType<MouseMovementTrigger>(entity);
			HandleTriggerOfType<MousePositionTrigger>(entity);
			HandleTriggerOfType<MouseTapTrigger>(entity);
			HandleTriggerOfType<MouseZoomTrigger>(entity);
			HandleTriggerOfType<MouseFlickTrigger>(entity);
		}

		private void HandleTriggerOfType<T>(Entity e) where T : class, MouseTrigger
		{
			var trigger = e as T;
			if (trigger != null)
				trigger.HandleWithMouse(this);
		}
	}
}