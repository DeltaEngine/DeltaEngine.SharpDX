using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class PlayerShipTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			playerShip = new PlayerShip();
		}

		private PlayerShip playerShip;

		[Test, CloseAfterFirstFrame]
		public void Accelerate()
		{
			Vector2D originalVelocity = playerShip.Get<Velocity2D>().velocity;
			playerShip.ShipAccelerate();
			Assert.AreNotEqual(originalVelocity, playerShip.Get<Velocity2D>().velocity);
		}

		[Test, CloseAfterFirstFrame]
		public void TurnChangesAngleCorrectly()
		{
			float originalAngle = playerShip.Rotation;
			playerShip.SteerLeft();
			Assert.Less(playerShip.Rotation, originalAngle);
			originalAngle = playerShip.Rotation;
			playerShip.SteerRight();
			Assert.Greater(playerShip.Rotation, originalAngle);
		}

		[Test, CloseAfterFirstFrame]
		public void FireRocket()
		{
			bool firedRocket = false;
			playerShip.ProjectileFired += () => { firedRocket = true; };
			playerShip.IsFiring = true;
			AdvanceTimeAndUpdateEntities(2);
			Assert.IsTrue(firedRocket);
		}

		[Test, CloseAfterFirstFrame]
		public void HittingBordersTopLeft()
		{
			playerShip.Set(new Rectangle(ScreenSpace.Current.TopLeft - new Vector2D(0.1f, 0.1f), 
				new Size(.05f)));
		}

		[Test, CloseAfterFirstFrame]
		public void HittingBordersBottomRight()
		{
			playerShip.Set(new Rectangle(ScreenSpace.Current.BottomRight, new Size(.05f)));
		}

		[Test, CloseAfterFirstFrame]
		public void CreatedEntityIsPauseable()
		{
			Assert.IsTrue(playerShip.IsPauseable);
		}
	}
}