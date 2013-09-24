using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Fires once when the mouse has not moved for a prescribed period. Ideally used in tandem with
	/// MouseMovementTrigger to cancel the logic raised on a hover.
	/// </summary>
	public class MouseHoverTrigger : Trigger
	{
		public MouseHoverTrigger(float hoverTime = DefaultHoverTime)
		{
			HoverTime = hoverTime;
			Start<Mouse>();
		}

		public float HoverTime { get; private set; }
		private const float DefaultHoverTime = 1.5f;

		public MouseHoverTrigger(string hoverTime)
		{
			var parameters = hoverTime.SplitAndTrim(new[] { ' ' });
			HoverTime = parameters.Length == 1 ? parameters[0].Convert<float>() : DefaultHoverTime;
			Start<Mouse>();
		}

		public bool IsHovering()
		{
			if (Elapsed >= HoverTime)
				return false;
			Elapsed += Time.Delta;
			return Elapsed >= HoverTime;
		}

		public float Elapsed { get; set; }
		public Vector2D LastPosition { get; set; }
	}
}