using CreepyTowers.Towers;

namespace CreepyTowers.Tests.Simple2D
{
	public class IronCreepStateChanger2D
	{
		public static void ChangeStatesIfIronCreep(TowerType damageType, Creep2D creep)
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

		private static void SetAffectedByFire(Creep2D creep)
		{
			StateChanger.MakeCreepMelt(creep);
			StateChanger.MakeCreepNormalToType(creep, TowerType.Slice);
			StateChanger.MakeCreepWeakToType(creep, TowerType.Impact);
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
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Water);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
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
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
			StateChanger.MakeCreepHardBoiledToType(creep, TowerType.Slice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Water);
		}
	}
}