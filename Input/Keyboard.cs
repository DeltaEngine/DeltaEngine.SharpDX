using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DeltaEngine.Entities;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Provides a way to fetch the current input values from a Keyboard device (virtual or real).
	/// </summary>
	public abstract class Keyboard : InputDevice
	{
		public State GetKeyState(Key key)
		{
			return keyboardStates[(int)key];
		}

		protected readonly State[] keyboardStates = new State[(int)Key.NumberOfKeys];

		public override void Update(IEnumerable<Entity> entities)
		{
			if (!IsAvailable)
				return; //ncrunch: no coverage
			// ReSharper disable PossibleMultipleEnumeration
			ProcessKeyTriggers(entities);
			ProcessInputHandling(entities);
			// ReSharper restore PossibleMultipleEnumeration
			UpdateKeyStates();
		}

		private void ProcessKeyTriggers(IEnumerable<Entity> entities)
		{
			foreach (var trigger in entities.OfType<KeyTrigger>())
				trigger.WasInvoked = GetKeyState(trigger.Key) == trigger.State;
		}

		private void ProcessInputHandling(IEnumerable<Entity> entities)
		{
			var entityWithFocus =
				entities.OfType<KeyboardControllable>().FirstOrDefault(
					entity => entity.IsEnabled && entity.HasFocus);
			if (entityWithFocus != null)
				entityWithFocus.Text = HandleInput(entityWithFocus.Text);
			else
				HandleInput("");
		}

		public string HandleInput(string inputText)
		{
			Key[] pressedKeys = newlyPressedKeys.ToArray();
			newlyPressedKeys.Clear();
			for (int i = 0; i < pressedKeys.Length; i++)
				inputText = HandleInputForKey(inputText, pressedKeys[i]);
			return inputText;
		}

		protected readonly List<Key> newlyPressedKeys = new List<Key>();

		protected abstract void UpdateKeyStates();

		private static string HandleInputForKey(string inputText, Key key)
		{
			if (key >= Key.D0 && key <= Key.D9)
				return inputText + key.ToString()[1].ToString(CultureInfo.InvariantCulture);
			if (key >= Key.NumPad0 && key <= Key.NumPad9)
				return inputText + key.ToString()[6].ToString(CultureInfo.InvariantCulture);
			if (key >= Key.A && key <= Key.Z)
				return inputText + key;
			if (key == Key.Space)
				return inputText + " ";
			if (key == Key.Decimal || key == Key.Period)
				return inputText + ".";
			if (key == Key.Comma)
				return inputText + ",";
			if (key == Key.Backspace && inputText.Length > 0)
				return inputText.Substring(0, inputText.Length - 1);
			return inputText;
		}

		public override bool IsAvailable { get; protected set; }
	}
}