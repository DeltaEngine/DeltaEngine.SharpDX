using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows mouse button presses to be tracked.
	/// </summary>
	public class MouseDoubleClickTrigger : PositionTrigger
	{
		public MouseDoubleClickTrigger(State state)
			: this(MouseButton.Left, state) { }

		public MouseDoubleClickTrigger(MouseButton button = MouseButton.Left, State state = State.Pressing)
		{
			Button = button;
			State = state;
		}

		public MouseButton Button { get; internal set; }
		public State State { get; internal set; }

		public MouseDoubleClickTrigger(string buttonAndState)
		{
			var parameters = buttonAndState.SplitAndTrim(new[] { ' ' });
			if (parameters.Length == 0)
				throw new CannotCreateMouseButtonTriggerWithoutButton();
			Button = parameters[0].Convert<MouseButton>();
			State = parameters.Length > 1 ? parameters[1].Convert<State>() : State.Pressing;
		}

		public class CannotCreateMouseButtonTriggerWithoutButton : Exception { }

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}
	}
}
