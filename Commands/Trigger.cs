using System;
using DeltaEngine.Core;
using DeltaEngine.Entities;

namespace DeltaEngine.Commands
{
	/// <summary>
	/// Whenever a trigger condition is matched it is invoked and will fire the action of the Command
	/// attached, for example a KeyTrigger will fire when the key is the specified key state.
	/// WasInvokedThisTick will be true till the end of the tick (UpdatePriority.Last resets it).
	/// </summary>
	public abstract class Trigger : Entity, Updateable
	{
		protected Trigger()
		{
			UpdatePriority = Priority.Last;
		}

		public void Invoke()
		{
			if (Invoked != null)
				Invoked();
			WasInvokedThisTick = true;
		}

		public Action Invoked;

		public bool WasInvokedThisTick { get; private set; }

		public void Update()
		{
			WasInvokedThisTick = false;
		}

		public bool IsPauseable { get { return false; } }

		public static object GenerateTriggerFromType(Type triggerType, string triggerName,
			string triggerValue)
		{
			object triggerGenerated = null;
			if (triggerType == null)
				throw new UnableToCreateTriggerTypeIsUnknown(triggerName); //ncrunch: no coverage
			try
			{
				triggerGenerated = Activator.CreateInstance(triggerType, triggerValue);
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
			return triggerGenerated;
		}

		private class UnableToCreateTriggerTypeIsUnknown : Exception
		{ //ncrunch: no coverage start
			public UnableToCreateTriggerTypeIsUnknown(string name)
				: base(name) { }
		} //ncrunch: no coverage end
	}
}