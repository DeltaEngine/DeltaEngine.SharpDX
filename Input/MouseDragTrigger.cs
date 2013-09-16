using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Drag events with Mouse.
	/// </summary>
	public class MouseDragTrigger : DragTrigger
	{
		public MouseDragTrigger(MouseButton button = MouseButton.Left)
		{
			Button = button;
		}

		public MouseButton Button { get; private set; }

		public MouseDragTrigger(string button)
		{
			var parameters = button.SplitAndTrim(new[] { ' ' });
			Button = parameters.Length > 0 ? parameters[0].Convert<MouseButton>() : MouseButton.Left;
		}

		protected override void StartInputDevice()
		{
			Start<Mouse>();
		}
	}
}