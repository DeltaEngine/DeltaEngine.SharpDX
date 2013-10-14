using System;
using CreepyTowers.Content;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Particles;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace CreepyTowers.Tests.Towers
{
	public class TowerTests : TestWithMocksOrVisually
	{
		[TestCase(TowerType.Water),
		TestCase(TowerType.Fire),
		TestCase(TowerType.Slice),
		TestCase(TowerType.Impact),
		TestCase(TowerType.Acid),
		TestCase(TowerType.Ice)]
		public void RenderTower(TowerType type)
		{
			new Tower(type, Vector3D.Zero);
		}

		[Test]
		public void CreateTowerAtClickedPosition()
		{
			var floor = new Plane(Vector3D.UnitY, 0.0f);
			var list = new ChangeableList<Tuple<int, int>>
			{
				new Tuple<int, int>(1, 1),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3)
			};
			var grid = new LevelGrid(24, 0.2f);
			new Command(pos =>
			{
				var ray = Camera.Current.ScreenPointToRay(ScreenSpace.Current.ToPixelSpace(pos));
				var position = floor.Intersect(ray).Value;
				position = grid.ComputeGridCoordinates(grid, position, list);
				new Tower(TowerType.Water, position);
			}).Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing));
		}

		[Test]
		public void DisposingTowerRemovesTowerEntity()
		{
			var tower = new Tower(TowerType.Water, Vector3D.Zero);
			tower.Dispose();
			Assert.IsFalse(tower.IsActive);
		}

		[Test]
		public void CheckTowerFireParticle()
		{
			var window = Resolve<Window>();
			window.BackgroundColor = Color.White;
			var tower = new Tower(TowerType.Water, Vector3D.Zero);
			var grid = new LevelGrid(24, 0.2f);
			var creep = new Creep(CreepType.Cloth, grid.PropertyMatrix[2, 8].MidPoint, 0);
			var particleEmitterData = CreateParticleEmitterData();
			var direction = creep.Position - tower.Position;
			var emitter = new Particle3DPointEmitter(particleEmitterData, tower.Position);
			new Command(() =>
			{
				particleEmitterData.StartVelocity.Start = Vector3D.Normalize(direction);
				particleEmitterData.StartVelocity.End = Vector3D.Zero;
				var angle = MathExtensions.Atan2(direction.Y, direction.X);
				particleEmitterData.StartRotation =
					new RangeGraph<ValueRange>(new ValueRange(-angle, -angle), new ValueRange(-angle, -angle));
				emitter.Spawn();
			}).Add(new KeyTrigger(Key.Space));
		}

		private static ParticleEmitterData CreateParticleEmitterData()
		{
			const int MaxParticles = 512;
			const float LifeTime = 2.0f;
			return new ParticleEmitterData
			{
				MaximumNumberOfParticles = MaxParticles,
				SpawnInterval = 0.0f,
				LifeTime = LifeTime,
				Size = new RangeGraph<Size>(Size.Half, Size.Half),
				Color = new RangeGraph<Color>(Color.White, Color.White),
				Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero),
				StartVelocity = new RangeGraph<Vector3D>(new Vector3D(-0.5f, -0.5f, 0.1f),
					new Vector3D(0.5f, 0.5f, 0.1f)),
				ParticleMaterial = new Material(Shader.Position3DColorUV,
					Effects.EffectsWaterRangedTrans.ToString()),
				BillboardMode = BillboardMode.Ground
			};
		}

		[Test]
		public void ShowTowerFiringAtCreepAtRegularIntervals()
		{
			//manager.Start<FindPossibleTargets>();
			var window = Resolve<Window>();
			window.BackgroundColor = Color.White;
			var tower = new Tower(TowerType.Water, Vector3D.Zero);
			var grid = new LevelGrid(24, 0.2f);
			var creep = new Creep(CreepType.Cloth, grid.PropertyMatrix[2, 8].MidPoint, 0);
			tower.FireAtCreep(creep);
		}

		//[Test]
		//public void TowerCannotAttackAgainBeforeTimeHasPassed()
		//{
		//	tower = new Tower(new Vector2D(0.3f, 0.4f), TowerType.Water);
		//	creep = new Creep(Vector2D.Half, Type.Sand);
		//	int numberOfAttacksRecieved = 0;
		//	creep.GotAttacked += () => { numberOfAttacksRecieved++; };
		//	AdvanceTimeAndUpdateEntities(1 / tower.Get<Tower.Properties>().AttackFrequency);
		//	tower.FireAtCreep(creep);
		//	tower.FireAtCreep(creep);
		//	AdvanceTimeAndUpdateEntities(1 / tower.Get<Tower.Properties>().AttackFrequency);
		//	tower.FireAtCreep(creep);
		//	Assert.AreEqual(2, numberOfAttacksRecieved);
		//}

		//[Test]
		//public void CheckForCreepUnderAttack()
		//{
		//	var tower = CreateDefaultTower();
		//	var creep = CreateDefaultCreep();
		//	var creepHpBeforeHit = creep.Get<CreepProperties>().CurrentHp;
		//	AdvanceTimeAndUpdateEntities(1.5f);
		//	tower.FireAtCreep(creep);
		//	var creepHpAfterHit = creep.Get<CreepProperties>().CurrentHp;
		//	Assert.Less(creepHpAfterHit, creepHpBeforeHit);
		//}

		//[Test]
		//public void CheckForCreepDead()
		//{
		//	var tower = CreateDefaultTower();
		//	var creep = CreateDefaultCreep();
		//	creep.Get<CreepProperties>().CurrentHp = 2.0f;
		//	AdvanceTimeAndUpdateEntities(1.5f);
		//	tower.FireAtCreep(creep);
		//	Assert.IsFalse(creep.IsActive);
		//}

		//[Test]
		//public void CheckIfCreepIsHit()
		//{
		//	var manager = new Manager(Resolve<ScreenSpace>());
		//	CreateTower(manager);
		//	CreateCreep(manager);
		//	CreateFireTower(manager);
		//	AddSingleCreepAndTowerToManager(manager);
		//	AdvanceTimeAndUpdateEntities(1);
		//}
	}
}