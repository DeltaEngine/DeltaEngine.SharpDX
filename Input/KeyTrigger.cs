using System;
using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Trigger implementation for Keyboard events.
	/// </summary>
	public class KeyTrigger : Trigger
	{
		public KeyTrigger(Key key, State state = State.Pressing)
		{
			Key = key;
			State = state;
			Start<Keyboard>();
		}

		public Key Key { get; internal set; }
		public State State { get; internal set; }

		public KeyTrigger(string keyAndState)
		{
			var parameters = keyAndState.SplitAndTrim(new[] { ' ' });
			if (parameters.Length == 0)
				throw new CannotCreateKeyTriggerWithoutKey();
			Key = parameters[0].Convert<Key>();
			State = parameters.Length > 1 ? parameters[1].Convert<State>() : State.Pressing;
			Start<Keyboard>();
		}

		public class CannotCreateKeyTriggerWithoutKey : Exception {}
	}
}