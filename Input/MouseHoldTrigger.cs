using System;
using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Fires once when the mouse button is pressed and the mouse has not moved for some time.
	/// </summary>
	public class MouseHoldTrigger : PositionTrigger
	{
		public MouseHoldTrigger(Rectangle holdArea, float holdTime = DefaultHoldTime,
			MouseButton button = MouseButton.Left)
		{
			HoldArea = holdArea;
			HoldTime = holdTime;
			Button = button;
		}

		public Rectangle HoldArea { get; private set; }
		public float HoldTime { get; private set; }
		public MouseButton Button { get; private set; }
		private const float DefaultHoldTime = 0.5f;

		public MouseHoldTrigger(string holdAreaAndHoldTimeAndButton)
		{
			var parameters = holdAreaAndHoldTimeAndButton.SplitAndTrim(new[] { ' ' });
			if (parameters.Length < 4)
				throw new CannotCreateMouseHoldTriggerWithoutHoldArea();
			HoldArea = BuildStringForParemeter(parameters).Convert<Rectangle>();
			HoldTime = parameters.Length > 4 ? parameters[4].Convert<float>() : DefaultHoldTime;
			Button = parameters.Length > 5 ? parameters[5].Convert<MouseButton>() : MouseButton.Left;
		}

		public class CannotCreateMouseHoldTriggerWithoutHoldArea : Exception {}

		private static string BuildStringForParemeter(IList<string> parameters)
		{
			return parameters[0] + " " + parameters[1] + " " + parameters[2] + " " + parameters[3];
		}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}

		public bool IsHovering()
		{
			if (Elapsed >= HoldTime || !HoldArea.Contains(Position))
				return false;
			Elapsed += Time.Delta;
			return Elapsed >= HoldTime;
		}

		public float Elapsed { get; set; }
		public Vector2D StartPosition { get; set; }
	}
}