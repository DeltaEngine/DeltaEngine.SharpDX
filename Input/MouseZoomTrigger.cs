using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	public class MouseZoomTrigger : ZoomTrigger
	{
		public MouseZoomTrigger(string unusedButRequiredByDynamicCreation = "") { }

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}

		public int LastScrollWheelValue { get; set; }
	}
}