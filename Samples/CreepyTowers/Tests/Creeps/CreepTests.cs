using CreepyTowers.Creeps;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class CreepTests : TestWithMocksOrVisually
	{
		[TestCase(CreepType.Cloth),
		TestCase(CreepType.Glass),
		TestCase(CreepType.Iron),
		TestCase(CreepType.Paper),
		TestCase(CreepType.Plastic),
		TestCase(CreepType.Sand),
		TestCase(CreepType.Wood)]
		public void RenderCreepIn3D(CreepType type)
		{
			new Creep(type, Vector3D.Zero, 0.0f);
		}

		//[Test]
		//public void DisplayCreepWalkingAlongXAxisForward()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Position = grid.PropertyMatrix[6, 0].MidPoint;
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(1.0f, 0.0f, 0.0f),
		//		StartGridPos = new Tuple<int, int>(0, 0),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(14, 0) }
		//	});

		//	Assert.LessOrEqual(grid.PropertyMatrix[14, 0].MidPoint.X, creep.Position.X);
		//}

		//[Test]
		//public void DisplayCreepWalkingAlongXAxisBackward()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Position = grid.PropertyMatrix[4, 0].MidPoint;
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(1.0f, 0.0f, 0.0f),
		//		StartGridPos = new Tuple<int, int>(4, 0),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
		//	});
		//}

		//[Test]
		//public void DisplayCreepWalkingAlongZAxisForward()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Position = grid.PropertyMatrix[0, 10].MidPoint;
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.0f, 0.0f, 0.5f),
		//		StartGridPos = new Tuple<int, int>(0, 0),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 16) }
		//	});

		//	Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		//}

		//[Test]
		//public void DisplayCreepWalkingAlongZAxisBackwards()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Position = grid.PropertyMatrix[0, 8].MidPoint;
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.0f, 0.0f, 1.0f),
		//		StartGridPos = new Tuple<int, int>(0, 10),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 4) }
		//	});

		//	Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		//}

		//[Test]
		//public void MoveCreepAlongWaypoints()
		//{
		//	var position = grid.PropertyMatrix[15, 2].MidPoint;
		//	var movement = new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.5f, 0.0f, 0.5f),
		//		StartGridPos = new Tuple<int, int>(15, 2),
		//		FinalGridPos = new Tuple<int, int>(4, 19),
		//		Waypoints =
		//			new List<Tuple<int, int>>
		//			{
		//				new Tuple<int, int>(11, 2),
		//				new Tuple<int, int>(11, 6),
		//				new Tuple<int, int>(7, 6),
		//				new Tuple<int, int>(7, 10),
		//				new Tuple<int, int>(12, 10),
		//				new Tuple<int, int>(12, 15),
		//				new Tuple<int, int>(8, 15),
		//				new Tuple<int, int>(8, 18),
		//				new Tuple<int, int>(4, 18),
		//				new Tuple<int, int>(4, 19),
		//			}
		//	};
		//	new Creep(position, CreepProp(Names.CreepCottonMummy, CreepType.Cloth), grid, movement);
		//}

		//[Test]
		//public void CreepDisappearsUponReachingDestination()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Position = creep.Grid.PropertyMatrix[15, 2].MidPoint;
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.5f, 0.5f, 0.0f),
		//		StartGridPos = new Tuple<int, int>(15, 2),
		//		FinalGridPos = new Tuple<int, int>(11, 2),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
		//	});

		//	Assert.AreEqual(creep.Grid.PropertyMatrix[15, 2].MidPoint, creep.Position);
		//	AdvanceTimeAndUpdateEntities(3.0f);
		//	Assert.AreEqual(creep.Grid.PropertyMatrix[11, 2].MidPoint, creep.Position);
		//	Assert.IsFalse(creep.IsActive);

		//}

		//[Test]
		//public void CreepDisappearsWhenDead()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Position = grid.PropertyMatrix[15, 2].MidPoint;
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.5f, 0.0f, 0.5f),
		//		StartGridPos = new Tuple<int, int>(15, 2),
		//		FinalGridPos = new Tuple<int, int>(11, 2),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
		//	});
		//	creep.ReceiveAttack(TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
		//	Assert.LessOrEqual(creep.Get<CreepProperties>().CurrentHp, 0.0f);
		//	Assert.IsFalse(creep.IsActive);
		//	Assert.IsFalse(creep.HealthBar.IsActive);
		//}

		//[Test]
		//public void DisplayCreepDyingEffect()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.ReceiveAttack(TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
		//}

		//[Test]
		//public void CheckForDamageWhenStateIsBurst()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.state.Burst = true;
		//	creep.UpdateStateTimersAndTimeBasedDamage();
		//	AdvanceTimeAndUpdateEntities(2.0f);
		//	Assert.Less(creep.Get<CreepProperties>().CurrentHp, creep.Get<CreepProperties>().MaxHp);
		//}

		//[Test]
		//public void CheckForDamageWhenStateIsBurn()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.state.Burn = true;
		//	creep.UpdateStateTimersAndTimeBasedDamage();
		//	AdvanceTimeAndUpdateEntities(2.0f);
		//	Assert.Less(creep.Get<CreepProperties>().CurrentHp, creep.Get<CreepProperties>().MaxHp);
		//}

		//[Test]
		//public void CheckWhenShatteredCreepIsCloseToAnotherCreep()
		//{
		//	var glassCreep = new Creep(Vector3D.Zero, CreepProp(Names.CreepGlass, CreepType.Glass),
		//		grid, movementData);
		//	var clothCreep = CreateDefaultCreep();
		//	clothCreep.Position = new Vector3D(1.0f, 1.0f, 0.0f);
		//	glassCreep.Shatter();
		//	Assert.Less(clothCreep.Get<CreepProperties>().CurrentHp,
		//		clothCreep.Get<CreepProperties>().MaxHp);
		//}

		//[Test]
		//public void CheckWhenShatteredCreepIsFarFromOtherCreeps()
		//{
		//	var glassCreep = new Creep(Vector3D.Zero, CreepProp(Names.CreepGlass, CreepType.Glass),
		//		grid, movementData);
		//	var clothCreep = CreateDefaultCreep();
		//	clothCreep.Position = new Vector3D(3.0f, 3.0f, 0.0f);
		//	glassCreep.Shatter();
		//	Assert.AreEqual(clothCreep.Get<CreepProperties>().CurrentHp,
		//		clothCreep.Get<CreepProperties>().MaxHp);
		//}

		//[Test]
		//public void DisplayCreepHealthBar()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.HealthBar.Value = 20.0f;
		//}

		//[Test]
		//public void HealthBarMovesAlongWithCreep()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.Remove<MovementInGrid.MovementData>();
		//	creep.Add(new MovementInGrid.MovementData
		//	{
		//		Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
		//		StartGridPos = new Tuple<int, int>(4, 0),
		//		Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
		//	});
		//}

		//[Test]
		//public void InactiveCreepsCantBeAttacked()
		//{
		//	var creep = CreateDefaultCreep();
		//	creep.IsActive = false;
		//	var creepHpBeforeAttack = creep.Get<CreepProperties>().CurrentHp;
		//	creep.ReceiveAttack(TowerType.Acid, 10.0f);
		//	var creepHpAfterAttack = creep.Get<CreepProperties>().CurrentHp;
		//	Assert.AreEqual(creepHpAfterAttack, creepHpBeforeAttack);
		//}
	}
}