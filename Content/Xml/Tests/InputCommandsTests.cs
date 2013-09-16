using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Content.Xml.Tests
{
	internal class InputCommandsTests : TestWithMocksOrVisually
	{
		[Test]
		public void TestInputCommands()
		{
			var inputCommands = ContentLoader.Load<InputCommands>("DefaultCommands");
			inputCommands.InternalCreateDefault();
			Assert.AreEqual("DefaultCommands", inputCommands.Name);
			Assert.IsTrue(inputCommands.InternalAllowCreationIfContentNotFound);
		}
	}
}