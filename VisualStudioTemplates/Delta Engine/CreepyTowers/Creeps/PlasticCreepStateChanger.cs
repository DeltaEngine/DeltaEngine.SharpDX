using $safeprojectname$.Towers;

namespace $safeprojectname$.Creeps
{
	public class PlasticCreepStateChanger
	{
		public static void ChangeStatesIfPlasticCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == TowerType.Acid)
				SetAffectedByAcid(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			StateChanger.MakeCreepBurn(creep);
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			StateChanger.MakeCreepSlow(creep);
			StateChanger.MakeCreepEnfeeble(creep);
		}

		private static void SetAffectedByAcid(Creep creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			if (!creep.state.Burn)
				return;

			ComeBackToNormal(creep);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Water);
		}

		private static void ComeBackToNormal(Creep creep)
		{
			creep.state.Burn = false;
			creep.state.Melt = false;
			creep.state.Enfeeble = false;
			creep.state.Slow = false;
		}

		private static void SetAffectedByIce(Creep creep)
		{
			if (!creep.state.Burn)
				return;

			ComeBackToNormal(creep);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Ice);
		}

		public static void ChangeStartStatesIfPlasticCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepWeakToType(creep, TowerType.Impact);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Fire);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Water);
		}
	}
}