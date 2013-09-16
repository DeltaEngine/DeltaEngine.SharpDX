using System;
using System.Collections.Generic;
using DInput = SharpDX.DirectInput;

namespace DeltaEngine.Input.SharpDX
{
	/// <summary>
	/// Helper class to map all the DirectInput keys to our Key enumeration.
	/// </summary>
	internal static class KeyboardKeyMapper
	{
		/// <summary>
		/// No matching DInput key for Separator, Plus, Question,
		/// Tilde, ChatPadGreen, ChatPadOrange, Pipe, Quotes
		/// </summary>
		static KeyboardKeyMapper()
		{
			KeyMap.Add(Key.WindowsKey, DInput.Key.LeftWindowsKey);
			KeyMap.Add(Key.LeftWindows, DInput.Key.LeftWindowsKey);
			KeyMap.Add(Key.RightWindows, DInput.Key.RightWindowsKey);
			KeyMap.Add(Key.None, DInput.Key.Unknown);
			KeyMap.Add(Key.Enter, DInput.Key.Return);
			KeyMap.Add(Key.CapsLock, DInput.Key.Capital);
			KeyMap.Add(Key.PrintScreen, DInput.Key.PrintScreen);
			KeyMap.Add(Key.NumLock, DInput.Key.NumberLock);
			KeyMap.Add(Key.Scroll, DInput.Key.ScrollLock);
			KeyMap.Add(Key.OpenBrackets, DInput.Key.LeftBracket);
			KeyMap.Add(Key.CloseBrackets, DInput.Key.RightBracket);
		}

		private static readonly Dictionary<Key, DInput.Key> KeyMap =
			new Dictionary<Key, DInput.Key>();

		public static DInput.Key Translate(Key key)
		{
			if (KeyMap.ContainsKey(key))
				return KeyMap[key];

			DInput.Key dinputKey;
			string keyName = key.ToString().Replace("Cursor", "").Replace("NumPad", "NumberPad");
			return Enum.TryParse(keyName, out dinputKey) ? dinputKey : DInput.Key.Unknown;
		}
	}
}