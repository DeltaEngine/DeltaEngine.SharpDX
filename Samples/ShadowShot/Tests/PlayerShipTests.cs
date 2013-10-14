using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Physics2D;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace ShadowShot.Tests
{
	public class PlayerShipTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			Resolve<Window>().ViewportPixelSize = new Size(500, 500);
			var shipImage = new Material(Shader.Position2DColorUV, "player");
			var shipDrawArea = Rectangle.FromCenter(new Vector2D(0.5f, 0.95f), playerShipSize);
			playerShip = new PlayerShip(shipImage, shipDrawArea, Resolve<ScreenSpace>().Viewport);
		}

		private PlayerShip playerShip;
		private readonly Size playerShipSize = new Size(0.05f);

		[Test]
		public void CreateShip() {}

		[Test]
		public void Accelerate()
		{
			Vector2D originalVelocity = playerShip.Get<Velocity2D.Data>().Velocity;
			playerShip.Accelerate(Vector2D.One);
			Assert.AreNotEqual(originalVelocity, playerShip.Get<Velocity2D.Data>().Velocity);
			playerShip.Accelerate(new Vector2D(0, 1.0f));
			Assert.AreNotEqual(originalVelocity, playerShip.Get<Velocity2D.Data>().Velocity);
		}

		[Test]
		public void HittingBordersBottomRight()
		{
			var screenspace = Resolve<ScreenSpace>();
			playerShip.Set(new Rectangle(screenspace.BottomRight, playerShipSize));
			Assert.LessOrEqual(screenspace.Viewport.BottomRight.X, playerShip.DrawArea.BottomRight.X);
			Assert.LessOrEqual(screenspace.Viewport.BottomRight.Y, playerShip.DrawArea.BottomRight.Y);
		}

		[Test]
		public void HittingBordersBottomLeft()
		{
			playerShip.Set(new Rectangle(new Vector2D(0.0f, 1.0f), playerShipSize));
			Assert.LessOrEqual(0.0f, playerShip.DrawArea.BottomRight.X);
			Assert.LessOrEqual(1.0, playerShip.DrawArea.BottomRight.Y);
		}

		[Test]
		public void PressSpacebarToFireWeapon()
		{
			new Command(() => playerShip.Fire()).Add(new KeyTrigger(Key.Space, State.Pressed));
		}

		[Test]
		public void CheckForWeaponFire()
		{
			var game = new Game(Resolve<Window>(),Resolve<ScreenSpace>());
			game.InitializeGame();
			bool weaponFired = false;
			game.Ship.ProjectileFired += point =>
			{
				weaponFired = true;
			};
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.Space, State.Pressed);
			AdvanceTimeAndUpdateEntities(0.5f);
			Assert.IsTrue(weaponFired);
		}

		[Test]
		public void CollisonWithAsteroidEndsGame()
		{
			playerShip.DrawArea = new Rectangle(Vector2D.Half, playerShip.Size);
			new GameController(playerShip, new Material(Shader.Position2DColorUV, "asteroid"),
				playerShip.Size, Resolve<ScreenSpace>());
		}

		[Test]
		public void DisposePlayerShip()
		{
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.Space, State.Pressed);
			playerShip.Dispose();
			Assert.AreEqual(0, playerShip.ActiveProjectileList.Count);
			Assert.IsFalse(playerShip.IsActive);
		}
	}
}