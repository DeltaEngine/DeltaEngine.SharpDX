using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Blocks.Tests
{
	/// <summary>
	/// Unit test for JewelBlocksContent
	/// </summary>
	public class JewelBlocksContentTests : TestWithMocksOrVisually
	{
		[Test]
		public void Constructor()
		{
			var content = new JewelBlocksContent();
			Assert.AreEqual("JewelBlocks_", content.Prefix);
			Assert.IsTrue(content.AreFiveBrickBlocksAllowed);
			Assert.IsFalse(content.DoBricksSplitInHalfWhenRowFull);
		}
	}
}