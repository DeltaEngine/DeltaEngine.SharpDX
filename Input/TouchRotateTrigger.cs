using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	internal class TouchRotateTrigger : DragTrigger
	{
		public TouchRotateTrigger() {}

		public TouchRotateTrigger(string empty)
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