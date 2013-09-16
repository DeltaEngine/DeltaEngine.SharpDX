using System;
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
			windowHandle = (IntPtr)window.Handle;
			CreateNativeKeyboard();
		}

		private readonly IntPtr windowHandle;

		private void CreateNativeKeyboard()
		{
			nativeState = new DInput.KeyboardState();
			directInput = new DInput.DirectInput();
			nativeKeyboard = new DInput.Keyboard(directInput);
			nativeKeyboard.SetCooperativeLevel(windowHandle,
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
	}
}