namespace Blocks
{
	/// <summary>
	/// The various rendering layers. Higher layers overdraw lower ones 
	/// </summary>
	public enum RenderLayer
	{
		Background,
		Foreground,
		Grid,
		FallingBrick,
		ZoomingBrick
	}
}