using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch double tap to be detected.
	/// </summary>
	public class TouchDoubleTapTrigger : InputTrigger
	{
		public TouchDoubleTapTrigger() {}

		public TouchDoubleTapTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new TouchDoubleTapTriggerHasNoParameters();
		}

		public class TouchDoubleTapTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}
	}
}