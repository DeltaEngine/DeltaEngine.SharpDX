using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch tap to be detected.
	/// </summary>
	public class TouchTapTrigger : Trigger
	{
		public TouchTapTrigger()
		{
			Start<Touch>();
		}

		public State LastState { get; set; }
	}
}