using $safeprojectname$.Towers;
using DeltaEngine.Core;

namespace $safeprojectname$.Creeps
{
	public class SandCreepStateChanger
	{
		public static void ChangeStatesIfSandCreep(Tower.TowerType damageType, Creep creep, 
			CreepProperties properties)
		{
			if (properties.CreepType != Creep.CreepType.Sand)
				return;

			if (damageType == Tower.TowerType.Impact || damageType == Tower.TowerType.Water)
			{
				creep.state.Slow = true;
				creep.state.SlowTimer = 0;
			}
			if (damageType == Tower.TowerType.Water)
				creep.state.Wet = true;

			if (damageType == Tower.TowerType.Ice)
				if (creep.state.Wet)
			{
				creep.state.Frozen = true;
				creep.state.Wet = false;
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
			if (damageType == Tower.TowerType.Fire)
			{
				if (creep.state.Wet)
				{
					creep.state.Wet = false;
					return;
				}
				creep.IsActive = false;
				new Creep(creep.Position, Creep.CreepType.Glass, Names.CreepCottonMummy);
			}
		}

		public static void ChangeStartStatesIfSandCreep(Creep.CreepType creepType, Creep creep)
		{
			if (creepType != Creep.CreepType.Sand)
				return;

			creep.state.SetVulnerabilitiesToNormal();
			creep.state.SetVulnerability(Tower.TowerType.Water, CreepState.VulnerabilityType.Weak);
			creep.state.SetVulnerability(Tower.TowerType.Impact, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Ice, CreepState.VulnerabilityType.Immune);
			creep.state.SetVulnerability(Tower.TowerType.Acid, CreepState.VulnerabilityType.Immune);
			creep.state.SetVulnerability(Tower.TowerType.Blade, CreepState.VulnerabilityType.HardBoiled);
		}
	}
}