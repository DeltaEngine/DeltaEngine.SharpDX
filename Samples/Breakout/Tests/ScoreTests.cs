using System;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class ScoreTests : TestWithMocksOrVisually
	{
		[Test]
		public void IncreasePoints(Type type)
		{
			var score = Resolve<Score>();
			Assert.IsTrue(score.ToString().Contains("Score: 0"), score.ToString());
			score.IncreasePoints();
		}

		[Test]
		public void NextLevelWithoutInitialization(Type type)
		{
			var score = Resolve<Score>();
			bool isGameOver = false;
			score.GameOver += () => isGameOver = true;
			Assert.AreEqual(1, score.Level);
			score.NextLevel();
			Assert.AreEqual(2, score.Level);
			Assert.IsFalse(isGameOver);
		}

		[Test]
		public void NextLevelWithLevelInitialization(Type type)
		{
			var score = Resolve<Score>();
			Resolve<Level>();
			bool isGameOver = false;
			score.GameOver += () => isGameOver = true;
			Assert.AreEqual(1, score.Level);
			score.NextLevel();
			Assert.AreEqual(2, score.Level);
			Assert.IsFalse(isGameOver);
		}

		[Test]
		public void LoseLivesUntilGameOver(Type type)
		{
			var score = Resolve<Score>();
			bool isGameOver = false;
			score.GameOver += () => isGameOver = true;
			score.LifeLost();
			score.LifeLost();
			score.LifeLost();
			Assert.IsTrue(isGameOver);
		}
	}
}