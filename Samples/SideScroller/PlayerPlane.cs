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
			PlayerFiredShot += point => { };
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
					CHeckIfHittingABorder(playerPlane);
					if (!isHittingABorder)
						CalculateRectAfterMove(playerPlane);
					var velocity2D = playerPlane.Get<Velocity2D>();
					velocity2D.velocity.Y -= velocity2D.velocity.Y * playerPlane.verticalDecelerationFactor *
						Time.Delta;
					playerPlane.Set(velocity2D);
					playerPlane.Rotation = RotationAccordingToVerticalSpeed();
				}
			}

			private static void CalculateRectAfterMove(PlayerPlane entity)
			{
				var pointAfterVerticalMovement = new Vector2D(entity.Get<Rectangle>().TopLeft.X,
					entity.Get<Rectangle>().TopLeft.Y + entity.Get<Velocity2D>().velocity.Y * Time.Delta);
				entity.Set(new Rectangle(pointAfterVerticalMovement, entity.Get<Rectangle>().Size));
			}

			private static void CHeckIfHittingABorder(PlayerPlane entity)
			{
				isHittingABorder = false;
				drawArea = entity.Get<Rectangle>();
				movementSpeed = entity.Get<Velocity2D>();
				CheckStopTopBorder(ScreenSpace.Current.Viewport);
				CheckStopBottomBorder(ScreenSpace.Current.Viewport);
				entity.Set(drawArea);
				entity.Set(movementSpeed);
			}

			private static Rectangle drawArea;
			private static Velocity2D movementSpeed;

			private static void CheckStopTopBorder(Rectangle borders)
			{
				if (drawArea.Top <= borders.Top && movementSpeed.velocity.Y < 0)
				{
					isHittingABorder = true;
					movementSpeed.velocity.Y = 0.02f;
					drawArea.Top = borders.Top;
				}
			}

			private static bool isHittingABorder;

			private static void CheckStopBottomBorder(Rectangle borders)
			{
				if (drawArea.Bottom >= borders.Bottom && movementSpeed.velocity.Y > 0)
				{
					isHittingABorder = true;
					movementSpeed.velocity.Y = -0.02f;
					drawArea.Bottom = borders.Bottom;
				}
			}

			private static float RotationAccordingToVerticalSpeed()
			{
				return 50 * movementSpeed.velocity.Y / MaximumSpeed;
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