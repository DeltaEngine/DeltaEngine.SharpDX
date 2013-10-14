using System;
using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace CreepyTowers
{
	//TODO: Remove, managers are not allowed
	public class Manager : Entity, IDisposable
	{
		public Manager(float cameraFovSizeFactor)
		{
			Game.CreateCamera.FovSizeFactor = cameraFovSizeFactor;
			Start<FindPossibleTargets>();
			UpdatePriority = Priority.High;
		}

		public struct PlayerData
		{
			public int ResourceFinances;
		}

		public Creep CreateCreep(Vector3D position, CreepType creepType,
			MovementInGrid.MovementData movementData)
		{
			var creep = new Creep(creepType, position, 0);
			creep.UpdateHealthBar += () => creep.RecalculateHitpointBar();
			return creep;
		}

		public void CreateTower(Vector3D position, TowerType towerType, string name)
		{
			//TODO: wtf is this, why is this hardcoded, towers have money amount!
			if (playerData.ResourceFinances < 100)
				MessageInsufficientMoney(100);
			playerData.ResourceFinances -= 100;
			MessageCreditsUpdated(-100);
			new Tower(towerType, position);
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
			if (Contains<InGameCommands>())
				Get<InGameCommands>().Dispose();
		}
	}
}