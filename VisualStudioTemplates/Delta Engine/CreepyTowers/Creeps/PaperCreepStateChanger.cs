using $safeprojectname$.Towers;

namespace $safeprojectname$.Creeps
{
	public class PaperCreepStateChanger
	{
		public static void ChangeStatesIfPaperCreep(TowerType damageType, Creep creep)
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
				ChangeStartStatesIfPaperCreep(creep);
			} else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				SetPaperCreepWetState(creep);
			} else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfPaperCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Impact);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Water);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Slice);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Acid);
		}

		private static void SetPaperCreepWetState(Creep creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Slice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Impact);
			StateChanger.MakeCreepNormalToType(creep, TowerType.Ice);
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			StateChanger.MakeCreepDelayed(creep);
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			SetPaperCreepWetState(creep);
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