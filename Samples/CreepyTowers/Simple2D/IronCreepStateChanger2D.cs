using CreepyTowers.Towers;

namespace CreepyTowers.Simple2D
{
	public class IronCreepStateChanger2D
	{
		public static void ChangeStatesIfIronCreep(Tower.TowerType damageType, Creep2D creep)
		{
			if (damageType == Tower.TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == Tower.TowerType.Acid)
				SetAffectedByAcid(creep);
			else if (damageType == Tower.TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == Tower.TowerType.Ice)
				SetAffectedByIce(creep);
		}

		private static void SetAffectedByFire(Creep2D creep)
		{
			StateChanger.MakeCreepMelt(creep);
			StateChanger.MakeCreepNormalToType(creep, Tower.TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Impact);
		}

		private static void SetAffectedByAcid(Creep2D creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			StateChanger.MakeCreepRust(creep);
			if (!creep.state.Melt)
				return;
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Water);
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Impact);
		}

		private static void SetAffectedByIce(Creep2D creep)
		{
			if (!creep.state.Melt)
				return;
			StateChanger.CheckChanceForSudden(creep);
		}

		public static void ChangeStartStatesIfIronCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Impact);
			StateChanger.MakeCreepHardBoiledToType(creep, Tower.TowerType.Slice);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Water);
		}
	}
}
