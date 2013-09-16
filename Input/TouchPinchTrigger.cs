using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch pinch to be detected.
	/// </summary>
	public class TouchPinchTrigger : Trigger
	{
		public TouchPinchTrigger(string parameter = "")
		{
			Start<Touch>();
		}
	}
}