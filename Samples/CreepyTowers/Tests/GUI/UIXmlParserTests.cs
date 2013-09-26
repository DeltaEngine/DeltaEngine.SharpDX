using CreepyTowers.GUI;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.GUI
{
	public class UIXmlParserTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckXmlExists()
		{
				var xmlParser = new UIXmlParser();
				xmlParser.ParseXml(Names.XmlMenuScene, "MainMenu");
				Assert.IsNotNull(xmlParser.UiObjectList);
		}
	}
}
