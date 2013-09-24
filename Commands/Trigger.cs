using DeltaEngine.Entities;

namespace DeltaEngine.Commands
{
	/// <summary>
	/// Whenever a trigger condition is matched it is invoked and will fire the action of the Command
	/// attached, for example a KeyTrigger will fire when the key is the specified key state.
	/// </summary>
	public abstract class Trigger : Entity, Updateable, Pauseable
	{
		protected Trigger()
		{
			UpdatePriority = Priority.Last;
		}

		public bool WasInvoked { get; set; }

		public void Update()
		{
			WasInvoked = false;
		}

		public bool IsPauseable { get { return false; } }
	}
}