using $safeprojectname$.Towers;
using DeltaEngine.Core;

namespace $safeprojectname$.Creeps
{
	public class StateChanger
	{
		public static void MakeCreepDelayed(Creep creep)
		{
			creep.state.Delayed = true;
			creep.state.DelayedTimer = 0;
		}

		public static void MakeCreepBurn(Creep creep)
		{
			creep.state.Burn = true;
			creep.state.BurnTimer = 0;
		}

		public static void MakeCreepBurst(Creep creep)
		{
			creep.state.Burst = true;
			creep.state.BurstTimer = 0;
		}

		public static void MakeCreepUnfreezable(Creep creep)
		{
			creep.state.Unfreezable = true;
			creep.state.UnfreezableTimer = 0;
			creep.state.Frozen = false;
			creep.state.Paralysed = false;
		}

		public static void MakeCreepFrozen(Creep creep)
		{
			if (creep.state.Unfreezable)
				return;

			creep.state.Frozen = true;
			creep.state.FrozenTimer = 0;
			MakeCreepParalysed(creep);
			MakeCreepResistantToType(creep, TowerType.Slice);
			creep.state.Wet = false;
			MakeCreepResistantToType(creep, TowerType.Water);
			MakeCreepVulnerableToType(creep, TowerType.Impact);
			creep.state.Burst = false;
			creep.state.Burn = false;
			MakeCreepImmuneToType(creep, TowerType.Fire);
		}

		public static void MakeCreepParalysed(Creep creep)
		{
			creep.state.Paralysed = true;
			creep.state.ParalysedTimer = 0;
		}

		public static void MakeCreepResistantToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Resistant);
		}

		public static void MakeCreepVulnerableToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Vulnerable);
		}

		public static void MakeCreepImmuneToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Immune);
		}

		public static void MakeCreepWeakToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Weak);
		}

		public static void MakeCreepFast(Creep creep)
		{
			creep.state.Fast = true;
			creep.state.FastTimer = 0;
		}

		public static void MakeCreepMelt(Creep creep)
		{
			MakeCreepEnfeeble(creep);
			MakeCreepLimitedSlow(creep);
			creep.state.Melt = true;
			creep.state.MeltTimer = 0;
		}

		public static void MakeCreepEnfeeble(Creep creep)
		{
			creep.state.Enfeeble = true;
			creep.state.EnfeebleTimer = 0;
		}

		public static void MakeCreepSlow(Creep creep)
		{
			creep.state.Slow = true;
			creep.state.SlowTimer = -1;
		}

		public static void MakeCreepLimitedSlow(Creep creep)
		{
			creep.state.Slow = true;
			creep.state.SlowTimer = 0;
		}

		public static void MakeCreepRust(Creep creep)
		{
			creep.state.Rust = true;
			creep.state.RustTimer = 0;
			MakeCreepEnfeeble(creep);
			MakeCreepSlow(creep);
		}

		public static void MakeCreepHealing(Creep creep)
		{
			creep.state.Healing = true;
		}

		public static void MakeCreepWet(Creep creep)
		{
			MakeCreepLimitedSlow(creep);
			if (creep.state.Frozen)
				return;

			creep.state.Wet = true;
			creep.state.WetTimer = 0;
			MakeCreepResistantToFire(creep);
		}

		private static void MakeCreepResistantToFire(Creep creep)
		{
			creep.state.Burst = false;
			creep.state.Burn = false;
			MakeCreepResistantToType(creep, TowerType.Fire);
		}

		public static void CheckChanceForSudden(Creep creep)
		{
			CheckChance(creep);
		}

		private static bool CheckChance(Creep creep)
		{
			var chanceForShatter = Randomizer.Current.Get(0, 100);
			if (chanceForShatter >= 10)
				return false;

			creep.CurrentHp = 0;
			return true;
		}

		public static void CheckChanceForShatter(Creep creep)
		{
			if (CheckChance(creep))
				creep.Shatter();
		}

		public static void MakeCreepHardBoiledToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.HardBoiled);
		}

		public static void MakeCreepNormalToType(Creep creep, TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Normal);
		}

		public static void CheckCreepState(TowerType type, Creep creep)
		{
			if (creep.Data.Type == CreepType.Cloth)
				ClothCreepStateChanger.ChangeStatesIfClothCreep(type, creep);
			else if (creep.Data.Type == CreepType.Sand)
				SandCreepStateChanger.ChangeStatesIfSandCreep(type, creep);
			else if (creep.Data.Type == CreepType.Glass)
				GlassCreepStateChanger.ChangeStatesIfGlassCreep(type, creep);
			else if (creep.Data.Type == CreepType.Wood)
				WoodCreepStateChanger.ChangeStatesIfWoodCreep(type, creep);
			else if (creep.Data.Type == CreepType.Plastic)
				PlasticCreepStateChanger.ChangeStatesIfPlasticCreep(type, creep);
			else if (creep.Data.Type == CreepType.Iron)
				IronCreepStateChanger.ChangeStatesIfIronCreep(type, creep);
			else if (creep.Data.Type == CreepType.Paper)
				PaperCreepStateChanger.ChangeStatesIfPaperCreep(type, creep);
		}

		public static void SetStartStateOfCreep(Creep creep)
		{
			if (creep.Data.Type == CreepType.Cloth)
				ClothCreepStateChanger.ChangeStartStatesIfClothCreep(creep);
			else if (creep.Data.Type == CreepType.Sand)
				SandCreepStateChanger.ChangeStartStatesIfSandCreep(creep);
			else if (creep.Data.Type == CreepType.Glass)
				GlassCreepStateChanger.ChangeStartStatesIfGlassCreep(creep);
			else if (creep.Data.Type == CreepType.Wood)
				WoodCreepStateChanger.ChangeStartStatesIfWoodCreep(creep);
			else if (creep.Data.Type == CreepType.Plastic)
				PlasticCreepStateChanger.ChangeStartStatesIfPlasticCreep(creep);
			else if (creep.Data.Type == CreepType.Iron)
				IronCreepStateChanger.ChangeStartStatesIfIronCreep(creep);
			else if (creep.Data.Type == CreepType.Paper)
				PaperCreepStateChanger.ChangeStartStatesIfPaperCreep(creep);
		}
	}
}