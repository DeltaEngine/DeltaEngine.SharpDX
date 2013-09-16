using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Physics2D;
using DeltaEngine.Rendering.Sprites;

namespace ShadowShot
{
	public class PlayerShip : Sprite, IDisposable
	{
		public PlayerShip(Material material, Rectangle drawArea, Rectangle borders)
			: base(material, drawArea)
		{
			timeLastShot = GlobalTime.Current.Milliseconds;
			viewportBorders = borders;
			Add(new Velocity2D.Data(Point.Zero, Constants.MaximumObjectVelocity));
			Start<MovementHandler>();
			Start<ProjectileHandler>();
			RenderLayer = (int)Constants.RenderLayer.PlayerShip;
			ProjectileFired += SpawnProjectile;
		}

		private float timeLastShot;
		private Rectangle viewportBorders;

		public void Accelerate(Point accelerateDirection)
		{
			var direction = new Point(accelerateDirection.X * Time.Delta, accelerateDirection.Y);
			Get<Velocity2D.Data>().Accelerate(direction);
		}

		private class MovementHandler : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (PlayerShip ship in entities)
				{
					var nextRect = CalculateRectAfterMove(ship);
					MoveEntity(ship, nextRect);
					var velocity2D = ship.Get<Velocity2D.Data>();
					velocity2D.Velocity -= velocity2D.Velocity * Constants.PlayerDecelFactor * Time.Delta;
					ship.Set(velocity2D);
				}
			}

			private static Rectangle CalculateRectAfterMove(PlayerShip entity)
			{
				return
					new Rectangle(
						entity.Get<Rectangle>().TopLeft + entity.Get<Velocity2D.Data>().Velocity * Time.Delta,
						entity.Get<Rectangle>().Size);
			}

			private static void MoveEntity(PlayerShip entity, Rectangle rect)
			{
				StopAtBorder(entity);
				entity.Set(rect);
			}

			private static void StopAtBorder(PlayerShip entity)
			{
				var rect = entity.Get<Rectangle>();
				var vel = entity.Get<Velocity2D.Data>();
				if (rect.Left < entity.viewportBorders.Left)
				{
					vel.Accelerate(0);
					vel.Accelerate(new Point(0.02f, 0));
					rect.Left = entity.viewportBorders.Left;
				}

				if (rect.Right > entity.viewportBorders.Right)
				{
					vel.Accelerate(0);
					vel.Accelerate(new Point(-0.02f, 0));
					rect.Right = entity.viewportBorders.Right;
				}

				entity.Set(vel);
				entity.Set(rect);
			}
		}

		private class ProjectileHandler : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (PlayerShip ship in entities)
				{
					foreach (Projectile projectile in ship.addProjectileList)
						if (projectile.IsActive)
							ship.ActiveProjectileList.Add(projectile);

					ship.addProjectileList.Clear();
				}
			}
		}

		private readonly List<Projectile> addProjectileList = new List<Projectile>();
		public List<Projectile> ActiveProjectileList = new List<Projectile>();

		public event Action<Point> ProjectileFired;

		private void SpawnProjectile(Point point)
		{
			var projectile = new Projectile(new Material(Shader.Position2DColorUv, "projectile"), point,
				viewportBorders);
			addProjectileList.Add(projectile);
		}

		public void Fire()
		{
			if (GlobalTime.Current.Milliseconds - 1 / PlayerCadance > timeLastShot)
			{
				timeLastShot = GlobalTime.Current.Milliseconds;
				if (ProjectileFired != null)
					ProjectileFired(DrawArea.Center);
			}
		}

		private const float PlayerCadance = 0.003f;

		public void Deccelerate()
		{
			Get<Velocity2D.Data>().Accelerate(0.7f);
		}

		public void Dispose()
		{
			ProjectileFired -= SpawnProjectile;

			foreach (Projectile projectile in ActiveProjectileList)
				projectile.Dispose();
			IsActive = false;
		}
	}
}