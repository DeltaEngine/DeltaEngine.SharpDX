using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class GlassCreepStateChangerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateGlassCreep()
		{
			creep = new Creep(CreepType.Glass, Vector3D.Zero, 0);
		}

		private Creep creep;

		[Test]
		public void CheckForFireTower()
		{
			GlassCreepStateChanger.ChangeStatesIfGlassCreep(TowerType.Fire, creep);
			Assert.IsTrue(creep.state.Melt);
			Assert.IsTrue(creep.state.Slow);
			Assert.IsTrue(creep.state.Enfeeble);
		}

		[Test]
		public void CheckForImpactTower()
		{
			GlassCreepStateChanger.ChangeStatesIfGlassCreep(TowerType.Impact, creep);
			Assert.IsFalse(creep.state.Sudden);
		}
	}
}