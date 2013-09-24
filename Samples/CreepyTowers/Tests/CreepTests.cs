using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class CreepTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			game = new Game(Resolve<Window>(), Resolve<Device>());
			grid = Game.CameraAndGrid.Grid;
		}

		private LevelGrid grid;
		private Game game;

		[Test]
		public void CreateClothCreep()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid>();
		}

		private Creep CreateDefaultCreep()
		{
			var position = grid.PropertyMatrix[2, 3].MidPoint;
			var creep = new Creep(position, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			AddDefaultMovementComponent(creep);
			return creep;
		}

		private static void AddDefaultMovementComponent(Creep creep)
		{
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
		}

		[Test]
		public void CreateIronCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Iron, Names.CreepIronTyrannosaurusAxe);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void CreateGlassCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void CreateSandCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Sand, Names.CreepsSandSandy);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void CreatePaperCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Paper, Names.CreepPaperPaperplane);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void CreateWoodCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Wood, Names.CreepsWoodScarecrow);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void CreatePlasticCreep()
		{
			var creep = new Creep(Vector3D.Zero, Creep.CreepType.Plastic, Names.CreepPlasticBottledog);
			AddDefaultMovementComponent(creep);
			creep.Remove<MovementInGrid>();
		}

		[Test]
		public void DisposingCreepRemovesCreepEntity()
		{
			var creep = CreateDefaultCreep();
			creep.Dispose();
			Assert.IsFalse(creep.IsActive);
		}

		[Test]
		public void DisplayCreepWalkingAlongXAxisForward()
		{
			var creep = CreateDefaultCreep();
			creep.Position = grid.PropertyMatrix[6, 0].MidPoint;
			creep.Remove<MovementInGrid.MovementData>();

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(1.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(0, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(14, 0) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[14, 0].MidPoint.X, creep.Position.X);
		}

		[Test]
		public void DisplayCreepWalkingAlongXAxisBackward()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[4, 0].MidPoint;
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(1.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
		}

		[Test]
		public void DisplayCreepWalkingAlongZAxisForward()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[0, 10].MidPoint;
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(0, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 16) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		}

		[Test]
		public void DisplayCreepWalkingAlongZAxisBackwards()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[0, 8].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 1.0f),
				StartGridPos = new Tuple<int, int>(0, 10),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 4) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		}

		[Test]
		public void MoveCreepAlongWaypoints()
		{
			var position = grid.PropertyMatrix[15, 2].MidPoint;
			var creep = new Creep(position, Creep.CreepType.Cloth, Names.CreepCottonMummy);

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(4, 19),
				Waypoints =
					new List<Tuple<int, int>>
					{
						new Tuple<int, int>(11, 2),
						new Tuple<int, int>(11, 6),
						new Tuple<int, int>(7, 6),
						new Tuple<int, int>(7, 10),
						new Tuple<int, int>(12, 10),
						new Tuple<int, int>(12, 15),
						new Tuple<int, int>(8, 15),
						new Tuple<int, int>(8, 18),
						new Tuple<int, int>(4, 18),
						new Tuple<int, int>(4, 19),
					}
			});
		}

		[Test]
		public void CreepDisappearsUponReachingDestination()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[15, 2].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(11, 2),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
			});

			Assert.AreEqual(grid.PropertyMatrix[15, 2].MidPoint, creep.Position);
			AdvanceTimeAndUpdateEntities(3.0f);
			var data = creep.Get<MovementInGrid.MovementData>();
			Assert.AreEqual(
				grid.PropertyMatrix[data.FinalGridPos.Item1, data.FinalGridPos.Item2].MidPoint,
				creep.Position);
			Assert.IsFalse(creep.IsActive);
		}

		[Test]
		public void CreepDisappearsWhenDead()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[15, 2].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(11, 2),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
			});

			creep.ReceiveAttack(Tower.TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
			Assert.LessOrEqual(creep.Get<CreepProperties>().CurrentHp, 0.0f);
			Assert.IsFalse(creep.IsActive);
			Assert.IsFalse(creep.HealthBar.IsActive);
		}

		[Test]
		public void DisplayCreepDyingEffect()
		{
			var creep = CreateDefaultCreep();
			//creep.ReceiveAttack(Tower.TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
			var creep2DPoint = Camera.Current.WorldToScreenPoint(creep.Position);
			var drawArea = Rectangle.FromCenter(creep2DPoint, new Size(0.07f));
			//new SpriteSheetAnimation("SpriteDyingCloud", drawArea);
		}

		[Test]
		public void CheckForDamageWhenStateIsBurst()
		{
			var creep = CreateDefaultCreep();
			creep.Add(new CreepProperties
			{
				MaxHp = 100.0f,
				CurrentHp = 100.0f,
				Resistance = 1.0f,
				CreepType = Creep.CreepType.Cloth,
				GoldReward = 20,
				Speed = 2.0f,
				Name = Names.CreepCottonMummy,
			});
			creep.state.Burst = true;
			creep.UpdateStateTimersAndTimeBasedDamage();
			AdvanceTimeAndUpdateEntities(2.0f);
			Assert.Less(creep.Get<CreepProperties>().CurrentHp, creep.Get<CreepProperties>().MaxHp);
		}

		[Test]
		public void CheckWhenShatteredCreepIsCloseToAnotherCreep()
		{
			var glassCreep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
			var clothCreep = new Creep(new Vector3D(1.0f, 1.0f, 0.0f), Creep.CreepType.Cloth,
				Names.CreepCottonMummy);
			clothCreep.Add(new CreepProperties
			{
				MaxHp = 100.0f,
				CurrentHp = 100.0f,
				Resistance = 1.0f,
				CreepType = Creep.CreepType.Cloth,
				GoldReward = 20,
				Speed = 2.0f,
				Name = Names.CreepCottonMummy,
			});
			AddDefaultMovementComponent(glassCreep);
			AddDefaultMovementComponent(clothCreep);
			glassCreep.Shatter();
			Assert.Less(clothCreep.Get<CreepProperties>().CurrentHp, clothCreep.Get<CreepProperties>().MaxHp);
		}

		[Test]
		public void CheckWhenShatteredCreepIsFarFromOtherCreeps()
		{
			var glassCreep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
			var clothCreep = new Creep(new Vector3D(3.0f, 3.0f, 0.0f), Creep.CreepType.Cloth,
				Names.CreepCottonMummy);
			clothCreep.Add(new CreepProperties
			{
				MaxHp = 100.0f,
				CurrentHp = 100.0f,
				Resistance = 1.0f,
				CreepType = Creep.CreepType.Cloth,
				GoldReward = 20,
				Speed = 2.0f,
				Name = Names.CreepCottonMummy,
			});
			AddDefaultMovementComponent(glassCreep);
			AddDefaultMovementComponent(clothCreep);
			glassCreep.Shatter();
			Assert.AreEqual(clothCreep.Get<CreepProperties>().CurrentHp, clothCreep.Get<CreepProperties>().MaxHp);
		}
	}
}