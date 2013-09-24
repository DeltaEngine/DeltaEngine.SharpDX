using CreepyTowers.Towers;

namespace CreepyTowers.Simple2D
{
	public class WoodCreepStateChanger2D
	{
		public static void ChangeStatesIfWoodCreep(Tower.TowerType damageType, Creep2D creep)
		{
			if (damageType == Tower.TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == Tower.TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == Tower.TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == Tower.TowerType.Ice)
				SetAffectedByIce(creep);
		}

		private static void SetAffectedByFire(Creep2D creep)
		{
			if (creep.state.Wet)
			{
				creep.state.Wet = false;
				ChangeStartStatesIfWoodCreep(creep);
			}
			else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				SetWoodCreepWetState(creep);
			}
			else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfWoodCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Blade);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Fire);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Acid);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Water);
		}

		private static void SetWoodCreepWetState(Creep2D creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Blade);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Ice);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			SetWoodCreepWetState(creep);
			StateChanger.MakeCreepHealing(creep);
		}

		private static void SetAffectedByIce(Creep2D creep)
		{
			if (creep.state.Wet)
				StateChanger.MakeCreepFrozen(creep);
			creep.state.Burst = false;
			creep.state.Burn = false;
		}
	}
}
