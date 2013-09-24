using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	/// <summary>
	/// The player's armed spaceship deployed against the dangerous rocks.
	/// Derived Sprite with Updatable giving it movement and fully automatic fireing of rockets. 
	/// </summary>
	public class PlayerShip : Sprite, Updateable
	{
		public PlayerShip()
			: base(new Material(Shader.Position2DColorUv, "ship1"), new Rectangle(Vector2D.Half, 
				new Size(.05f)))
		{
			Add(new Velocity2D(Vector2D.Zero, MaximumPlayerVelocity));
			RenderLayer = (int)AsteroidsRenderLayer.Player;
			projectileMaterial = new Material(Shader.Position2DColorUv, "projectile");
			timeLastShot = GlobalTime.Current.Milliseconds;
		}

		private const float MaximumPlayerVelocity = .5f;
		private readonly Material projectileMaterial;
		private float timeLastShot;

		public void ShipAccelerate()
		{
			Get<Velocity2D>().Accelerate(PlayerAcceleration * Time.Delta, Rotation);
		}

		private const float PlayerAcceleration = 1;

		public void SteerLeft()
		{
			Rotation -= PlayerTurnSpeed * Time.Delta;
		}

		public void SteerRight()
		{
			Rotation += PlayerTurnSpeed * Time.Delta;
		}

		private const float PlayerTurnSpeed = 160;
		private const float PlayerDecelFactor = 0.7f;

		public void Update()
		{
			MoveUpdate();
			if (!IsFiring || !(GlobalTime.Current.Milliseconds - 1 / CadenceShotsPerSec > timeLastShot))
				return;
			new Projectile(projectileMaterial, DrawArea.Center, Rotation);
			timeLastShot = GlobalTime.Current.Milliseconds;
			if (ProjectileFired != null)
				ProjectileFired.Invoke();
		}

		private const float CadenceShotsPerSec = 0.003f;
		public bool IsFiring { get; set; }
		public event Action ProjectileFired;

		private void MoveUpdate()
		{
			var nextRect = CalculateRectAfterMove(Time.Delta);
			MoveEntity(nextRect);
			var velocity2D = Get<Velocity2D>();
			velocity2D.velocity -= velocity2D.velocity * PlayerDecelFactor * Time.Delta;
			Set(velocity2D);
		}

		private Rectangle CalculateRectAfterMove(float deltaT)
		{
			return
				new Rectangle(
					DrawArea.TopLeft + Get<Velocity2D>().velocity * deltaT, Size);
		}

		private void MoveEntity(Rectangle rect)
		{
			StopAtBorder();
			Set(rect);
		}

		private void StopAtBorder()
		{
			var drawArea = DrawArea;
			var vel = Get<Velocity2D>();
			var borders = ScreenSpace.Current.Viewport;
			CheckStopRightBorder(ref drawArea, vel, borders);
			CheckStopLeftBorder(ref drawArea, vel, borders);
			CheckStopTopBorder(ref drawArea, vel, borders);
			CheckStopBottomBorder(ref drawArea, vel, borders);
			Set(vel);
			Set(drawArea);
		}

		private static void CheckStopRightBorder(ref Rectangle rect, Velocity2D vel, Rectangle borders)
		{
			if (rect.Right <= borders.Right)
				return;
			vel.velocity.X = -0.02f;
			rect.Right = borders.Right;
		}

		private static void CheckStopLeftBorder(ref Rectangle rect, Velocity2D vel, Rectangle borders)
		{
			if (rect.Left >= borders.Left)
				return;
			vel.velocity.X = 0.02f;
			rect.Left = borders.Left;
		}

		private static void CheckStopTopBorder(ref Rectangle rect, Velocity2D vel, Rectangle borders)
		{
			if (rect.Top >= borders.Top)
				return;
			vel.velocity.Y = 0.02f;
			rect.Top = borders.Top;
		}

		private static void CheckStopBottomBorder(ref Rectangle rect, Velocity2D vel, Rectangle borders)
		{
			if (rect.Bottom <= borders.Bottom)
				return;
			vel.velocity.Y = -0.02f;
			rect.Bottom = borders.Bottom;
		}
	}
}