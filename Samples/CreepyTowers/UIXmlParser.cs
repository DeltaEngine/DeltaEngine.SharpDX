using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace CreepyTowers
{
	public class UIXmlParser
	{
		public UIXmlParser()
		{
			UIObjectList = new List<UIObject>();
		}

		public List<UIObject> UIObjectList { get; private set; }

		public void ParseXml(string xmlName, string childName)
		{
			UIObjectList.Clear();
			var xmlContent = ContentLoader.Load<XmlContent>(xmlName);
			if (xmlContent == null)
				return;

			var menu = xmlContent.Data.GetChild(childName);
			if (menu == null)
				return;
			var data = menu.GetChildren("Item");
			foreach (XmlData item in data)
				UIObjectList.Add(GetLevelObjectDefinition(item));
		}

		private static UIObject GetLevelObjectDefinition(XmlData item)
		{
			return new UIObject
			{
				Name = item.GetAttributeValue("Name"),
				Position =
					new Point(float.Parse(item.GetAttributeValue("PositionX")),
						float.Parse(item.GetAttributeValue("PositionY"))),
				ObjectSize =
					new Size(float.Parse(item.GetAttributeValue("Width")),
						float.Parse(item.GetAttributeValue("Height")))
			};
		}
	}
}