using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace $safeprojectname$.GUI
{
	public class UIXmlParser
	{
		public UIXmlParser()
		{
			UiObjectList = new List<UIObject>();
		}

		public List<UIObject> UiObjectList
		{
			get;
			private set;
		}

		public void ParseXml(string xmlName, string childName)
		{
			UiObjectList.Clear();
			var xmlContent = ContentLoader.Load<XmlContent>(xmlName);
			if (xmlContent == null)
				return;

			var menu = xmlContent.Data.GetChild(childName);
			if (menu == null)
				return;

			var data = menu.GetChildren("Item");
			foreach (XmlData item in data)
				UiObjectList.Add(GetLevelObjectDefinition(item));
		}

		private static UIObject GetLevelObjectDefinition(XmlData item)
		{
			return new UIObject {
				Name = item.GetAttributeValue("Name"),
				Type = item.GetAttributeValue("Type"),
				Position = new Vector2D(float.Parse(item.GetAttributeValue("PositionX")), 
					float.Parse(item.GetAttributeValue("PositionY"))),
				ObjectSize = new Size(float.Parse(item.GetAttributeValue("Width")), 
					float.Parse(item.GetAttributeValue("Height")))
			};
		}
	}
}