using $safeprojectname$.Towers;

namespace $safeprojectname$.Creeps
{
	public class WoodCreepStateChanger
	{
		public static void ChangeStatesIfWoodCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			if (creep.state.Wet)
			{
				creep.state.Wet = false;
				ChangeStartStatesIfWoodCreep(creep);
			} else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				SetWoodCreepWetState(creep);
			} else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfWoodCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Fire);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Acid);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Water);
		}

		private static void SetWoodCreepWetState(Creep creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Ice);
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			SetWoodCreepWetState(creep);
			StateChanger.MakeCreepHealing(creep);
		}

		private static void SetAffectedByIce(Creep creep)
		{
			if (creep.state.Wet)
				StateChanger.MakeCreepFrozen(creep);

			creep.state.Burst = false;
			creep.state.Burn = false;
			creep.state.Fast = false;
		}
	}
}