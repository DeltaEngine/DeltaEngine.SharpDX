using CreepyTowers.Towers;

namespace CreepyTowers.Simple2D
{
	public class GlassCreepStateChanger2D
	{
		public static void ChangeStatesIfGlassCreep(Tower.TowerType damageType, Creep2D creep)
		{
			if (damageType == Tower.TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == Tower.TowerType.Impact)
				SetAffectedByImpact(creep);
		}

		private static void SetAffectedByFire(Creep2D creep)
		{
			StateChanger.MakeCreepMelt(creep);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.CheckChanceForShatter(creep);
		}

		public static void ChangeStartStatesIfGlassCreep(Creep2D creep)
		{
			StateChanger.MakeCreepWeakToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Acid);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Water);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, Tower.TowerType.Impact);
		}
	}
}