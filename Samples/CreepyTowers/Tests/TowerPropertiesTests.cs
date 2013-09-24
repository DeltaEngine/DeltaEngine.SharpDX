using CreepyTowers.Towers;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	///<summary>
	///Tests for parsing of the TowerProperties.xml
	///</summary>
	public class TowerPropertiesTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void TowerProperties()
		{
			towerProp = new TowerProperties
			{
				Name = Names.TowerAcidConeJanitor,
				TowerType = Tower.TowerType.Acid,
				AttackType = Tower.AttackType.DirectShot,
				Range = 30.0f,
				AttackFrequency = 3.0f,
				AttackDamage = 30.0f,
				Cost = 100
			};
		}

		private TowerProperties towerProp;

		[Test, CloseAfterFirstFrame]
		public void LoadTowerPropertiesAndCheckNumberOfAvailableTowers()
		{
			Assert.AreEqual(Names.TowerAcidConeJanitor, towerProp.Name);
			Assert.AreEqual(Tower.TowerType.Acid, towerProp.TowerType);
			Assert.AreEqual(Tower.AttackType.DirectShot, towerProp.AttackType);
			Assert.AreEqual(30.0f, towerProp.Range);
			Assert.AreEqual(3.0f, towerProp.AttackFrequency);
			Assert.AreEqual(30.0f, towerProp.AttackDamage);
			Assert.AreEqual(100, towerProp.Cost);
		}
	}
}