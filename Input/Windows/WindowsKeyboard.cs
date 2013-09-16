using System;
using System.Collections.Generic;

namespace DeltaEngine.Input.Windows
{
	/// <summary>
	/// Native keyboard implementation using a windows hook.
	/// </summary>
	public sealed class WindowsKeyboard : Keyboard
	{
		public WindowsKeyboard()
		{
			hook = new WindowsHook(WindowsHook.KeyboardHookId, HandleProcMessage);
			pressedKeys = new List<Key>();
			releasedKeys = new List<Key>();
			IsAvailable = true;
		}

		private readonly WindowsHook hook;
		private readonly List<Key> pressedKeys;
		private readonly List<Key> releasedKeys;

		protected override void UpdateKeyStates()
		{
			for (int i = 0; i < (int)Key.NumberOfKeys; i++)
			{
				keyboardStates[i] = ProcessReleasedAndPressedListsForKey((Key)i);
				if (keyboardStates[i] == State.Pressing)
					newlyPressedKeys.Add((Key)i);
			}
			pressedKeys.Clear();
			releasedKeys.Clear();
		}

		private State ProcessReleasedAndPressedListsForKey(Key key)
		{
			bool isReleased = releasedKeys.Contains(key);
			bool isPressed = pressedKeys.Contains(key);
			return UpdateInputState(keyboardStates[(int)key], isReleased, isPressed);
		}

		private static State UpdateInputState(State previousState, bool isReleased, bool isPressed)
		{
			if (previousState == State.Pressing && isReleased == false)
				return State.Pressed;
			if (isPressed && isReleased == false)
				return previousState != State.Pressed ? State.Pressing : previousState;
			if (isReleased)
				return State.Releasing;
			return previousState == State.Releasing ? State.Released : previousState;
		}

		private void HandleProcMessage(IntPtr wParam, IntPtr lParam, int msg)
		{
			var keyCode = (Key)wParam.ToInt32();
			if (IsKeyPressed(lParam.ToInt32()))
				pressedKeys.Add(keyCode);
			else
				releasedKeys.Add(keyCode);
		}

		private static bool IsKeyPressed(int lParam)
		{
			return ((uint)(lParam & 0x80000000) >> 0xFF) != 1;
		}

		public override void Dispose()
		{
			hook.Dispose();
			IsAvailable = false;
		}
	}
}