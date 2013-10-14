using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.ScreenSpaces;

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
			HandleMouseButtonTrigger(entity);
			HandleMouseDragTrigger(entity);
			HandleMouseDragDropTrigger(entity);
			HandleMouseHoldTrigger(entity);
			HandleMouseHoverTrigger(entity);
			HandleMouseMovementTrigger(entity);
			HandleMousePositionTrigger(entity);
			HandleMouseTapTrigger(entity);
			HandleMouseZoomTrigger(entity);
			HandleMouseFlickTrigger(entity);
		}

		private void HandleMouseButtonTrigger(Entity entity)
		{
			var trigger = entity as MouseButtonTrigger;
			if (trigger == null)
				return;
			trigger.Position = Position;
			if (GetButtonState(trigger.Button) == trigger.State &&
				ScreenSpace.Current.Viewport.Contains(Position))
				trigger.Invoke();
		}

		private void HandleMouseDragTrigger(Entity entity)
		{
			var trigger = entity as MouseDragTrigger;
			if (trigger == null)
				return;
			if (GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartPosition = Position;
			else if (trigger.StartPosition != Vector2D.Unused &&
				GetButtonState(trigger.Button) != State.Released)
			{
				var movementDirection = trigger.StartPosition.DirectionTo(Position);
				if (movementDirection.Length > PositionEpsilon)
				{
					if ((trigger.Direction == DragDirection.Horizontal &&
						Math.Abs(movementDirection.Y) < AllowedDragDirectionOffset) ||
						(trigger.Direction == DragDirection.Vertical &&
							Math.Abs(movementDirection.X) < AllowedDragDirectionOffset) ||
						trigger.Direction == DragDirection.Free)
					{
						trigger.Position = Position;
						trigger.DoneDragging = GetButtonState(trigger.Button) == State.Releasing;
					}
					if (ScreenSpace.Current.Viewport.Contains(Position))
						trigger.Invoke();
				}
			}
			else
			{
				trigger.StartPosition = Vector2D.Unused;
				trigger.DoneDragging = false;
			}
		}

		private const float PositionEpsilon = 0.0025f;
		private const float AllowedDragDirectionOffset = 0.01f;

		private void HandleMouseDragDropTrigger(Entity entity)
		{
			var trigger = entity as MouseDragDropTrigger;
			if (trigger == null)
				return;
			if (trigger.StartArea.Contains(Position) && GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartDragPosition = Position;
			else if (trigger.StartDragPosition != Vector2D.Unused &&
				GetButtonState(trigger.Button) != State.Released)
				if (trigger.StartDragPosition.DistanceTo(Position) > PositionEpsilon)
					trigger.Invoke();
				else
					trigger.StartDragPosition = Vector2D.Unused;
		}

		private void HandleMouseHoldTrigger(Entity entity)
		{
			var trigger = entity as MouseHoldTrigger;
			if (trigger == null)
				return;
			if (GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartPosition = Position;
			trigger.Position = Position;
			if (CheckHoverState(trigger))
			{
				if (trigger.IsHovering())
					trigger.Invoke();
			}
			else
				trigger.Elapsed = 0.0f;
		}

		private bool CheckHoverState(MouseHoldTrigger trigger)
		{
			return trigger.HoldArea.Contains(trigger.StartPosition) &&
				GetButtonState(trigger.Button) == State.Pressed &&
				trigger.StartPosition.DistanceTo(Position) < PositionEpsilon;
		}

		private void HandleMouseHoverTrigger(Entity entity)
		{
			var trigger = entity as MouseHoverTrigger;
			if (trigger == null)
				return;
			if (trigger.LastPosition.DistanceTo(Position) < PositionEpsilon)
			{
				if (trigger.IsHovering())
					trigger.Invoke();
			}
			else
			{
				trigger.LastPosition = Position;
				trigger.Elapsed = 0.0f;
			}
		}

		private void HandleMouseMovementTrigger(Entity entity)
		{
			var trigger = entity as MouseMovementTrigger;
			if (trigger == null)
				return;
			if (trigger.Position == Position)
				return;
			trigger.Position = Position;
			if (ScreenSpace.Current.Viewport.Contains(Position))
				trigger.Invoke();
		}

		private void HandleMousePositionTrigger(Entity entity)
		{
			var trigger = entity as MousePositionTrigger;
			if (trigger == null)
				return;
			if (trigger.Position == Position)
				return;
			var isButton = GetButtonState(trigger.Button) == trigger.State;
			trigger.Position = Position;
			if (isButton && ScreenSpace.Current.Viewport.Contains(Position))
				trigger.Invoke();
		}

		private void HandleMouseTapTrigger(Entity entity)
		{
			var trigger = entity as MouseTapTrigger;
			if (trigger == null)
				return;
			bool wasJustStartedPressing = trigger.LastState == State.Pressing;
			State currentState = GetButtonState(trigger.Button);
			var isNowReleased = currentState == State.Releasing;
			trigger.LastState = currentState;
			if (isNowReleased && wasJustStartedPressing)
				trigger.Invoke();
		}

		private void HandleMouseZoomTrigger(Entity entity)
		{
			var trigger = entity as MouseZoomTrigger;
			if (trigger == null)
				return;
			int currentScrollValueDifference = ScrollWheelValue - trigger.LastScrollWheelValue;
			trigger.LastScrollWheelValue = ScrollWheelValue;
			if (currentScrollValueDifference > 0)
				trigger.ZoomAmount = 1;
			else if (currentScrollValueDifference < 0)
				trigger.ZoomAmount = -1;
			else
				trigger.ZoomAmount = 0;
			if (trigger.ZoomAmount != 0)
				trigger.Invoke();
		}

		private void HandleMouseFlickTrigger(Entity entity)
		{
			var trigger = entity as MouseFlickTrigger;
			if (trigger == null)
				return;
			if (LeftButton == State.Pressing)
				SetFlickPositionAndResetTime(trigger, Position);
			else if (trigger.StartPosition != Vector2D.Unused && LeftButton != State.Released)
			{
				trigger.PressTime += Time.Delta;
				if (LeftButton == State.Releasing &&
					trigger.StartPosition.DistanceTo(Position) > PositionEpsilon && trigger.PressTime < 0.3f)
					trigger.Invoke();
			}
			else
				SetFlickPositionAndResetTime(trigger, Vector2D.Unused);
		}

		private static void SetFlickPositionAndResetTime(MouseFlickTrigger trigger, Vector2D position)
		{
			trigger.StartPosition = position;
			trigger.PressTime = 0;
		}
	}
}