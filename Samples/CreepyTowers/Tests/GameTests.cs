using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			game = new Game(Resolve<Window>(), Resolve<Device>());
		}

		private Game game;

		[Test]
		public void CheckGameCreation()
		{
			Assert.AreEqual("Creepy Towers", Game.window.Title);
			Assert.AreEqual(new Size(1920, 1080), Game.window.ViewportPixelSize);
			Assert.IsFalse(Game.window.IsFullscreen);
			Assert.AreEqual(6.0f, game.MaxZoomedOutFovSize);
		}

		[Test, CloseAfterFirstFrame]
		public void EndGameClosesGameWindow()
		{
			Game.EndGame();
			Assert.IsTrue(Game.window.IsClosing);
		}
	}
}