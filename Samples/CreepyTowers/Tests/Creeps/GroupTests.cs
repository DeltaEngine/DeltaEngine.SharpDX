using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class GroupTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateGroup()
		{
			var group = new Group
			{
				Name = "Squad",
				CreepList = new List<string> { Names.CreepCottonMummy },
				CreepSpawnTimeInterval = 1.0f
			};

			Assert.AreEqual("Squad", group.Name);
			Assert.AreEqual(Names.CreepCottonMummy, group.CreepList[0]);
			Assert.AreEqual(1.0f, group.CreepSpawnTimeInterval);
		}

		[Test, CloseAfterFirstFrame]
		public void CheckGroupXmlData()
		{
			var groupXmlParser = new GroupXmlParser();
			groupXmlParser.ParseXml(Names.XmlGroupCreeps);
			Assert.IsNotNull(groupXmlParser.CreepGroupsList);
		}
	}
}