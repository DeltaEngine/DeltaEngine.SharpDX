using System;
using System.Collections.Generic;
using DeltaEngine.Commands;
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
		public Point Position { get; protected set; }
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
			if (button == MouseButton.X2)
				return X2Button;
			return LeftButton;
		}

		public abstract void SetPosition(Point newPosition);

		public override void Update(IEnumerable<Entity> entities)
		{
			if (IsAvailable)
				foreach (Entity entity in entities)
					InvokeTriggersForEntity(entity);
		}

		private void InvokeTriggersForEntity(Entity entity)
		{
			TryInvokeTriggerOfType<MouseButtonTrigger>(entity, IsMouseButtonTriggered);
			TryInvokeTriggerOfType<MouseDragTrigger>(entity, IsMouseDragTriggered);
			TryInvokeTriggerOfType<MouseDragDropTrigger>(entity, IsMouseDragDropTriggered);
			TryInvokeTriggerOfType<MouseHoldTrigger>(entity, IsMouseHoldTriggered);
			TryInvokeTriggerOfType<MouseHoverTrigger>(entity, IsMouseHoverTriggered);
			TryInvokeTriggerOfType<MouseMovementTrigger>(entity, IsMouseMovementTriggered);
			TryInvokeTriggerOfType<MousePositionTrigger>(entity, IsMousePositionTriggered);
			TryInvokeTriggerOfType<MouseTapTrigger>(entity, IsMouseTapTriggered);
			TryInvokeTriggerOfType<MouseZoomTrigger>(entity, IsMouseZoomTriggered);
		}

		private static void TryInvokeTriggerOfType<T>(Entity entity, Func<T, bool> triggeredCode)
			where T : Trigger
		{
			var trigger = entity as T;
			if (trigger != null)
				trigger.WasInvoked = triggeredCode.Invoke(trigger);
		}

		private bool IsMouseButtonTriggered(MouseButtonTrigger trigger)
		{
			trigger.Position = Position;
			return GetButtonState(trigger.Button) == trigger.State;
		}

		private bool IsMouseDragTriggered(MouseDragTrigger trigger)
		{
			if (GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartPosition = Position;
			else if (trigger.StartPosition != Point.Unused &&
				GetButtonState(trigger.Button) != State.Released)
			{
				if (trigger.StartPosition.DistanceTo(Position) > PositionEpsilon)
				{
					trigger.Position = Position;
					trigger.DoneDragging = GetButtonState(trigger.Button) == State.Releasing;
					return true;
				}
			}
			else
			{
				trigger.StartPosition = Point.Unused;
				trigger.DoneDragging = false;
			}
			return false;
		}

		private const float PositionEpsilon = 0.0025f;

		private bool IsMouseDragDropTriggered(MouseDragDropTrigger trigger)
		{
			if (trigger.StartArea.Contains(Position) && GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartDragPosition = Position;
			else if (trigger.StartDragPosition != Point.Unused &&
				GetButtonState(trigger.Button) != State.Released)
			{
				if (trigger.StartDragPosition.DistanceTo(Position) > PositionEpsilon)
					return true;
			}
			else
				trigger.StartDragPosition = Point.Unused;
			return false;
		}

		private bool IsMouseHoldTriggered(MouseHoldTrigger trigger)
		{
			if (GetButtonState(trigger.Button) == State.Pressing)
				trigger.StartPosition = Position;
			trigger.Position = Position;
			if (CheckHoverState(trigger))
				return trigger.IsHovering();
			trigger.Elapsed = 0.0f;
			return false;
		}

		private bool CheckHoverState(MouseHoldTrigger trigger)
		{
			return trigger.HoldArea.Contains(trigger.StartPosition) &&
				GetButtonState(trigger.Button) == State.Pressed &&
				trigger.StartPosition.DistanceTo(Position) < PositionEpsilon;
		}

		private bool IsMouseHoverTriggered(MouseHoverTrigger trigger)
		{
			if (trigger.LastPosition.DistanceTo(Position) < PositionEpsilon)
				return trigger.IsHovering();
			trigger.LastPosition = Position;
			trigger.Elapsed = 0.0f;
			return false;
		}

		private bool IsMouseMovementTriggered(MouseMovementTrigger trigger)
		{
			bool changedPosition = trigger.Position != Position && trigger.Position != Point.Unused;
			trigger.Position = Position;
			return changedPosition;
		}

		private bool IsMousePositionTriggered(MousePositionTrigger trigger)
		{
			var isButton = GetButtonState(trigger.Button) == trigger.State;
			bool hasPositionChanged = trigger.Position != Position && trigger.Position != Point.Unused &&
				ScreenSpace.Current.Viewport.Contains(trigger.Position);
			trigger.Position = Position;
			return isButton && hasPositionChanged;
		}

		private bool IsMouseTapTriggered(MouseTapTrigger trigger)
		{
			bool wasJustStartedPressing = trigger.LastState == State.Pressing;
			State currentState = GetButtonState(trigger.Button);
			var isNowReleased = currentState == State.Releasing;
			trigger.LastState = currentState;
			return isNowReleased && wasJustStartedPressing;
		}

		private bool IsMouseZoomTriggered(MouseZoomTrigger trigger)
		{
			int currentScrollValueDifference = ScrollWheelValue - trigger.LastScrollWheelValue;
			trigger.LastScrollWheelValue = ScrollWheelValue;
			if (currentScrollValueDifference > 0)
				trigger.ZoomAmount = 1;
			else if (currentScrollValueDifference < 0)
				trigger.ZoomAmount = -1;
			else
				trigger.ZoomAmount = 0;
			return trigger.ZoomAmount != 0;
		}
	}
}