namespace DeltaEngine.Commands
{
	/// <summary>
	/// Allows the application to get informed if any input device triggers any zoom geasture.
	/// </summary>
	public abstract class ZoomTrigger : InputTrigger
	{
		public int ZoomAmount { get; set; }
	}
}