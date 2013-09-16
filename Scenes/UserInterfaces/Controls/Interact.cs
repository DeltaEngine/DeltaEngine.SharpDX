using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Enables a control to respond to mouse and touch input.
	/// </summary>
	public class Interact : UpdateBehavior
	{
		public Interact()
			: base(Priority.High)
		{
			new Command(point => leftClick = point).Add(new MouseButtonTrigger()).Add(
				new TouchPressTrigger());
			new Command(point => leftRelease = point).Add(new MouseButtonTrigger(State.Releasing)).Add(
				new TouchPressTrigger(State.Releasing));
			new Command(point => movement = point).Add(new MouseMovementTrigger()).Add(
				new TouchMovementTrigger());
			new Command((start, end, done) =>
			{
				dragStart = start;
				dragEnd = end;
				dragDone = done;
			}).Add(new MouseDragTrigger()).Add(new TouchDragTrigger());
		}

		private Point leftClick = Point.Unused;
		private Point leftRelease = Point.Unused;
		private Point movement = Point.Unused;
		private Point dragStart = Point.Unused;
		private Point dragEnd = Point.Unused;
		private bool dragDone;

		public override void Update(IEnumerable<Entity> entities)
		{
			// ReSharper disable PossibleMultipleEnumeration
			if (dragStart == Point.Unused)
				ResetDrag(entities);
			if (DidATriggerFireThisFrame())
				UpdateStateOfControls(entities);
			ProcessShiftOfFocus(entities);
			Reset();
		}

		private static void ResetDrag(IEnumerable<Entity> entities)
		{
			foreach (Control control in entities.OfType<Control>())
				ResetDragForControl(control.State);
		}

		private static void ResetDragForControl(InteractiveState state)
		{
			state.DragStart = Point.Unused;
			state.DragEnd = Point.Unused;
			state.DragDone = false;
		}

		private bool DidATriggerFireThisFrame()
		{
			return leftClick != Point.Unused || leftRelease != Point.Unused || movement != Point.Unused ||
				dragStart != Point.Unused;
		}

		private void UpdateStateOfControls(IEnumerable<Entity> entities)
		{
			foreach (Control control in entities.OfType<Control>().Where(
					control => control.IsEnabled && control.Visibility == Visibility.Show))
				UpdateState(control, control.State);
		}

		private void UpdateState(Control control, InteractiveState state)
		{
			ProcessAnyLeftClick(control, state);
			ProcessAnyLeftRelease(control, state);
			ProcessAnyMovement(control, state);
			ProcessAnyDrag(state);
		}

		private void ProcessAnyLeftClick(Control control, InteractiveState state)
		{
			if (leftClick != Point.Unused)
				state.IsPressed = control.RotatedDrawAreaContains(leftClick);
		}

		private void ProcessAnyLeftRelease(Control control, InteractiveState state)
		{
			if (leftRelease == Point.Unused)
				return;
			if (state.IsPressed && control.RotatedDrawAreaContains(leftRelease))
				ClickControl(control, state);
			else
				state.IsPressed = false;
		}

		private static void ClickControl(Control control, InteractiveState state)
		{
			state.IsPressed = false;
			control.Click();
			if (state.CanHaveFocus)
				state.WantsFocus = true;
		}

		private void ProcessAnyMovement(Control control, InteractiveState state)
		{
			if (movement == Point.Unused)
				return;
			state.IsInside = control.RotatedDrawAreaContains(movement);
			Point rotatedMovement = control.Rotation == 0.0f
				? movement : movement.RotateAround(control.RotationCenter, -control.Rotation);
			state.RelativePointerPosition = control.DrawArea.GetRelativePoint(rotatedMovement);
		}

		private void ProcessAnyDrag(InteractiveState state)
		{
			if (dragStart == Point.Unused)
				return;
			state.DragStart = dragStart;
			state.DragEnd = dragEnd;
			state.DragDone = dragDone;
		}

		private static void ProcessShiftOfFocus(IEnumerable<Entity> entities)
		{
			// ReSharper disable PossibleMultipleEnumeration
			var entityWhichWantsFocus =
				entities.FirstOrDefault(entity => entity.Get<InteractiveState>().WantsFocus);
			if (entityWhichWantsFocus == null)
				return;
			foreach (var state in entities.Select(entity => entity.Get<InteractiveState>()))
			{
				state.WantsFocus = false;
				state.HasFocus = false;
			}
			entityWhichWantsFocus.Get<InteractiveState>().HasFocus = true;
			// ReSharper restore PossibleMultipleEnumeration
		}

		private void Reset()
		{
			leftClick = Point.Unused;
			leftRelease = Point.Unused;
			movement = Point.Unused;
			dragStart = Point.Unused;
			dragDone = false;
		}
	}
}