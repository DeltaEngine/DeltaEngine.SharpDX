using System;
using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	public class TouchRotateTrigger : DragTrigger
	{
		public TouchRotateTrigger() {}

		public TouchRotateTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new TouchRotateTriggerHasNoParameters();
		}

		public class TouchRotateTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}

		public float Angle { get; set; }
	}
}