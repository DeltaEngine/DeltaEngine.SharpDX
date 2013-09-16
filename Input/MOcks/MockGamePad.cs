using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input.Mocks
{
	/// <summary>
	/// Mock game pad for unit testing.
	/// </summary>
	public sealed class MockGamePad : GamePad
	{
		public MockGamePad()
			: base(GamePadNumber.Any)
		{
			IsAvailable = true;
			gamePadButtonStates = new State[GamePadButton.A.GetCount()];
		}

		public override bool IsAvailable { get; protected set; }
		private static State[] gamePadButtonStates;

		public override void Dispose() {}

		public override Point GetLeftThumbStick()
		{
			return Point.Zero;
		}

		public override Point GetRightThumbStick()
		{
			return Point.Zero;
		}

		public override float GetLeftTrigger()
		{
			return 0.0f;
		}

		public override float GetRightTrigger()
		{
			return 0.0f;
		}

		public override State GetButtonState(GamePadButton button)
		{
			return gamePadButtonStates[(int)button];
		}

		public void SetGamePadState(GamePadButton button, State state)
		{
			gamePadButtonStates[(int)button] = state;
		}

		public override void Vibrate(float strength) {}
	}
}