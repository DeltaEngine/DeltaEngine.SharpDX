namespace $safeprojectname$.Simple2D
{
	public class PaperCreepStateChanger2D
	{
		public static void ChangeStatesIfPaperCreep(Tower.TowerType damageType, Creep2D creep)
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
				ChangeStartStatesIfPaperCreep(creep);
			} else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				SetPaperCreepWetState(creep);
			} else
			{
				StateChanger.MakeCreepFast(creep);
				StateChanger.MakeCreepBurst(creep);
			}
		}

		public static void ChangeStartStatesIfPaperCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Water);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Blade);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Acid);
		}

		private static void SetPaperCreepWetState(Creep2D creep)
		{
			StateChanger.MakeCreepWet(creep);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Blade);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepNormalToType(creep, Tower.TowerType.Ice);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepParalysed(creep);
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
		}
	}
}