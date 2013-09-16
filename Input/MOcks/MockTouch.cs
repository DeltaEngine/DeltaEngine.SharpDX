using DeltaEngine.Datatypes;

namespace DeltaEngine.Input.Mocks
{
	/// <summary>
	/// Mock touch device for unit testing.
	/// </summary>
	public sealed class MockTouch : Touch
	{
		public MockTouch()
		{
			IsAvailable = true;
			position = Point.Half;
			TouchStates = new State[MaxNumberOfTouchIndices];
		}

		public override bool IsAvailable { get; protected set; }
		private Point position;
		internal readonly State[] TouchStates;
		private const int MaxNumberOfTouchIndices = 10;

		public override void Dispose()
		{
			IsAvailable = false;
		}

		public override Point GetPosition(int touchIndex)
		{
			return position;
		}

		public void SetTouchState(int touchIndex, State state, Point newTouchPosition)
		{
			position = newTouchPosition;
			TouchStates[touchIndex] = state;
		}

		public override State GetState(int touchIndex)
		{
			return TouchStates[touchIndex];
		}
	}
}