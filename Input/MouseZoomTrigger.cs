using System;
using DeltaEngine.Commands;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Input
{
	public class MouseZoomTrigger : ZoomTrigger, MouseTrigger
	{
		public MouseZoomTrigger() { }

		public MouseZoomTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new MouseZoomTriggerHasNoParameters();
		}

		public class MouseZoomTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}

		public int LastScrollWheelValue { get; set; }

		public void HandleWithMouse(Mouse mouse)
		{
			int currentScrollValueDifference = mouse.ScrollWheelValue - LastScrollWheelValue;
			LastScrollWheelValue = mouse.ScrollWheelValue;
			if(!ScreenSpace.Current.Viewport.Contains(mouse.Position))
				return;
			if (currentScrollValueDifference > 0)
				ZoomAmount = 1;
			else if (currentScrollValueDifference < 0)
				ZoomAmount = -1;
			else
				ZoomAmount = 0;
			if (ZoomAmount != 0)
				Invoke();
		}
	}
}