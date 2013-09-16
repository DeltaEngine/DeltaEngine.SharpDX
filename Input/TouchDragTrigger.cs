using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch drag to be detected.
	/// </summary>
	public class TouchDragTrigger : DragTrigger
	{
		public TouchDragTrigger(string unused = null) {}

		protected override void StartInputDevice()
		{
			Start<Touch>();
		}
	}
}