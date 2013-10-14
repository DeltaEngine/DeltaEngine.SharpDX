using System;
using System.Collections.Generic;
using System.IO;
using DeltaEngine.Content.Xml;
using DeltaEngine.Extensions;

namespace $safeprojectname$.Creeps
{
	public class GroupPropertiesXml : XmlContent
	{
		public GroupPropertiesXml(string contentName) : base(contentName)
		{
			creepGroupsList = new Dictionary<string, Group>();
		}

		private readonly Dictionary<string, Group> creepGroupsList;

		protected override void LoadData(Stream fileData)
		{
			base.LoadData(fileData);
			foreach (XmlData group in Data.GetChildren("Group"))
				try
				{
					var data = ParseGroupData(group);
					creepGroupsList [data.Name] = data;
				}
				catch (Exception ex)
				{
					throw new InvalidGroupProperties(ex);
				}
		}
		private class InvalidGroupProperties : Exception
		{
			public InvalidGroupProperties(Exception inner) : base("Invalid CombatProperties", inner)
			{
			}
		}
		private static Group ParseGroupData(XmlData group)
		{
			return new Group {
				Name = group.GetAttributeValue("Name"),
				CreepList = new List<string>(group.GetAttributeValue("CreepTypeList").SplitAndTrim(',')),
				CreepSpawnTimeInterval = float.Parse(group.GetAttributeValue("SpawnIntervalList"))
			};
		}

		public Group Get(string name)
		{
			return creepGroupsList [name];
		}
	}
}