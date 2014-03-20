using System;
using System.Runtime.InteropServices;
using DeltaEngine.Core;
using DInput = SlimDX.DirectInput;

namespace DeltaEngine.Input.SlimDX
{
	/// <summary>
	/// Native implementation of the Keyboard interface using DirectInput
	/// </summary>
	public sealed class SlimDXKeyboard : Keyboard
	{
		public SlimDXKeyboard(Window window)
		{
			IsAvailable = true;
			windowHandle = window.Handle;
			CreateNativeKeyboard();
		}

		private readonly IntPtr windowHandle;

		private void CreateNativeKeyboard()
		{
			nativeState = new DInput.KeyboardState();
			directInput = new DInput.DirectInput();
			nativeKeyboard = new DInput.Keyboard(directInput);
			nativeKeyboard.SetCooperativeLevel(windowHandle,
				DInput.CooperativeLevel.Nonexclusive | DInput.CooperativeLevel.Background);
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