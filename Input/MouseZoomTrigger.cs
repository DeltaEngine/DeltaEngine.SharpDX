using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	public class MouseZoomTrigger : ZoomTrigger
	{
		public MouseZoomTrigger(string parameter = "") { }

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}

		public int LastScrollWheelValue { get; set; }
	}
}