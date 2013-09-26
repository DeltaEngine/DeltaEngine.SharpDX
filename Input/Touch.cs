using System;
using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Provides a way to fetch the current input values from a Touch device.
	/// </summary>
	public abstract class Touch : InputDevice
	{
		public abstract Vector2D GetPosition(int touchIndex);
		public abstract State GetState(int touchIndex);

		public override void Update(IEnumerable<Entity> entities)
		{
			if (IsAvailable)
				foreach (Entity entity in entities)
					InvokeTriggersForEntity(entity);
		}

		private void InvokeTriggersForEntity(Entity entity)
		{
			TryInvokeTriggerOfType<TouchPressTrigger>(entity, IsTouchPressTriggered);
			TryInvokeTriggerOfType<TouchMovementTrigger>(entity, IsTouchMovementTriggered);
			TryInvokeTriggerOfType<TouchPositionTrigger>(entity, IsTouchPositionTriggered);
			TryInvokeTriggerOfType<TouchTapTrigger>(entity, IsTouchTapTriggered);
			TryInvokeTriggerOfType<TouchDragTrigger>(entity, IsTouchDragTriggered);
			TryInvokeTriggerOfType<TouchDragDropTrigger>(entity, IsTouchDragDropTriggered);
			TryInvokeTriggerOfType<TouchHoldTrigger>(entity, IsTouchHoldTriggered);
			TryInvokeTriggerOfType<TouchPinchTrigger>(entity, IsTouchPinchTriggered);
			TryInvokeTriggerOfType<TouchRotateTrigger>(entity, IsTouchRotateTriggered);
			TryInvokeTriggerOfType<TouchFlickTrigger>(entity, IsTouchFlickTriggered);
		}

		private static void TryInvokeTriggerOfType<T>(Entity entity, Func<T, bool> triggeredCode)
			where T : Trigger
		{
			var trigger = entity as T;
			if (trigger != null)
				trigger.WasInvoked = triggeredCode.Invoke(trigger);
		}

		private bool IsTouchPressTriggered(TouchPressTrigger trigger)
		{
			return GetState(0) == trigger.State;
		}

		private bool IsTouchMovementTriggered(TouchMovementTrigger trigger)
		{
			bool changedPosition = trigger.Position != GetPosition(0) &&
				trigger.Position != Vector2D.Unused;
			trigger.Position = GetPosition(0);
			return changedPosition;
		}

		private bool IsTouchPositionTriggered(TouchPositionTrigger trigger)
		{
			var isButton = GetState(0) == trigger.State;
			bool hasPositionChanged = trigger.Position != GetPosition(0) &&
				trigger.Position != Vector2D.Unused &&
				ScreenSpace.Current.Viewport.Contains(trigger.Position);
			trigger.Position = GetPosition(0);
			return isButton && hasPositionChanged;
		}

		private bool IsTouchTapTriggered(TouchTapTrigger trigger)
		{
			bool wasJustStartedPressing = trigger.LastState == State.Pressing;
			State currentState = GetState(0);
			var isNowReleased = currentState == State.Releasing;
			trigger.LastState = currentState;
			return isNowReleased && wasJustStartedPressing;
		}

		private bool IsTouchFlickTriggered(TouchFlickTrigger trigger)
		{
			if (GetState(0) == State.Pressing)
			{
				trigger.StartPosition = GetPosition(0);
				trigger.PressTime = 0;
			}
			else if (trigger.StartPosition != Vector2D.Unused && GetState(0) != State.Released)
			{
				trigger.PressTime += Time.Delta;
				if (GetState(0) == State.Releasing &&
					trigger.StartPosition.DistanceTo(GetPosition(0)) > PositionEpsilon)
					return trigger.PressTime < 0.3f;
			}
			else
			{
				trigger.StartPosition = Vector2D.Unused;
				trigger.PressTime = 0;
			}
			return false;
		}

		private bool IsTouchDragTriggered(TouchDragTrigger trigger)
		{
			if (GetState(0) == State.Pressing)
				trigger.StartPosition = GetPosition(0);
			else if (trigger.StartPosition != Vector2D.Unused &&
				GetState(0) != State.Released)
			{
				var movementDirection = trigger.StartPosition.DirectionTo(GetPosition(0));
				if (movementDirection.Length > PositionEpsilon)
				{
					if ((trigger.Direction == DragDirection.Horizontal &&
						Math.Abs(movementDirection.Y) < AllowedDragDirectionOffset) ||
						(trigger.Direction == DragDirection.Vertical &&
							Math.Abs(movementDirection.X) < AllowedDragDirectionOffset) ||
						trigger.Direction == DragDirection.Free)
					{
						trigger.Position = GetPosition(0);
						trigger.DoneDragging = GetState(0) == State.Releasing;
					}
					return true;
				}
			}
			else
			{
				trigger.StartPosition = Vector2D.Unused;
				trigger.DoneDragging = false;
			}
			return false;
		}

		private const float PositionEpsilon = 0.0025f;
		private const float AllowedDragDirectionOffset = 0.01f;

		private bool IsTouchPinchTriggered(TouchPinchTrigger trigger)
		{
			if (GetState(0) >= State.Pressing && GetState(1) >= State.Pressing)
			{
				trigger.Distance = Math.Abs((GetPosition(1) - GetPosition(0)).Length);
				return true;
			}
			trigger.Distance = 0f;
			return false;
		}

		private bool IsTouchRotateTriggered(TouchRotateTrigger trigger)
		{
			if (GetState(0) >= State.Pressing && GetState(1) >= State.Pressing)
			{
				var vector = (GetPosition(1) - GetPosition(0));
				vector = vector / vector.Length;
				trigger.Angle = (float)Math.Atan2(vector.X, vector.Y);
				if (trigger.Angle < 0)
					trigger.Angle += (float)(2 * Math.PI);
				return true;
			}
			trigger.Angle = 0f;
			return false;
		}

		private bool IsTouchDragDropTriggered(TouchDragDropTrigger trigger)
		{
			var position = GetPosition(0);
			if (trigger.StartArea.Contains(position) && GetState(0) == State.Pressing)
				trigger.StartDragPosition = position;
			else if (trigger.StartDragPosition != Vector2D.Unused && GetState(0) != State.Released)
			{
				if (trigger.StartDragPosition.DistanceTo(position) > PositionEpsilon)
					return true;
			}
			else
				trigger.StartDragPosition = Vector2D.Unused;
			return false;
		}

		private bool IsTouchHoldTriggered(TouchHoldTrigger trigger)
		{
			if (GetState(0) == State.Pressing)
				trigger.StartPosition = GetPosition(0);
			trigger.Position = GetPosition(0);
			if (CheckHoverState(trigger))
				return trigger.IsHovering();
			trigger.Elapsed = 0.0f;
			return false;
		}

		private bool CheckHoverState(TouchHoldTrigger trigger)
		{
			return trigger.HoldArea.Contains(trigger.StartPosition) &&
				GetState(0) == State.Pressed &&
				trigger.StartPosition.DistanceTo(GetPosition(0)) < PositionEpsilon;
		}
	}
}