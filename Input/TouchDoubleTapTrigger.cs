using DeltaEngine.Commands;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows a touch double tap to be detected.
	/// </summary>
	public class TouchDoubleTapTrigger : Trigger
	{
		public TouchDoubleTapTrigger(string unused = null)
		{
			Start<Touch>();
		}
	}
}