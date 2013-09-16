using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows mouse button tap to be tracked.
	/// </summary>
	public class MouseTapTrigger : Trigger
	{
		public MouseTapTrigger(MouseButton button)
		{
			Button = button;
			Start<Mouse>();
		}

		public MouseButton Button { get; internal set; }
		public State LastState { get; set; }
	}
}