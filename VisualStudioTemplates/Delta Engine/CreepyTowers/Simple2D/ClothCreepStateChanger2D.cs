using $safeprojectname$.Towers;

namespace $safeprojectname$.Simple2D
{
	public class ClothCreepStateChanger2D
	{
		public static void ChangeStatesIfClothCreep(Tower.TowerType damageType, Creep2D creep)
		{
			if (damageType == Tower.TowerType.Ice)
				SetAffectedByIce(creep);
			else if (damageType == Tower.TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == Tower.TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == Tower.TowerType.Acid)
				SetAffectedByAcid(creep);
			else if (damageType == Tower.TowerType.Fire)
				SetAffectedByFire(creep);
		}

		private static void SetAffectedByIce(Creep2D creep)
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

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepLimitedSlow(creep);
			if (creep.state.Frozen)
				StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			if (creep.state.Fast)
				creep.state.Fast = false;

			SetClothCreepWetState(creep);
		}

		private static void SetClothCreepWetState(Creep2D creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepNormalToType(creep, Tower.TowerType.Slice);
		}

		private static void SetAffectedByAcid(Creep2D creep)
		{
			StateChanger.MakeCreepEnfeeble(creep);
		}

		private static void SetAffectedByFire(Creep2D creep)
		{
			StateChanger.MakeCreepFast(creep);
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

		public static void ChangeStartStatesIfClothCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Slice);
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Acid);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Fire);
		}
	}
}