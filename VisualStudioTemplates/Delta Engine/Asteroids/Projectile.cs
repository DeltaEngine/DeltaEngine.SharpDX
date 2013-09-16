using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Physics2D;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class Projectile : Sprite
	{
		public Projectile(Material texture, Point startPosition, float angle) : base(texture, 
			Rectangle.FromCenter(startPosition, new Size(.02f)))
		{
			Rotation = angle;
			RenderLayer = (int)AsteroidsRenderLayer.Rockets;
			Add(new SimplePhysics.Data {
				Gravity = Point.Zero,
				Velocity = new Point(MathExtensions.Sin(angle) * ProjectileVelocity, 
					-MathExtensions.Cos(angle) * ProjectileVelocity)
			});
			Start<MoveAndDisposeOnBorderCollision>();
		}

		private const float ProjectileVelocity = .5f;
		private class MoveAndDisposeOnBorderCollision : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var projectile = entity as Projectile;
					projectile.DrawArea = CalculateFutureDrawArea(projectile, Time.Delta);
					if (ObjectHasCrossedScreenBorder(projectile.DrawArea, ScreenSpace.Current.Viewport))
						projectile.IsActive = false;
				}
			}

			private static Rectangle CalculateFutureDrawArea(Projectile projectile, float deltaT)
			{
				return new Rectangle(projectile.DrawArea.TopLeft + 
					projectile.Get<SimplePhysics.Data>().Velocity * deltaT, projectile.DrawArea.Size);
			}

			private static bool ObjectHasCrossedScreenBorder(Rectangle objectArea, Rectangle borders)
			{
				return (objectArea.Right <= borders.Left || objectArea.Left >= borders.Right || 
					objectArea.Bottom <= borders.Top || objectArea.Top >= borders.Bottom);
			}
		}
	}
}