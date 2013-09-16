using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Extensions;

namespace CreepyTowers
{
	public class GroupXmlParser
	{
		public GroupXmlParser()
		{
			CreepGroupsList = new List<Group>();
		}

		public List<Group> CreepGroupsList { get; private set; }

		public void ParseXml(string xmlName)
		{
			var xmlContent = ContentLoader.Load<XmlContent>(xmlName);
			if (xmlContent == null)
				return;

			foreach (XmlData group in xmlContent.Data.GetChildren("Group"))
				CreepGroupsList.Add(CreateNewGroup(group));
		}

		private static Group CreateNewGroup(XmlData group)
		{
			return new Group
			{
				Name = group.GetAttributeValue("Name"),
				CreepList = new List<string>(group.GetAttributeValue("CreepList").SplitAndTrim(',')),
				CreepSpawnTimeInterval = float.Parse(group.GetAttributeValue("CreepSpawnTimeInterval"))
			};
		}
	}
}