using System.Collections.Generic;
using CreepyTowers.Content;
using CreepyTowers.Creeps;
using DeltaEngine.Content;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class GroupTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateGroupAndCheckValues()
		{
			var group = new Group
			{
				Name = "Squad",
				CreepList = new List<string> { CreepModels.CreepCottonMummyHigh.ToString() },
				CreepSpawnTimeInterval = 1.0f
			};
			Assert.AreEqual("Squad", group.Name);
			Assert.AreEqual(CreepModels.CreepCottonMummyHigh.ToString(), group.CreepList[0]);
			Assert.AreEqual(1.0f, group.CreepSpawnTimeInterval);
		}

		[Test, Category("Slow"), CloseAfterFirstFrame]
		public void CheckGroupXmlData()
		{
			var groupProperties = ContentLoader.Load<GroupPropertiesXml>("GroupProperties");
			Assert.AreEqual(3, groupProperties.Get("Squad").CreepList.Count);
			Assert.AreEqual(2, groupProperties.Get("IronMen").CreepList.Count);
		}
	}
}