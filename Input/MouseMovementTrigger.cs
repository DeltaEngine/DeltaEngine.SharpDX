using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Tracks any mouse movement, useful to update cursor positions or check hover states.
	/// </summary>
	public class MouseMovementTrigger : DragTrigger
	{
		public MouseMovementTrigger() {}

		public MouseMovementTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new MouseMovementTriggerHasNoParameters();
		}

		public class MouseMovementTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}
	}
}