using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class NamesTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckComicStripImagesList()
		{
			Assert.AreEqual(4, Names.ComicStripImages.Count);
			Assert.AreEqual(Names.ComicStripBubble, Names.ComicStripImages[0]);
		}

		[Test]
		public void CheckUiImagesList()
		{
			Assert.AreEqual(6, Names.UiImages.Count);
			Assert.AreEqual(Names.UIAvatarDragon, Names.UiImages[1]);
		}

		[Test]
		public void CheckUiButtonsList()
		{
			Assert.AreEqual(5, Names.UiButtons.Count);
			Assert.AreEqual(Names.UIUnicornSpecialAttackSwap, Names.UiButtons[2]);
		}
	}
}
