using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	/// <summary>
	/// Tests for parsing of the CreepProperties.xml
	/// </summary>
	public class CreepPropertiesTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreepProperties()
		{
			creepProp = new CreepProperties
			{
				Name = Names.CreepCottonMummy,
				MaxHp = 100,
				CurrentHp = 50,
				Speed = 2.0f,
				Resistance = 0.5f,
				CreepType = Creep.CreepType.Cloth,
				GoldReward = 20,
				TypeDamageModifier =
					new Dictionary<Tower.TowerType, float>
					{
						{ Tower.TowerType.Acid, 1.0f },
						{ Tower.TowerType.Fire, 3.0f }
					}
			};
		}

		private CreepProperties creepProp;

		[Test, CloseAfterFirstFrame]
		public void CheckCreepProperties()
		{
			Assert.AreEqual(Names.CreepCottonMummy, creepProp.Name);
			Assert.AreEqual(100, creepProp.MaxHp);
			Assert.AreEqual(50, creepProp.CurrentHp);
			Assert.AreEqual(2.0f, creepProp.Speed);
			Assert.AreEqual(0.5f, creepProp.Resistance);
			Assert.AreEqual(Creep.CreepType.Cloth, creepProp.CreepType);
			Assert.AreEqual(20, creepProp.GoldReward);
			Assert.AreEqual(2, creepProp.TypeDamageModifier.Count);
			Assert.AreEqual(1.0f, creepProp.TypeDamageModifier[Tower.TowerType.Acid]);
			Assert.AreEqual(3.0f, creepProp.TypeDamageModifier[Tower.TowerType.Fire]);
		}
	}
}