using System.Runtime.InteropServices;
using DeltaEngine.Core;
using DInput = SharpDX.DirectInput;

namespace DeltaEngine.Input.SharpDX
{
	/// <summary>
	/// Native implementation of the Keyboard interface using DirectInput.
	/// </summary>
	public sealed class SharpDXKeyboard : Keyboard
	{
		public SharpDXKeyboard(Window window)
		{
			IsAvailable = true;
			this.window = window;
			CreateNativeKeyboard();
		}

		private readonly Window window;

		private void CreateNativeKeyboard()
		{
			nativeState = new DInput.KeyboardState();
			directInput = new DInput.DirectInput();
			nativeKeyboard = new DInput.Keyboard(directInput);
			if (window.IsWindowsFormAndNotJustAPanel)
				nativeKeyboard.SetCooperativeLevel(window.Handle,
					DInput.CooperativeLevel.NonExclusive | DInput.CooperativeLevel.Background);
			nativeKeyboard.Acquire();
		}

		private DInput.KeyboardState nativeState;
		private DInput.DirectInput directInput;
		private DInput.Keyboard nativeKeyboard;

		public override void Dispose()
		{
			if (nativeKeyboard != null)
			{
				nativeKeyboard.Unacquire();
				nativeKeyboard = null;
			}

			directInput = null;
			IsAvailable = false;
		}

		protected override void UpdateKeyStates()
		{
			nativeKeyboard.GetCurrentState(ref nativeState);
			for (int i = 0; i < (int)Key.NumberOfKeys; i++)
				UpdateKeyState(i, nativeState.IsPressed(KeyboardKeyMapper.Translate((Key)i)));
		}

		private void UpdateKeyState(int key, bool nowPressed)
		{
			keyboardStates[key] = keyboardStates[key].UpdateOnNativePressing(nowPressed);
			if (keyboardStates[key] == State.Pressing)
				newlyPressedKeys.Add((Key)key);
		}

		protected override bool IsCapsLocked
		{
			get { return (((ushort)GetKeyState(0x14)) & 0xffff) != 0; }
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true,
			CallingConvention = CallingConvention.Winapi)]
		private static extern short GetKeyState(int keyCode);
	}
}