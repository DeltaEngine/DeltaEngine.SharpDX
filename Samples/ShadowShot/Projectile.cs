using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Physics2D;
using DeltaEngine.Rendering.Sprites;

namespace ShadowShot
{
	public class Projectile : Sprite, IDisposable
	{
		public Projectile(Material projectileMaterial, Point centerPoint, Rectangle borders)
			: base(projectileMaterial, Rectangle.FromCenter(centerPoint, ProjectileSize))
		{
			this.borders = borders;
			RenderLayer = (int)Constants.RenderLayer.Rockets;
			Add(new SimplePhysics.Data { Gravity = Point.Zero, Velocity = new Point(0.0f, -1.0f), });
			Start<MovementHandler>();
		}

		private static readonly Size ProjectileSize = new Size(0.008f, 0.015f);
		private Rectangle borders;

		private class MovementHandler : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Projectile projectile in entities)
				{
					projectile.DrawArea = CalculateFutureDrawArea(projectile);

					if (ObjectHasCrossedScreenBorder(projectile.DrawArea, projectile.borders))
						projectile.IsActive = false;
				}
			}

			private static Rectangle CalculateFutureDrawArea(Projectile projectile)
			{
				return
					new Rectangle(
						projectile.DrawArea.TopLeft + projectile.Get<SimplePhysics.Data>().Velocity * Time.Delta,
						projectile.DrawArea.Size);
			}

			private bool ObjectHasCrossedScreenBorder(Rectangle objectArea, Rectangle borders)
			{
				return (objectArea.Right <= borders.Left ||
					objectArea.Left >= borders.Right || objectArea.Bottom <= borders.Top ||
					objectArea.Top >= borders.Bottom);
			}
		}

		public void Dispose()
		{
			IsActive = false;
		}
	}
}