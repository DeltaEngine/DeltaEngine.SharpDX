using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
//TODO: fix all Creep constructor calls
	public class CalculateDamageTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>());
		}

		[Test]
		public void CheckResistanceBasedOnImmuneType()
		{
			var creep = new Creep(CreepType.Glass, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Acid);
			Assert.AreEqual(0.1f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnVulnerableType()
		{
			var creep = new Creep(CreepType.Glass, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Impact);
			Assert.AreEqual(3.0f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnResistantType()
		{
			var creep = new Creep(CreepType.Sand, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Impact);
			Assert.AreEqual(0.5f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnHardboiledType()
		{
			var creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Ice);
			Assert.AreEqual(0.25f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnWeakType()
		{
			var creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Slice);
			Assert.AreEqual(2.0f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnNormalDamageType()
		{
			var creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
			var resistance = creep.CalculateResistanceBasedOnStates(TowerType.Water);
			Assert.AreEqual(1.0f, resistance);
		}
	}
}