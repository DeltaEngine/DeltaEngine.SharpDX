using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class CalculateDamageTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			movementData = new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			};

			damageCalculator = new CalculateDamage();
		}

		private MovementInGrid.MovementData movementData;
		private CalculateDamage damageCalculator;

		[Test]
		public void CheckResistanceBasedOnImmuneType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Acid,
				creep);
			Assert.AreEqual(0.1f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnVulnerableType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Impact,
				creep);
			Assert.AreEqual(3.0f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnResistantType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Sand, Names.CreepsSandSandy);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Impact,
				creep);
			Assert.AreEqual(0.5f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnHardboiledType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Ice,
				creep);
			Assert.AreEqual(0.25f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnWeakType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Blade,
				creep);
			Assert.AreEqual(2.0f, resistance);
		}

		[Test]
		public void CheckResistanceBasedOnNormalDamageType()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			creep.Add(movementData);
			var resistance = damageCalculator.CalculateResistanceBasedOnStates(Tower.TowerType.Water,
				creep);
			Assert.AreEqual(1.0f, resistance);
		}
	}
}