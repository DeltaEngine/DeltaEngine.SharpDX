using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch dual drag to be detected.
	/// </summary>
	public class TouchDualDragTrigger : DragTrigger
	{
		public TouchDualDragTrigger(DragDirection direction = DragDirection.Free)
		{
			Direction = direction;
		}

		public DragDirection Direction { get; private set; }
		public Vector2D SecondStartPosition { get; set; }
		public Vector2D SecondPosition { get; set; }

		public TouchDualDragTrigger(string direction)
		{
			var parameters = direction.SplitAndTrim(new[] { ' ' });
			Direction = parameters.Length > 0
				? parameters[0].Convert<DragDirection>() : DragDirection.Free;
		}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}
	}
}