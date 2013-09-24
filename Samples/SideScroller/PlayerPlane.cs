using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.ScreenSpaces;

namespace SideScroller
{
	public class PlayerPlane : Plane
	{
		/// <summary>
		/// Special behavior of the player's plane for use in the side-scrolling shoot'em'up.
		/// Can be controlled in its vertical position, fire shots and make the environment move.
		/// (It is so mighty that when it flies faster, in truth the world turns faster!)
		/// </summary>
		public PlayerPlane(Material playerTexture, Vector2D initialPosition)
			: base(playerTexture, initialPosition)

		{
			Hitpoints = 3;
			verticalDecelerationFactor = 3.0f;
			verticalAccelerationFactor = 1.5f;
			RenderLayer = (int)DefRenderLayer.Player;
			PlayerFiredShot += point => {};
			Add(new Velocity2D(Vector2D.Zero, MaximumSpeed));
			Start<PlayerMovement>();
			Start<MachineGunFire>();
		}

		protected class PlayerMovement : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var playerPlane = entity as PlayerPlane;
					var newRect = CalculateRectAfterMove(playerPlane);
					MoveEntity(playerPlane, newRect);
					var velocity2D = playerPlane.Get<Velocity2D>();
					velocity2D.velocity.Y -= velocity2D.velocity.Y * playerPlane.verticalDecelerationFactor *
						Time.Delta;
					playerPlane.Set(velocity2D);
					playerPlane.Rotation = RotationAccordingToVerticalSpeed(velocity2D.velocity);
				}
			}

			private static Rectangle CalculateRectAfterMove(PlayerPlane entity)
			{
				var pointAfterVerticalMovement = new Vector2D(entity.Get<Rectangle>().TopLeft.X,
					entity.Get<Rectangle>().TopLeft.Y + entity.Get<Velocity2D>().velocity.Y * Time.Delta);

				return new Rectangle(pointAfterVerticalMovement, entity.Get<Rectangle>().Size);
			}

			private void MoveEntity(PlayerPlane entity, Rectangle rect)
			{
				StopAtBorderVertically(entity);
				entity.Set(rect);
			}

			private static void StopAtBorderVertically(PlayerPlane entity)
			{
				var rect = entity.Get<Rectangle>();
				var vel = entity.Get<Velocity2D>();
				CheckStopTopBorder(rect, vel, ScreenSpace.Current.Viewport);
				CheckStopBottomBorder(rect, vel, ScreenSpace.Current.Viewport);
				entity.Set(vel);
				entity.Set(rect);
			}

			private static void CheckStopTopBorder(Rectangle rect, Velocity2D vel, Rectangle borders)
			{
				if (rect.Top <= borders.Top && vel.velocity.Y < 0)
				{
					vel.velocity.Y = 0.02f;
					rect.Top = borders.Top;
				}
			}

			private static void CheckStopBottomBorder(Rectangle rect, Velocity2D vel, Rectangle borders)
			{
				if (rect.Bottom >= borders.Bottom && vel.velocity.Y > 0)
				{
					vel.velocity.Y = -0.02f;
					rect.Bottom = borders.Bottom;
				}
			}

			private static float RotationAccordingToVerticalSpeed(Vector2D vel)
			{
				return 50 * vel.Y / MaximumSpeed;
			}
		}

		private class MachineGunFire : UpdateBehavior
		{
			public MachineGunFire()
			{
				timeLastShot = GlobalTime.Current.Milliseconds;
			}

			private float timeLastShot;

			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var playerPlane = entity as PlayerPlane;
					if (playerPlane.IsFireing && GlobalTime.Current.Milliseconds - 1 / Cadence > timeLastShot)
					{
						playerPlane.PlayerFiredShot(playerPlane.Get<Rectangle>().Center);
						timeLastShot = GlobalTime.Current.Milliseconds;
					}
				}
			}
		}

		public bool IsFireing;
		public const float Cadence = 0.01f;
		public event Action<Vector2D> PlayerFiredShot;
	}
}