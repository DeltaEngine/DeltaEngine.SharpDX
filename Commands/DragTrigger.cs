using DeltaEngine.Datatypes;

namespace DeltaEngine.Commands
{
	/// <summary>
	/// Allows a start and end position based drag trigger to be invoked. 
	/// </summary>
	public abstract class DragTrigger : PositionTrigger
	{
		public Vector2D StartPosition { get; set; }
		public bool DoneDragging { get; set; }
	}
}