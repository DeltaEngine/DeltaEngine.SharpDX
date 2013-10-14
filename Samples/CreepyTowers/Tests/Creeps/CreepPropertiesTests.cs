using System.Collections.Generic;
using CreepyTowers.Content;
using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	//TODO: this is pretty useless, test the actual loading, not just setting and getting values!
	/// <summary>
	/// Tests for parsing of the CreepProperties.xml
	/// </summary>
	public class CreepPropertiesTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void CreepDataShouldBeAsCreated()
		{
			CreepData creepProp = CreateCreepData();
			Assert.AreEqual(CreepModels.CreepCottonMummyHigh.ToString(), creepProp.Name);
			Assert.AreEqual(100, creepProp.MaxHp);
			Assert.AreEqual(2.0f, creepProp.Speed);
			Assert.AreEqual(0.5f, creepProp.Resistance);
			Assert.AreEqual(CreepType.Cloth, creepProp.Type);
			Assert.AreEqual(20, creepProp.GoldReward);
			Assert.AreEqual(2, creepProp.TypeDamageModifier.Count);
			Assert.AreEqual(1.0f, creepProp.TypeDamageModifier[TowerType.Acid]);
			Assert.AreEqual(3.0f, creepProp.TypeDamageModifier[TowerType.Fire]);
		}

		private static CreepData CreateCreepData()
		{
			var creepName = CreepModels.CreepCottonMummyHigh.ToString();
			var typeDamageModifier = new Dictionary<TowerType, float>();
			typeDamageModifier.Add(TowerType.Acid, 1.0f);
			typeDamageModifier.Add(TowerType.Fire, 3.0f);
			return new CreepData(CreepType.Cloth, creepName, 100, 2.0f, 0.5f, 20, typeDamageModifier);
		}

		//ncrunch: no coverage start
		[Test, Category("Slow"), CloseAfterFirstFrame]
		public void LoadCreepPropertiesXmlContentAndCheckNames()
		{
			var creepProperties = ContentLoader.Load<CreepPropertiesXml>("CreepProperties");
			Assert.IsTrue(creepProperties.Get(CreepType.Paper).Name.Contains("Paper"));
			Assert.IsTrue(creepProperties.Get(CreepType.Glass).Name.Contains("Glass"));
		}
	}
}