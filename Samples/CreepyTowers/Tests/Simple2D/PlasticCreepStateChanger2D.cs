using CreepyTowers.Towers;

namespace CreepyTowers.Tests.Simple2D
{
	public class PlasticCreepStateChanger2D
	{
		public static void ChangeStatesIfPlasticCreep(TowerType damageType, Creep2D creep)
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

		private static void SetAffectedByFire(Creep2D creep)
		{
			StateChanger.MakeCreepBurn(creep);
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepSlow(creep);
			StateChanger.MakeCreepEnfeeble(creep);
		}

		private static void SetAffectedByAcid(Creep2D creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			if (!creep.state.Burn)
				return;
			ComeBackToNormal(creep);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Water);
		}

		private static void ComeBackToNormal(Creep2D creep)
		{
			creep.state.Burn = false;
			creep.state.Melt = false;
			creep.state.Enfeeble = false;
			creep.state.Slow = false;
		}

		private static void SetAffectedByIce(Creep2D creep)
		{
			if (!creep.state.Burn)
				return;
			ComeBackToNormal(creep);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Ice);
		}

		public static void ChangeStartStatesIfPlasticCreep(Creep2D creep)
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