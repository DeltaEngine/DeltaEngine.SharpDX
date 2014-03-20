using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using SlimDX.XInput;
using SlimDXState = SlimDX.XInput.State;

namespace DeltaEngine.Input.SlimDX
{
	/// <summary>
	/// Native implementation of the GamePad interface using SlimDX
	/// </summary>
	public class SlimDXGamePad : GamePad
	{
		public SlimDXGamePad(GamePadNumber number = GamePadNumber.Any)
			: base(number)
		{
			controller = new Controller(GetUserIndexFromNumber());
			states = new State[GamePadButton.A.GetCount()];
			for (int i = 0; i < states.Length; i++)
				states[i] = State.Released;
		}

		private Controller controller;
		private readonly State[] states;

		private UserIndex GetUserIndexFromNumber()
		{
			if (Number == GamePadNumber.Any)
				return GetAnyController();
			if (Number == GamePadNumber.Two)
				return UserIndex.Two;
			if (Number == GamePadNumber.Three)
				return UserIndex.Three;
			return Number == GamePadNumber.Four ? UserIndex.Four : UserIndex.One;
		}

		private static UserIndex GetAnyController()
		{
			if (new Controller(UserIndex.Two).IsConnected)
				return UserIndex.Two;
			if (new Controller(UserIndex.Three).IsConnected)
				return UserIndex.Three;
			return new Controller(UserIndex.Three).IsConnected ? UserIndex.Four : UserIndex.One;
		}

		public override bool IsAvailable
		{
			get { return controller.IsConnected; }
			protected set { }
		}

		protected override void UpdateGamePadStates()
		{
			var state = controller.GetState();
			UpdateValuesFromState(ref state);
		}

		private void UpdateValuesFromState(ref SlimDXState state)
		{
			UpdateAllButtons(state.Gamepad.Buttons);
			if (lastPacket == state.PacketNumber)
				return;
			lastPacket = state.PacketNumber;
			leftThumbStick = Normalize(state.Gamepad.LeftThumbX, state.Gamepad.LeftThumbY,
				Gamepad.GamepadLeftThumbDeadZone);
			rightThumbStick = Normalize(state.Gamepad.RightThumbX, state.Gamepad.RightThumbY,
				Gamepad.GamepadRightThumbDeadZone);
			leftTrigger = state.Gamepad.LeftTrigger / (float)byte.MaxValue;
			rightTrigger = state.Gamepad.RightTrigger / (float)byte.MaxValue;
		}

		private uint lastPacket;
		private Vector2D leftThumbStick;
		private Vector2D rightThumbStick;
		private float leftTrigger;
		private float rightTrigger;

		private static Vector2D Normalize(short rawX, short rawY, short threshold)
		{
			var value = new Vector2D(rawX, rawY);
			var magnitude = value.DistanceTo(Vector2D.Zero);
			var direction = value / (magnitude == 0 ? 1 : magnitude);
			var normalizedMagnitude = 0.0f;
			if (magnitude - threshold > 0)
				normalizedMagnitude = Math.Min((magnitude - threshold) / (short.MaxValue - threshold), 1);

			return direction * normalizedMagnitude;
		}

		private void UpdateAllButtons(GamepadButtonFlags buttons)
		{
			UpdateNormalButtons(buttons);
			UpdateStickAndShoulderButtons(buttons);
			UpdateDPadButtons(buttons);
		}

		private void UpdateNormalButtons(GamepadButtonFlags buttons)
		{
			UpdateButton(buttons, GamepadButtonFlags.A, GamePadButton.A);
			UpdateButton(buttons, GamepadButtonFlags.B, GamePadButton.B);
			UpdateButton(buttons, GamepadButtonFlags.X, GamePadButton.X);
			UpdateButton(buttons, GamepadButtonFlags.Y, GamePadButton.Y);
			UpdateButton(buttons, GamepadButtonFlags.Back, GamePadButton.Back);
			UpdateButton(buttons, GamepadButtonFlags.Start, GamePadButton.Start);
		}

		private void UpdateButton(GamepadButtonFlags buttons, GamepadButtonFlags nativeButton,
			GamePadButton button)
		{
			var buttonIndex = (int)button;
			states[buttonIndex] =
				states[buttonIndex].UpdateOnNativePressing((buttons & nativeButton) != 0);
		}

		private void UpdateStickAndShoulderButtons(GamepadButtonFlags buttons)
		{
			UpdateButton(buttons, GamepadButtonFlags.LeftShoulder, GamePadButton.LeftShoulder);
			UpdateButton(buttons, GamepadButtonFlags.LeftThumb, GamePadButton.LeftStick);
			UpdateButton(buttons, GamepadButtonFlags.RightShoulder, GamePadButton.RightShoulder);
			UpdateButton(buttons, GamepadButtonFlags.RightThumb, GamePadButton.RightStick);
		}

		private void UpdateDPadButtons(GamepadButtonFlags buttons)
		{
			UpdateButton(buttons, GamepadButtonFlags.DPadDown, GamePadButton.Down);
			UpdateButton(buttons, GamepadButtonFlags.DPadUp, GamePadButton.Up);
			UpdateButton(buttons, GamepadButtonFlags.DPadLeft, GamePadButton.Left);
			UpdateButton(buttons, GamepadButtonFlags.DPadRight, GamePadButton.Right);
		}

		public override void Dispose()
		{
			controller = null;
		}

		public override Vector2D GetLeftThumbStick()
		{
			return leftThumbStick;
		}

		public override Vector2D GetRightThumbStick()
		{
			return rightThumbStick;
		}

		public override float GetLeftTrigger()
		{
			return leftTrigger;
		}

		public override float GetRightTrigger()
		{
			return rightTrigger;
		}

		public override State GetButtonState(GamePadButton button)
		{
			return states[(int)button];
		}

		public override void Vibrate(float strength)
		{
			var motorSpeed = (ushort)(strength * ushort.MaxValue);
			controller.SetVibration(new Vibration
			{
				LeftMotorSpeed = motorSpeed,
				RightMotorSpeed = motorSpeed
			});
		}
	}
}