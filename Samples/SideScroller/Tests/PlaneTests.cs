using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace SideScroller.Tests
{
	internal class PlaneTests : TestWithMocksOrVisually
	{
		[Test]
		public void FireShotEvent()
		{
			InitPlayerPlane();
			bool shotfired = false;
			playerPlane.PlayerFiredShot += point =>
			{
				Assert.AreEqual(playerPlane.Get<Rectangle>().Center, point);
				shotfired = true;
			};
			playerPlane.IsFireing = true;
			AdvanceTimeAndUpdateEntities(0.2f);
			Assert.IsTrue(shotfired);
		}

		private void InitPlayerPlane()
		{
			var material = new Material(Shader.Position2DColorUV, PlaneTextureName);
			playerPlane = new PlayerPlane(material, Vector2D.Half);
		}

		private PlayerPlane playerPlane;
		private const string PlaneTextureName = "PlayerPlane";

		[Test]
		public void MovePlaneVertically()
		{
			InitPlayerPlane();
			CheckMoveUp();
			CheckMoveDown();
			CheckStop();
		}

		private void CheckMoveUp()
		{
			var originalYCoord = playerPlane.YPosition;
			playerPlane.AccelerateVertically(1);
			AdvanceTimeAndUpdateEntities();
			Assert.Greater(playerPlane.YPosition, originalYCoord);
		}

		private void CheckMoveDown()
		{
			var originalYCoord = playerPlane.YPosition;
			playerPlane.AccelerateVertically(-1);
			AdvanceTimeAndUpdateEntities();
			Assert.Less(playerPlane.YPosition, originalYCoord);
		}

		private void CheckStop()
		{
			playerPlane.AccelerateVertically(3);
			var originalSpeed = playerPlane.Get<Velocity2D>().velocity.Y;
			playerPlane.StopVertically();
			AdvanceTimeAndUpdateEntities();
			Assert.Less(playerPlane.Get<Velocity2D>().velocity.Y, originalSpeed);
		}

		[Test]
		public void HittingTopBorder()
		{
			InitPlayerPlane();
			playerPlane.DrawArea = new Rectangle(new Vector2D(playerPlane.DrawArea.Left, -0.5f),
				playerPlane.DrawArea.Size);
			playerPlane.Set(new Velocity2D(new Vector2D(0, -0.1f), 0.5f));
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(ScreenSpace.Current.Viewport.Top, playerPlane.DrawArea.Top);
		}

		[Test]
		public void HittingBottomBorder()
		{
			InitPlayerPlane();
			playerPlane.DrawArea = new Rectangle(new Vector2D(playerPlane.DrawArea.Left, 1.5f),
				playerPlane.DrawArea.Size);
			playerPlane.Set(new Velocity2D(new Vector2D(0, 0.1f), 0.5f));
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(ScreenSpace.Current.Viewport.Bottom, playerPlane.DrawArea.Bottom);
		}

		[Test]
		public void CreatePlayerPlane()
		{
			InitPlayerPlane();
		}
	}
}