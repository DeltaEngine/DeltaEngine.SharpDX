using CreepyTowers.Content;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Towers
{
	//TODO: Add more useful tests to actually load and check tower settings
	/// <summary>
	/// Tests for parsing of the TowerProperties.xml
	/// </summary>
	public class TowerPropertiesTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void TowerDataShouldBeAsCreated()
		{
			TowerData towerData = CreateTowerData();
			Assert.AreEqual(TowerModels.TowerAcidConeJanitorHigh.ToString(), towerData.Name);
			Assert.AreEqual(TowerType.Acid, towerData.Type);
			Assert.AreEqual(AttackType.DirectShot, towerData.AttackType);
			Assert.AreEqual(30.0f, towerData.Range);
			Assert.AreEqual(3.0f, towerData.AttackFrequency);
			Assert.AreEqual(30.0f, towerData.AttackDamage);
			Assert.AreEqual(100, towerData.Cost);
		}

		private static TowerData CreateTowerData()
		{
			return new TowerData(TowerType.Acid, TowerModels.TowerAcidConeJanitorHigh.ToString(),
				AttackType.DirectShot, 30.0f, 3.0f, 30.0f, 100);
		}

		[Test, Category("Slow"), CloseAfterFirstFrame]
		public void LoadTowerPropertiesXmlContentAndCheckNames()
		{
			var towerProperties = ContentLoader.Load<TowerPropertiesXml>("TowerProperties");
			Assert.IsTrue(towerProperties.Get(TowerType.Acid).Name.Contains("Acid"));
			Assert.IsTrue(towerProperties.Get(TowerType.Water).Name.Contains("Water"));
		}
	}
}