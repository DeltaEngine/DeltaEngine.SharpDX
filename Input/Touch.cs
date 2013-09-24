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
			if (!IsAvailable)
				return; //ncrunch: no coverage
			foreach (Entity entity in entities)
			{
				TryInvokeTriggerOfType<TouchPressTrigger>(entity, IsTouchPressTriggered);
				TryInvokeTriggerOfType<TouchMovementTrigger>(entity, IsTouchMovementTriggered);
				TryInvokeTriggerOfType<TouchPositionTrigger>(entity, IsTouchPositionTriggered);
				TryInvokeTriggerOfType<TouchTapTrigger>(entity, IsTouchTapTriggered);
				TryInvokeTriggerOfType<TouchDragTrigger>(entity, IsTouchDragTriggered);
				TryInvokeTriggerOfType<TouchDragDropTrigger>(entity, IsTouchDragDropTriggered);
				TryInvokeTriggerOfType<TouchHoldTrigger>(entity, IsTouchHoldTriggered);
			}
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

		private bool IsTouchDragTriggered(TouchDragTrigger trigger)
		{
			if (GetState(0) == State.Pressing)
				trigger.StartPosition = GetPosition(0);
			else if (trigger.StartPosition != Vector2D.Unused &&
				GetState(0) != State.Released)
			{
				if (trigger.StartPosition.DistanceTo(GetPosition(0)) > PositionEpsilon)
				{
					trigger.Position = GetPosition(0);
					trigger.DoneDragging = GetState(0) == State.Releasing;
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