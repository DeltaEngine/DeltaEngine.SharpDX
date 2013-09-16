using DeltaEngine.Datatypes;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Touch input message for remote input via networking.
	/// </summary>
	public class TouchMessage
	{
		protected TouchMessage() {}

		public TouchMessage(Point[] positions, bool[] pressedTouches)
		{
			Positions = positions;
			PressedTouches = pressedTouches;
		}

		public Point[] Positions { get; private set; }
		public bool[] PressedTouches { get; private set; }

		public const int MaxNumberOfTouches = 10;
	}
}