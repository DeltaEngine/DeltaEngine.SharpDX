using System;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.Rendering3D.Cameras;

namespace CreepyTowers
{
	public class Manager : Entity, IDisposable
	{
		public Manager(float cameraFovSizeFactor)
		{
			Game.CameraAndGrid.FovSizeFactor = cameraFovSizeFactor;
			towerPropXmlParser = new TowerPropertiesXmlParser();
			creepPropXmlParser = new CreepPropertiesXmlParser();
			Start<FindPossibleTargets>();
			UpdatePriority = Priority.High;
		}

		private readonly TowerPropertiesXmlParser towerPropXmlParser;
		private readonly CreepPropertiesXmlParser creepPropXmlParser;

		public struct PlayerData
		{
			public int ResourceFinances;
		}

		public Level Level { get; set; }

		public Creep CreateCreep(Vector3D position, string name, Creep.CreepType creepType, 
			MovementInGrid.MovementData movementData)
		{
			var creep = new Creep(position, name, creepPropXmlParser.CreepPropertiesData[creepType])
			{
				Orientation = Quaternion.FromAxisAngle(Vector3D.UnitY, 180.0f)
			};
			creep.Add(movementData);
			creep.UpdateHealthBar += () => UpdateCreepHealthBar(creep);

			//creep.CreepIsDead += creep.Dispose;
			return creep;
		}

		public void UpdateCreepHealthBar(Creep creep)
		{
			if (creep.HealthBar == null)
				return;

			var healthBar3DPos = new Vector3D(creep.Position.X, creep.Position.Y + 0.25f,
				creep.Position.Z);
			var healthBar2DPos = Camera.Current.WorldToScreenPoint(healthBar3DPos);
			var creepProperties = creep.Get<CreepProperties>();
			var healthPercent = (creepProperties.CurrentHp / creepProperties.MaxHp) * 100;
			var drawArea = Rectangle.FromCenter(healthBar2DPos, creep.HealthBarSize);

			if (healthPercent > 80)
				creep.HealthBar =
					new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarGreen100), drawArea);
			else if (healthPercent > 60)
				creep.HealthBar = new Sprite(
					new Material(Shader.Position2DUv, Names.ImageHealthBarGreen80), drawArea);
			else if (healthPercent > 50)
				creep.HealthBar = new Sprite(
					new Material(Shader.Position2DUv, Names.ImageHealthBarGreen60), drawArea);
			else if (healthPercent > 40)
				creep.HealthBar =
					new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange50), drawArea);
			else if (healthPercent > 25)
				creep.HealthBar =
					new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange40), drawArea);
			else if (healthPercent > 20)
				creep.HealthBar =
					new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange25), drawArea);
			else if (healthPercent > 10)
				creep.HealthBar = new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarRed20),
					drawArea);
			else if (healthPercent > 5)
				creep.HealthBar = new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarRed10),
					drawArea);
			else
				creep.HealthBar = new Sprite(new Material(Shader.Position2DUv, Names.ImageHealthBarRed05),
					drawArea);

			creep.HealthBar.DrawArea = drawArea;
		}

		public void CreateTower(Vector3D position, Tower.TowerType towerType, string name)
		{
			if (playerData.ResourceFinances < 100)
				MessageInsufficientMoney(100);

			playerData.ResourceFinances -= 100;
			MessageCreditsUpdated(-100);

			if (towerPropXmlParser.TowerPropertiesData.ContainsKey(towerType))
				new Tower(position, name, towerPropXmlParser.TowerPropertiesData[towerType]);
		}

		public PlayerData playerData;

		public void MessageInsufficientMoney(int amountRequired)
		{
			if (InsufficientCredits != null)
				InsufficientCredits(amountRequired);
		}

		public event Action<int> InsufficientCredits;

		public void MessageCreditsUpdated(int difference)
		{
			if (CreditsUpdated != null)
				CreditsUpdated(difference);
		}

		public event Action<int> CreditsUpdated;

		public void Dispose()
		{
			foreach (Creep creep in EntitiesRunner.Current.GetEntitiesOfType<Creep>())
				creep.Dispose();

			foreach (Tower tower in EntitiesRunner.Current.GetEntitiesOfType<Tower>())
				tower.Dispose();

			if (Contains<InputCommands>())
				Get<InputCommands>().Dispose();

			//IsActive = false;
		}
	}
}