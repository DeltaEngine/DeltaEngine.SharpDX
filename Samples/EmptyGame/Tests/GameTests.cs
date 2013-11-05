using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace EmptyGame.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[Test]
		public void Create()
		{
			var game = new Game(Resolve<Window>());
			var initialColor = new Color();
			Assert.AreEqual(0.0f, game.ElapsedTimeSinceColorChange);
			Assert.AreEqual(initialColor, game.CurrentColor);
			Assert.AreEqual(initialColor, game.NextColor);
		}

		[Test]
		public void ContinuouslyChangeBackgroundColor()
		{
			var game = new Game(Resolve<Window>());
			var initialColor = new Color();
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreNotEqual(initialColor, game.NextColor);
		}
	}
}