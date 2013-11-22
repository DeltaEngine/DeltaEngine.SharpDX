using System;
using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Trigger implementation for Thumb Sticks and Triggers on controllers.
	/// </summary>
	public class GamePadAnalogTrigger : PositionTrigger
	{
		public GamePadAnalogTrigger(GamePadAnalog gamePadAnalog)
		{
			Analog = gamePadAnalog;
		}

		public GamePadAnalog Analog { get; private set; }

		public GamePadAnalogTrigger(string gamePadAnalog)
		{
			if (String.IsNullOrEmpty(gamePadAnalog))
				throw new CannotCreateGamePadStickTriggerWithoutGamePadStick();
			Analog = gamePadAnalog.Convert<GamePadAnalog>();
		}

		public class CannotCreateGamePadStickTriggerWithoutGamePadStick : Exception {}

		protected override void StartInputDevice()
		{
			Start<GamePad>();
		}
	}
}