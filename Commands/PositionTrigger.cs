using DeltaEngine.Datatypes;

namespace DeltaEngine.Commands
{
	/// <summary>
	/// Allows a position based trigger to be invoked along with any associated Command or Entity.
	/// </summary>
	public abstract class PositionTrigger : InputTrigger
	{
		public Point Position { get; set; }
	}
}