using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class EnemyPlane : Plane
	{
		public EnemyPlane(Material enemyTexture, Point initialPosition) : base(enemyTexture, 
			initialPosition)
		{
			Hitpoints = 5;
			verticalDecelerationFactor = 3.0f;
			verticalAccelerationFactor = 1.5f;
			RenderLayer = (int)DefRenderLayer.Player;
			Add(new Velocity2D(new Point(-0.5f, 0), MaximumSpeed));
			Start<EnemyHandler>();
		}

		public void CheckIfHitAndReact(Point playerShotStartPosition)
		{
			if (Math.Abs(playerShotStartPosition.Y - Center.Y) < 0.1f)
				Hitpoints--;
		}
		private class EnemyHandler : UpdateBehavior
		{
			private static Rectangle CalculateRectAfterMove(Entity entity)
			{
				var pointAfterVerticalMovement = new Point(entity.Get<Rectangle>().TopLeft.X + 
					entity.Get<Velocity2D>().velocity.X * Time.Delta, entity.Get<Rectangle>().TopLeft.Y + 
						entity.Get<Velocity2D>().velocity.Y * Time.Delta);
				return new Rectangle(pointAfterVerticalMovement, entity.Get<Rectangle>().Size);
			}

			private static void MoveEntity(Entity entity, Rectangle rect)
			{
				entity.Set(rect);
			}

			private static float RotationAccordingToVerticalSpeed(Point vel)
			{
				return -50 * vel.Y / MaximumSpeed;
			}

			private static void FireShotIfRightTime(EnemyPlane entity)
			{
				if (entity.timeLastShot - GlobalTime.Current.Milliseconds > 1)
				{
					entity.timeLastShot = GlobalTime.Current.Milliseconds;
					entity.EnemyFiredShot(entity.Center);
				}
			}

			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var enemy = entity as EnemyPlane;
					if (enemy.defeated)
					{
						enemy.AccelerateVertically(0.02f);
						if (enemy.DrawArea.Top > ScreenSpace.Current.Viewport.Bottom)
							enemy.IsActive = false;
					} else
						FireShotIfRightTime(enemy);
					var newRect = CalculateRectAfterMove(enemy);
					MoveEntity(enemy, newRect);
					var velocity2D = enemy.Get<Velocity2D>();
					velocity2D.velocity.Y -= velocity2D.velocity.Y * enemy.verticalDecelerationFactor * 
						Time.Delta;
					enemy.Set(velocity2D);
					enemy.Rotation = RotationAccordingToVerticalSpeed(velocity2D.velocity);
					if (enemy.DrawArea.Right < ScreenSpace.Current.Viewport.Left)
						entity.IsActive = false;
				}
			}
		}
		internal float timeLastShot;

		public event Action<Point> EnemyFiredShot;
	}
}