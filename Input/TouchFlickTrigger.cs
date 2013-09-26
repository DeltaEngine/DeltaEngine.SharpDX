using System;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch flick to be detected.
	/// </summary>
	public class TouchFlickTrigger : InputTrigger
	{
		public TouchFlickTrigger() {}

		public TouchFlickTrigger(string empty)
		{
			if (!String.IsNullOrEmpty(empty))
				throw new TouchFlickTriggerHasNoParameters();
		}

		public class TouchFlickTriggerHasNoParameters : Exception {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}

		public float PressTime { get; set; }
		public Vector2D StartPosition { get; set; }
	}
}