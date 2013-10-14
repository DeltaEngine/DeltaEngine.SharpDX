using CreepyTowers.Towers;

namespace CreepyTowers.Tests.Simple2D
{
	public class GlassCreepStateChanger2D
	{
		public static void ChangeStatesIfGlassCreep(TowerType damageType, Creep2D creep)
		{
			if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
			else if (damageType == TowerType.Impact)
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
			StateChanger.MakeCreepWeakToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Acid);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Water);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Fire);
			StateChanger.MakeCreepVulnerableToType(creep, TowerType.Impact);
		}
	}
}