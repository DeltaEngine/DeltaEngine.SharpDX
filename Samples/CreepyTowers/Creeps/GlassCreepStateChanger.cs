using CreepyTowers.Towers;
using DeltaEngine.Core;

namespace CreepyTowers.Creeps
{
  public class GlassCreepStateChanger
  {
    public static void ChangeStatesIfGlassCreep(Tower.TowerType damageType, Creep creep,
      CreepProperties properties)
    {
      if (properties.CreepType != Creep.CreepType.Glass)
        return;
      if (damageType == Tower.TowerType.Fire)
      {
        creep.state.Melt = true;
        creep.state.Slow = true;
        creep.state.Enfeeble = true;
      }

      if (damageType == Tower.TowerType.Impact)
      {
        var chanceForShather = Randomizer.Current.Get(0, 100);
        if (chanceForShather < 10)
        {
          creep.IsActive = false;
          creep.Shatter();
        }
      }
    }

    public static void ChangeStartStatesIfGlassCreep(Creep.CreepType creepType, Creep creep)
		{
			if(creepType != Creep.CreepType.Glass)
				return;
			creep.state.SetVulnerability(Tower.TowerType.Ice, CreepState.VulnerabilityType.Weak);
			creep.state.SetVulnerability(Tower.TowerType.Acid, CreepState.VulnerabilityType.Immune);
			creep.state.SetVulnerability(Tower.TowerType.Water, CreepState.VulnerabilityType.Immune);
			creep.state.SetVulnerability(Tower.TowerType.Blade, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Fire, CreepState.VulnerabilityType.Resistant);
			creep.state.SetVulnerability(Tower.TowerType.Impact, CreepState.VulnerabilityType.Vulnerable);
    }
  }
}