using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch drag to be detected.
	/// </summary>
	public class TouchDragTrigger : DragTrigger
	{
		public TouchDragTrigger(DragDirection direction = DragDirection.Free)
		{
			Direction = direction;
		}

		public DragDirection Direction { get; private set; }

		public TouchDragTrigger(string direction)
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