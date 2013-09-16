using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch hold to be detected.
	/// </summary>
	public class TouchHoldTrigger : PositionTrigger
	{
		public TouchHoldTrigger(Rectangle holdArea, float holdTime = DefaultHoldTime)
		{
			HoldArea = holdArea;
			HoldTime = holdTime;
		}

		public Rectangle HoldArea { get; private set; }
		public float HoldTime { get; private set; }
		private const float DefaultHoldTime = 1.0f;

		public bool IsHovering()
		{
			if (Elapsed >= HoldTime || !HoldArea.Contains(Position))
				return false;
			Elapsed += Time.Delta;
			return Elapsed >= HoldTime;
		}

		public float Elapsed { get; set; }
		public Point StartPosition { get; set; }
		public Point LastPosition { get; set; }

		public TouchHoldTrigger(string parameter = "") {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}
	}
}