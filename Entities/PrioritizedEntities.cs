using System.Collections.Generic;
using DeltaEngine.Extensions;

namespace DeltaEngine.Entities
{
	/// <summary>
	/// Used to manage entities in connection with their behaviors, which can change during Update.
	/// </summary>
	internal class PrioritizedEntities
	{
		public readonly ChangeableList<Entity> entities = new ChangeableList<Entity>();
		public readonly Dictionary<UpdateBehavior, ChangeableList<Entity>> behaviors =
			new Dictionary<UpdateBehavior, ChangeableList<Entity>>();
		public readonly Dictionary<UpdateBehavior, ChangeableList<Entity>>
			delayedNewBehaviorsWhileUpdating = new Dictionary<UpdateBehavior, ChangeableList<Entity>>();

		public void AddEntityToBehavior(UpdateBehavior behavior, Entity entity)
		{
			AddToBehaviors(EntitiesRunner.Current.State == UpdateDrawState.Update
				? delayedNewBehaviorsWhileUpdating : behaviors, behavior, entity);
		}

		private static void AddToBehaviors(
			Dictionary<UpdateBehavior, ChangeableList<Entity>> dictionary, UpdateBehavior behavior,
			Entity entity)
		{
			if (dictionary.ContainsKey(behavior))
			{
				if (entity != null)
					dictionary[behavior].Add(entity);
			}
			else
			{
				var list = new ChangeableList<Entity>();
				if (entity != null)
					list.Add(entity);
				dictionary.Add(behavior, list);
			}
		}

		public void RemoveEntityFromBehaviors(Entity entity, UpdateBehavior updateBehavior)
		{
			if (delayedNewBehaviorsWhileUpdating.ContainsKey(updateBehavior))
				delayedNewBehaviorsWhileUpdating[updateBehavior].Remove(entity);
			if (behaviors.ContainsKey(updateBehavior))
				behaviors[updateBehavior].Remove(entity);
		}

		public void Clear()
		{
			entities.Clear();
			behaviors.Clear();
		}
	}
}