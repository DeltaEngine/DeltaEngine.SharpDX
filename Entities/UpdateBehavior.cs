using System.Collections.Generic;

namespace DeltaEngine.Entities
{
	/// <summary>
	/// Goes through all entities of a specfic type each update tick.
	/// </summary>
	public abstract class UpdateBehavior
	{
		protected UpdateBehavior(Priority priority = Priority.Normal)
		{
			this.priority = priority;
		}

		internal readonly Priority priority;

		public abstract void Update(IEnumerable<Entity> entities);
	}
}