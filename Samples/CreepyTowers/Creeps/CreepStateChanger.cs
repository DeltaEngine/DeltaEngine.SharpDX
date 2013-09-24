using CreepyTowers.Towers;

namespace CreepyTowers.Creeps
{
	public class CreepStateChanger
	{
		public void CheckIfChangingCreepState(Tower.TowerType damageType, Creep creep,
			CreepProperties properties)
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(damageType, creep, properties);
			SandCreepStateChanger.ChangeStatesIfSandCreep(damageType, creep, properties);
			GlassCreepStateChanger.ChangeStatesIfGlassCreep(damageType, creep, properties);
		}

		public void SetStartStateOfCreep(Creep.CreepType creepType, Creep creep)
		{
			ClothCreepStateChanger.ChangeStartStatesIfClothCreep(creepType, creep);
			SandCreepStateChanger.ChangeStartStatesIfSandCreep(creepType, creep);
			GlassCreepStateChanger.ChangeStartStatesIfGlassCreep(creepType, creep);
		}
	}
}