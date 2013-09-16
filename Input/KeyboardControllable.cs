namespace DeltaEngine.Input
{
	/// <summary>
	/// Used for UI controls that can respond to keyboard input
	/// </summary>
	public interface KeyboardControllable
	{
		bool IsEnabled { get; }
		bool HasFocus { get; }
		string Text { get; set; }
	}
}