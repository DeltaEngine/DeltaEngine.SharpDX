using $safeprojectname$.Levels;
using $safeprojectname$.Towers;
using DeltaEngine.GameLogic;

namespace $safeprojectname$.Creeps
{
	public class SandCreepStateChanger
	{
		public static void ChangeStatesIfSandCreep(TowerType damageType, Creep creep)
		{
			if (damageType == TowerType.Impact)
				SetAffectedByImpact(creep);
			else if (damageType == TowerType.Water)
				SetAffectedByWater(creep);
			else if (damageType == TowerType.Ice)
				SetAffectedByIce(creep);
			else if (damageType == TowerType.Fire)
				SetAffectedByFire(creep);
		}

		private static void SetAffectedByImpact(Creep creep)
		{
			StateChanger.MakeCreepLimitedSlow(creep);
			if (!creep.state.Frozen)
				return;

			StateChanger.CheckChanceForSudden(creep);
		}

		private static void SetAffectedByWater(Creep creep)
		{
			StateChanger.MakeCreepWet(creep);
		}

		private static void SetAffectedByIce(Creep creep)
		{
			if (creep.state.Wet)
				StateChanger.MakeCreepFrozen(creep);
		}

		private static void SetAffectedByFire(Creep creep)
		{
			if (creep.state.Wet)
				creep.state.Wet = false;
			else if (creep.state.Frozen)
			{
				creep.state.Frozen = false;
				StateChanger.MakeCreepUnfreezable(creep);
				StateChanger.MakeCreepWet(creep);
			} else
				TransformInGlassCreep(creep);
		}

		private static void TransformInGlassCreep(Creep creep)
		{
			var percentage = creep.CurrentHp / creep.Data.MaxHp;
			if (!(Level.Current is GameLevelRoom))
				return;

			var level = Level.Current as GameLevelRoom;
			var newCreep = level.SpawnCreep(CreepType.Glass);
			newCreep.CurrentHp *= percentage;
			level.RemoveCreep(creep);
		}

		public static void ChangeStartStatesIfSandCreep(Creep creep)
		{
			creep.state.SetVulnerabilitiesToNormal();
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Ice);
			StateChanger.MakeCreepImmuneToType(creep, TowerType.Acid);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Slice);
			StateChanger.MakeCreepResistantToType(creep, TowerType.Impact);
		}
	}
}