using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Blocks.Tests
{
	public class UserInterfaceTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			content = new JewelBlocksContent();
			userInterface = new UserInterface(content);
		}

		private JewelBlocksContent content;
		private UserInterface userInterface;

		[Test]
		public void UserInterfaceShouldChangeFromLandscapeToPortrait()
		{
			userInterface.ShowUserInterfacePortrait();
		}

		[Test]
		public void OnLoseScoreShouldBeZero()
		{
			userInterface.Lose();
			Assert.AreEqual(0, userInterface.Score);
		}

		[Test]
		public void UserInterfaceShouldBeHidden()
		{
			userInterface.Dispose();
		}
	}
}