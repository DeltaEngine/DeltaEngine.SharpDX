using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch pinch to be detected.
	/// </summary>
	public class TouchPinchTrigger : InputTrigger
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
	}
}