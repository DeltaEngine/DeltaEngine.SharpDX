using CreepyTowers.Towers;

namespace CreepyTowers.Creeps
{
	public class IronCreepStateChanger
	{
		public static void ChangeStatesIfIronCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == TowerType.Acid)
				SetAffectedByAcid(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			StateChanger.MakeCreepMelt(creep);
			StateChanger.MakeCreepNormalToType(creep, TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Impact);
		}

		private static void SetAffectedByAcid(Creep creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			StateChanger.MakeCreepRust(creep);
			if (!creep.state.Melt)
				return;
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Water);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
		}

		private static void SetAffectedByIce(Creep creep)
		{
			if (!creep.state.Melt)
				return;
			StateChanger.CheckChanceForSudden(creep);
		}

		public static void ChangeStartStatesIfIronCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Slice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Water);
		}
	}
}
