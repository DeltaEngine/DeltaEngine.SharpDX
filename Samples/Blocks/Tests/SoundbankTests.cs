using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Blocks.Tests
{
	/// <summary>
	/// Unit tests for Soundbank class
	/// </summary>
	public class SoundbankTests : TestWithMocksOrVisually
	{
		[Test]
		public void Constructor()
		{
			var soundbank = new Soundbank(new JewelBlocksContent());
			Assert.IsNotNull(soundbank.BlockAffixed);
			Assert.IsNotNull(soundbank.BlockCouldntMove);
			Assert.IsNotNull(soundbank.BlockMoved);
			Assert.IsNotNull(soundbank.GameLost);
			Assert.IsNotNull(soundbank.MultipleRowsRemoved);
			Assert.IsNotNull(soundbank.RowRemoved);
		}
	}
}