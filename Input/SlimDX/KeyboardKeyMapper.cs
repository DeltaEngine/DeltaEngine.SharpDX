using System;
using System.Collections.Generic;
using DInput = SlimDX.DirectInput;

namespace DeltaEngine.Input.SlimDX
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
			AddSpecialKeys();
			AddMathKeys();
			KeyMap.Add(Key.WindowsKey, DInput.Key.LeftWindowsKey);
			KeyMap.Add(Key.LeftWindows, DInput.Key.LeftWindowsKey);
			KeyMap.Add(Key.RightWindows, DInput.Key.RightWindowsKey);
		}

		private static void AddSpecialKeys()
		{
			KeyMap.Add(Key.None, DInput.Key.Unknown);
			KeyMap.Add(Key.Enter, DInput.Key.Return);
			KeyMap.Add(Key.NumLock, DInput.Key.NumberLock);
			KeyMap.Add(Key.Scroll, DInput.Key.ScrollLock);
			KeyMap.Add(Key.OpenBrackets, DInput.Key.LeftBracket);
			KeyMap.Add(Key.CloseBrackets, DInput.Key.RightBracket);
		}

		private static void AddMathKeys()
		{
			KeyMap.Add(Key.Multiply, DInput.Key.NumberPadStar);
			KeyMap.Add(Key.Add, DInput.Key.NumberPadPlus);
			KeyMap.Add(Key.Subtract, DInput.Key.NumberPadMinus);
			KeyMap.Add(Key.Decimal, DInput.Key.NumberPadComma);
			KeyMap.Add(Key.Divide, DInput.Key.NumberPadSlash);
		}

		private static readonly Dictionary<Key, DInput.Key> KeyMap =
			new Dictionary<Key, DInput.Key>();

		public static DInput.Key Translate(Key key)
		{
			if (KeyMap.ContainsKey(key))
				return KeyMap[key];
			string keyName = ModifyKeyNameIfNecessary(key.ToString());
			DInput.Key dinputKey;
			return Enum.TryParse(keyName, out dinputKey) ? dinputKey : DInput.Key.Unknown;
		}

		private static string ModifyKeyNameIfNecessary(string keyName)
		{
			if (keyName.StartsWith("Cursor"))
				keyName = keyName.Replace("Cursor", "") + "Arrow";
			else
				keyName = keyName.Replace("NumPad", "NumberPad");
			return keyName;
		}
	}
}