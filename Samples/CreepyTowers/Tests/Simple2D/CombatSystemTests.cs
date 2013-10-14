using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using NUnit.Framework;

namespace CreepyTowers.Tests.Simple2D
{
	public class CombatSystemTests : TestWithBasic2DDisplaySystem
	{
		[SetUp]
		public void SetUpCreep()
		{
			creep = new Creep2D(display, new Vector2D(0, 1), CreepType.Cloth);
		}

		private Creep2D creep;

		[TestCase(TowerType.Water, 107.5f),
		 TestCase(TowerType.Fire, 107.5f),
		 TestCase(TowerType.Ice, 107.5f),
		 TestCase(TowerType.Slice, 107.5f),
		 TestCase(TowerType.Impact, 107.5f),
		 TestCase(TowerType.Acid, 107.0f)]
		public void CheckForCreepDamageOutputInImmuneState(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep2D>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Immune);
			AdvanceTimeAndUpdateEntities(2.4f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}
	}
}