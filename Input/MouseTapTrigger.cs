using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows mouse button tap to be tracked.
	/// </summary>
	public class MouseTapTrigger : Trigger, MouseTrigger
	{
		public MouseTapTrigger(MouseButton button)
		{
			Button = button;
			Start<Mouse>();
		}

		public MouseButton Button { get; private set; }

		public void HandleWithMouse(Mouse mouse)
		{
			bool wasJustStartedPressing = lastState == State.Pressing;
			State currentState = mouse.GetButtonState(Button);
			var isNowReleased = currentState == State.Releasing;
			lastState = currentState;
			if (isNowReleased && wasJustStartedPressing)
				Invoke();
		}

		private State lastState;
	}
}