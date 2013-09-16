using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class UIXmlParserTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckXmlExists()
		{
				var xmlParser = new UIXmlParser();
				xmlParser.ParseXml(Names.XmlMenuScene, "MainMenu");
				Assert.IsNotNull(xmlParser.UIObjectList);
		}
	}
}
