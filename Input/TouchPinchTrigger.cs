using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch pinch to be detected.
	/// </summary>
	public class TouchPinchTrigger : InputTrigger, TouchTrigger
	{
		public TouchPinchTrigger() {}

		public TouchPinchTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new TouchPinchTriggerHasNoParameters();
		}

		public class TouchPinchTriggerHasNoParameters : Exception {}

		public float Distance { get; set; }

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}

		public void HandleWithTouch(Touch touch)
		{
			if (touch.GetState(0) >= State.Pressing && touch.GetState(1) >= State.Pressing)
			{
				Distance = Math.Abs((touch.GetPosition(1) - touch.GetPosition(0)).Length);
				Invoke();
			}
			else
				Distance = 0f;
		}
	}
}