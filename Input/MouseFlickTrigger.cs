using System;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a mouse flick to be detected.
	/// </summary>
	public class MouseFlickTrigger : InputTrigger
	{
		public MouseFlickTrigger() {}

		public MouseFlickTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new MouseFlickTriggerHasNoParameters();
		}

		public class MouseFlickTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}

		public float PressTime { get; set; }
		public Vector2D StartPosition { get; set; }
	}
}