namespace $safeprojectname$.Simple2D
{
	public class SandCreepStateChanger2D
	{
		public static void ChangeStatesIfSandCreep(Tower.TowerType damageType, Creep2D creep, 
			Basic2DDisplaySystem basic)
		{
			if (damageType == Tower.TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == Tower.TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == Tower.TowerType.Ice)
				SetAffectedByIce(creep);
			else if (damageType == Tower.TowerType.Fire)
				SetAffectedByFire(creep, basic);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepSlow(creep);
			if (!creep.state.Frozen)
				return;

			StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep2D creep)
		{
			StateChanger.MakeCreepWet(creep);
		}

		private static void SetAffectedByIce(Creep2D creep)
		{
			if (creep.state.Wet)
				StateChanger.MakeCreepFrozen(creep);
		}

		private static void SetAffectedByFire(Creep2D creep, Basic2DDisplaySystem basic)
		{
			if (creep.state.Wet)
				creep.state.Wet = false;
			else if (creep.state.Frozen)
				StateChanger.MakeCreepWet(creep);
			else
			{
				basic.AddCreep(creep.listOfNodes [0], creep.Target, Creep.CreepType.Glass);
				creep.hitpointBar.IsActive = false;
				creep.IsActive = false;
			}
		}

		public static void ChangeStartStatesIfSandCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, Tower.TowerType.Acid);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Blade);
			StateChanger.MakeCreepResistantToType(creep, Tower.TowerType.Impact);
		}
	}
}