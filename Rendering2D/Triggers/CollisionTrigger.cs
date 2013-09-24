using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D.Triggers
{
	public class CollisionTrigger : UpdateBehavior
	{
		public CollisionTrigger() : base(Priority.High) { }

		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (var entity in entities.OfType<Entity2D>())
			{
				var data = entity.Get<Data>();
				var foundEntities = GetEntitiesFromSearchTags(data);
				bool isColliding = false;
				foreach (var otherEntity in foundEntities.OfType<Entity2D>())
					if (entity != otherEntity && !isColliding)
						isColliding = IsEntityRectCollidingWithOtherEntityRect(entity, otherEntity);
				entity.Color = isColliding ? data.TriggeredColor : data.DefaultColor;
			}
		}

		private static IEnumerable<Entity> GetEntitiesFromSearchTags(Data data)
		{
			var foundEntities = new List<Entity>();
			foreach (var tag in data.SearchTags)
				foreach (var foundEntity in EntitiesRunner.Current.GetEntitiesWithTag(tag))
					foundEntities.Add(foundEntity);
			return foundEntities;
		}

		private static bool IsEntityRectCollidingWithOtherEntityRect(Entity2D entity,
			Entity2D otherEntity)
		{
			return entity.DrawArea.IsColliding(entity.Rotation, otherEntity.DrawArea,
				otherEntity.Rotation);
		}

		public class Data
		{
			public Data(List<string> searchTags, Color triggeredColor, Color defaultColor)
			{
				SearchTags = searchTags;
				TriggeredColor = triggeredColor;
				DefaultColor = defaultColor;
			}

			public Data(Color triggeredColor, Color defaultColor) :
				this(new List<string>(), triggeredColor, defaultColor) { }

			public List<string> SearchTags { get; private set; }
			public Color TriggeredColor { get; private set; }
			public Color DefaultColor { get; private set; }
		}
	}
}