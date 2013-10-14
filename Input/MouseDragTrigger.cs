using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Drag events with Mouse.
	/// </summary>
	public class MouseDragTrigger : DragTrigger
	{
		public MouseDragTrigger(MouseButton button = MouseButton.Left,
			DragDirection direction = DragDirection.Free)
		{
			Button = button;
			Direction = direction;
		}

		public MouseButton Button { get; private set; }
		public DragDirection Direction { get; private set; }

		public MouseDragTrigger(string button)
		{
			var parameters = button.SplitAndTrim(new[] { ' ' });
			Button = parameters.Length > 0 ? parameters[0].Convert<MouseButton>() : MouseButton.Left;
			Direction = parameters.Length > 1
				? parameters[1].Convert<DragDirection>() : DragDirection.Free;
		}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}
	}
}