using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class GameTests : TestWithMocksOrVisually
	{
		private void CreateAndStartGame()
		{
			game = new Game(Resolve<Window>());
			game.StartGame();
		}

		private Game game;

		[Test, CloseAfterFirstFrame]
		public void GameOverResultsInSameStateEvenMultipleCalls()
		{
			CreateAndStartGame();
			game.GameOver();
			game.GameOver();
			Assert.AreEqual(GameState.GameOver, game.GameState);
			Assert.IsFalse(game.InteractionLogics.Player.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void RestartGameGivesRunningGameAgain()
		{
			CreateAndStartGame();
			game.RestartGame();
			Assert.AreEqual(GameState.Playing, game.GameState);
		}
	}
}