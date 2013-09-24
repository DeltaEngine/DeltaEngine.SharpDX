using $safeprojectname$.Towers;
using DeltaEngine.Core;

namespace $safeprojectname$.Creeps
{
	public class ClothCreepStateChanger
	{
		public static void ChangeStatesIfClothCreep(Tower.TowerType damageType, Creep creep, 
			CreepProperties properties)
		{
			if (properties.CreepType != Creep.CreepType.Cloth)
				return;

			if ((damageType == Tower.TowerType.Ice || damageType == Tower.TowerType.Impact || 
				damageType == Tower.TowerType.Water))
			{
				creep.state.Slow = true;
				creep.state.SlowTimer = 0;
			}
			if (damageType == Tower.TowerType.Acid)
			{
				creep.state.Enfeeble = true;
				creep.state.EnfeebleTimer = 0;
			}
			if (damageType == Tower.TowerType.Fire)
				SetClothCreepStatesWhenAttackedByFire(creep);

			if (damageType == Tower.TowerType.Water)
			{
				if (creep.state.Frozen)
					return;

				SetClothCreepWetState(creep);
			}
			if (damageType == Tower.TowerType.Ice)
			{
				if (creep.state.Wet)
					SetClothCreepFrozenState(creep);

				creep.state.Wet = false;
				creep.state.Burst = false;
				creep.state.Burn = false;
			}
			if (damageType == Tower.TowerType.Impact)
				if (creep.state.Frozen)
			{
				var chanceForShather = Randomizer.Current.Get(0, 100);
				if (chanceForShather < 10)
				{
					creep.IsActive = false;
					creep.Shatter();
				}
			}
		}

		private static void SetClothCreepStatesWhenAttackedByFire(Creep creep)
		{
			if (creep.state.Wet)
			{
				creep.state.Wet = false;
				ChangeStartStatesIfClothCreep(Creep.CreepType.Cloth, creep);
			} else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				SetClothCreepWetState(creep);
			} else
			{
				creep.state.Burst = true;
				creep.state.BurstTimer = 0;
			}
		}

		private static void SetClothCreepWetState(Creep creep)
		{
			creep.state.Burst = false;
			creep.state.Burn = false;
			creep.state.WetTimer = 0;
			creep.state.Wet = true;
			creep.state.SetVulnerability(Tower.TowerType.Fire, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Impact, CreepState.VulnerabilityType.HardBoiled);
			creep.state.SetVulnerability(Tower.TowerType.Ice, CreepState.VulnerabilityType.Weak);
		}

		private static void SetClothCreepFrozenState(Creep creep)
		{
			creep.state.Frozen = true;
			creep.state.FrozenTimer = 0;
			creep.state.Paralysed = true;
			creep.state.SetVulnerability(Tower.TowerType.Blade, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Water, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Impact, CreepState.VulnerabilityType.Vulnerable);
			creep.state.SetVulnerability(Tower.TowerType.Fire, CreepState.VulnerabilityType.Immune);
		}

		public static void ChangeStartStatesIfClothCreep(Creep.CreepType creepType, Creep creep)
		{
			if (creepType != Creep.CreepType.Cloth)
				return;

			creep.state.SetVulnerabilitiesToNormal();
			creep.state.SetVulnerability(Tower.TowerType.Ice, CreepState.VulnerabilityType.HardBoiled);
			creep.state.SetVulnerability(Tower.TowerType.Blade, CreepState.VulnerabilityType.Weak);
			creep.state.SetVulnerability(Tower.TowerType.Impact, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Acid, CreepState.VulnerabilityType.Vulnerable);
			creep.state.SetVulnerability(Tower.TowerType.Fire, CreepState.VulnerabilityType.Vulnerable);
		}
	}
}