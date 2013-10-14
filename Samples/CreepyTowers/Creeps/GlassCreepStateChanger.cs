using CreepyTowers.Towers;

namespace CreepyTowers.Creeps
{
	public class GlassCreepStateChanger
	{
		public static void ChangeStatesIfGlassCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			StateChanger.CheckChanceForShatter(creep);
		}

		public static void ChangeStartStatesIfGlassCreep(Creep creep)
		{
			StateChanger.MakeCreepWeakToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Acid);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Water);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Impact);
		}
	}
}