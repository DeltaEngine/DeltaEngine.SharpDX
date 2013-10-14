using $safeprojectname$.Towers;

namespace $safeprojectname$.Creeps
{
	public class ClothCreepStateChanger
	{
		public static void ChangeStatesIfClothCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
			else if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Acid)
				SetAffectedByAcid(creep);
			else if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
		}

		private static void SetAffectedByIce(Creep creep)
		{
			if (creep.state.Burst)
			{
				creep.state.Fast = false;
				creep.state.Burst = false;
			} else
			{
				StateChanger.MakeCreepSlow(creep);
				if (creep.state.Wet)
					StateChanger.MakeCreepFrozen(creep);

				creep.state.Burst = false;
				creep.state.Burn = false;
				creep.state.Fast = false;
			}
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			StateChanger.MakeCreepLimitedSlow(creep);
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			if (creep.state.Fast)
				creep.state.Fast = false;

			SetClothCreepWetState(creep);
		}

		private static void SetClothCreepWetState(Creep creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Ice);
			StateChanger.MakeCreepNormalToType(creep, TowerType.Slice);
		}

		private static void SetAffectedByAcid(Creep creep)
		{
			StateChanger.MakeCreepEnfeeble(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			if (creep.state.Wet)
			{
				creep.state.Wet = false;
				ChangeStartStatesIfClothCreep(creep);
			} else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				SetClothCreepWetState(creep);
			} else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfClothCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Ice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Slice);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Impact);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Acid);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Fire);
		}
	}
}