using CreepyTowers.Towers;

namespace CreepyTowers.Tests.Simple2D
{
	public class PaperCreepStateChanger2D
	{
		public static void ChangeStatesIfPaperCreep(TowerType damageType, Creep2D creep)
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

		private static void SetAffectedByFire(Creep2D creep)
		{
			if (creep.state.Wet)
			{
				creep.state.Wet = false;
				ChangeStartStatesIfPaperCreep(creep);
			}
			else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				SetPaperCreepWetState(creep);
			}
			else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfPaperCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Impact);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Water);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Slice);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Acid);
		}

		private static void SetPaperCreepWetState(Creep2D creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Slice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Impact);
			StateChanger.MakeCreepNormalToType(creep, TowerType.Ice);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepDelayed(creep);
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			SetPaperCreepWetState(creep);
		}

		private static void SetAffectedByIce(Creep2D creep)
		{
			if (creep.state.Wet)
				StateChanger.MakeCreepFrozen(creep);
			creep.state.Burst = false;
			creep.state.Burn = false;
			creep.state.Fast = false;
		}
	}
}