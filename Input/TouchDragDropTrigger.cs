using DeltaEngine.Commands;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Drag and Drop events with Touch.
	/// </summary>
	public class TouchDragDropTrigger : Trigger
	{
		public TouchDragDropTrigger(Rectangle startArea)
		{
			StartArea = startArea;
			StartDragPosition = Point.Unused;
			Start<Touch>();
		}

		public Rectangle StartArea { get; private set; }
		public Point StartDragPosition { get; set; }
	}
}