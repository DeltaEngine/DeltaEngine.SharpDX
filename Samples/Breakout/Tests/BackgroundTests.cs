using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class BackgroundTests : TestWithMocksOrVisually
	{
		[Test]
		public void Draw()
		{
			var background = Resolve<Background>();
			Assert.IsTrue(background.IsVisible == true);
		}
	}
}