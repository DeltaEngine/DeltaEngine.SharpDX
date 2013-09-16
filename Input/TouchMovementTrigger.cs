using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Tracks any touch movement, useful to update cursor positions.
	/// </summary>
	public class TouchMovementTrigger : DragTrigger
	{
		public TouchMovementTrigger() {}

		public TouchMovementTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new TouchMovementTriggerHasNoParameters();
		}

		public class TouchMovementTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}
	}
}