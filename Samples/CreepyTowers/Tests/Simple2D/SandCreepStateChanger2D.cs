using CreepyTowers.Creeps;
using CreepyTowers.Towers;

namespace CreepyTowers.Tests.Simple2D
{
	public class SandCreepStateChanger2D
	{
		public static void ChangeStatesIfSandCreep(TowerType damageType, Creep2D creep, 
			Basic2DDisplaySystem basic)
		{
			if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
			else if (damageType == TowerType.Fire)
				SetAffectedByFire(creep, basic);
		}

		private static void SetAffectedByImpact(Creep2D creep)
		{
			StateChanger.MakeCreepLimitedSlow(creep);
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
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				StateChanger.MakeCreepWet(creep);
			}
			else
				TransformInGlassCreep(creep, basic);
		}

		private static void TransformInGlassCreep(Creep2D creep, Basic2DDisplaySystem basic)
		{
			var percentage = creep.CurrentHp / creep.data.MaxHp;
			var newCreep = basic.AddCreep(creep.listOfNodes[0].GetVector2D(), 
				creep.Target.GetVector2D(), CreepType.Glass);
			newCreep.CurrentHp *= percentage;
			basic.Creeps.Remove(creep);
			creep.hitpointBar.IsActive = false;
			creep.IsActive = false;
		}

		public static void ChangeStartStatesIfSandCreep(Creep2D creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Acid);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
		}
	}
}