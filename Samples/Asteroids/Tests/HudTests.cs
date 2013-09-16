using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class HudTests : TestWithMocksOrVisually
	{
		[Test]
		public void SetScore()
		{
			var hud = new HudInterface();
			hud.SetScoreText(5);
			Assert.AreEqual("5", hud.ScoreDisplay.Text);
		}
	}
}