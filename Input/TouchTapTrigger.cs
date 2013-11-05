using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch tap to be detected.
	/// </summary>
	public class TouchTapTrigger : Trigger, TouchTrigger
	{
		public TouchTapTrigger()
		{
			Start<Touch>();
		}

		public void HandleWithTouch(Touch touch)
		{
			bool wasJustStartedPressing = lastState == State.Pressing;
			State currentState = touch.GetState(0);
			var isNowReleased = currentState == State.Releasing;
			lastState = currentState;
			if (isNowReleased && wasJustStartedPressing)
				Invoke();
		}

		private State lastState;
	}
}