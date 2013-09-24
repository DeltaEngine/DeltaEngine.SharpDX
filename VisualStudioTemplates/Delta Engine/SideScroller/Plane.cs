using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;

namespace $safeprojectname$
{
	public abstract class Plane : Sprite
	{
		protected Plane(Material texture, Vector2D initialPosition) : base(texture, 
			Rectangle.FromCenter(initialPosition, new Size(0.2f, 0.1f)))
		{
			Start<HitPointsHandler>();
		}

		internal float verticalDecelerationFactor, verticalAccelerationFactor;
		protected const float MaximumSpeed = 2;

		public int Hitpoints
		{
			get;
			protected set;
		}

		internal bool defeated;
		protected class HitPointsHandler : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var plane = entity as Plane;
					if (plane.Hitpoints <= 0)
						plane.defeated = true;
				}
			}
		}
		public void AccelerateVertically(float magnitude)
		{
			Get<Velocity2D>().Accelerate(new Vector2D(0, verticalAccelerationFactor * magnitude));
			verticalDecelerationFactor = 0.8f;
		}

		public void StopVertically()
		{
			verticalDecelerationFactor = 4.0f;
		}

		public float YPosition
		{
			get
			{
				return Get<Rectangle>().Center.Y;
			}
		}
	}
}