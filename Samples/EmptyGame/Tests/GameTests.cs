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
			Assert.AreEqual(0.0f, game.FadePercentage);
			Assert.AreEqual(initialColor, game.CurrentColor);
			Assert.AreEqual(initialColor, game.NextColor);
		}

		//[Test]
		//public void ChangeNextColorToFadeToAfterOneSecond()
		//{
		//	var game = new Game(Resolve<Window>());
		//	Color initialCurrentColor = game.CurrentColor;
		//	Color initialNextColor = game.NextColor;
		//	resolver.AdvanceTimeAndRunEntities(1.0f);
		//	Assert.IsTrue(game.FadePercentage > 0.9f);
		//	resolver.AdvanceTimeAndRunEntities(0.1f);
		//	Assert.AreEqual(initialCurrentColor, game.CurrentColor);
		//	Assert.AreNotEqual(initialNextColor, game.NextColor);
		//	Assert.IsTrue(game.FadePercentage < 0.1f);
		//}

		[Test]
		public void ContinuouslyChangeBackgroundColor()
		{
			new Game(Resolve<Window>());
		}
	}
}