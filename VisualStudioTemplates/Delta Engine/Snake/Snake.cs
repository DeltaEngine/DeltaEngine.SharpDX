using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class Snake : Entity2D
	{
		public Snake(int gridSize, Color color) : base(Rectangle.Zero)
		{
			Add(new Body(gridSize, color));
			Start<SnakeHandler>();
		}

		public void Dispose()
		{
			var body = Get<Body>();
			foreach (var bodyPart in body.BodyParts)
				bodyPart.IsActive = false;

			Get<Body>().BodyParts.Clear();
			Remove<Body>();
			Stop<SnakeHandler>();
		}
		internal class SnakeHandler : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					if (!Time.CheckEvery(0.15f))
						return;

					var body = entity.Get<Body>();
					body.MoveBody();
					body.CheckSnakeCollidesWithChunk();
					body.CheckSnakeCollisionWithBorderOrItself();
				}
			}
		}
	}
}