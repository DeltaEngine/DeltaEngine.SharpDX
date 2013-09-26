using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Core;

namespace CreepyTowers.Simple2D
{
	public class StateChanger
	{
		public static void MakeCreepDelayed(Creep2D creep)
		{
			creep.state.Delayed = true;
			creep.state.DelayedTimer = 0;
		}

		public static void MakeCreepBurn(Creep2D creep)
		{
			creep.state.Burn = true;
			creep.state.BurnTimer = 0;
		}

		public static void MakeCreepBurst(Creep2D creep)
		{
			creep.state.Burst = true;
			creep.state.BurstTimer = 0;
		}

		public static void MakeCreepUnfreezable(Creep2D creep)
		{
			creep.state.Unfreezable = true;
			creep.state.UnfreezableTimer = 0;
			creep.state.Frozen = false;
			creep.state.Paralysed = false;
		}

		public static void MakeCreepFrozen(Creep2D creep)
		{
			if (creep.state.Unfreezable)
				return;
			creep.state.Frozen = true;
			creep.state.FrozenTimer = 0;
			MakeCreepParalysed(creep);
			MakeCreepResistantToType(creep, Tower.TowerType.Slice);
			creep.state.Wet = false;
			MakeCreepResistantToType(creep, Tower.TowerType.Water);
			MakeCreepVulnerableToType(creep, Tower.TowerType.Impact);
			creep.state.Burst = false;
			creep.state.Burn = false;
			MakeCreepImmuneToType(creep, Tower.TowerType.Fire);
		}

		public static void MakeCreepParalysed(Creep2D creep)
		{
			creep.state.Paralysed = true;
			creep.state.ParalysedTimer = 0;
		}

		public static void MakeCreepResistantToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Resistant);
		}

		public static void MakeCreepVulnerableToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Vulnerable);
		}

		public static void MakeCreepImmuneToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Immune);
		}

		public static void MakeCreepWeakToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Weak);
		}

		public static void MakeCreepFast(Creep2D creep)
		{
			creep.state.Fast = true;
			creep.state.FastTimer = 0;
		}

		public static void MakeCreepMelt(Creep2D creep)
		{
			MakeCreepEnfeeble(creep);
			MakeCreepLimitedSlow(creep);
			creep.state.Melt = true;
			creep.state.MeltTimer = 0;
		}

		public static void MakeCreepEnfeeble(Creep2D creep)
		{
			creep.state.Enfeeble = true;
			creep.state.EnfeebleTimer = 0;
		}

		public static void MakeCreepSlow(Creep2D creep)
		{
			creep.state.Slow = true;
			creep.state.SlowTimer = -1;
		}

		public static void MakeCreepLimitedSlow(Creep2D creep)
		{
			creep.state.Slow = true;
			creep.state.SlowTimer = 0;
		}

		public static void MakeCreepRust(Creep2D creep)
		{
			creep.state.Rust = true;
			creep.state.RustTimer = 0;
			MakeCreepEnfeeble(creep);
			MakeCreepSlow(creep);
		}

		public static void MakeCreepHealing(Creep2D creep)
		{
			creep.state.Healing = true;
		}

		public static void MakeCreepWet(Creep2D creep)
		{
			MakeCreepLimitedSlow(creep);
			if (creep.state.Frozen)
				return;
			creep.state.Wet = true;
			creep.state.WetTimer = 0;
			MakeCreepResistantToFire(creep);
		}

		private static void MakeCreepResistantToFire(Creep2D creep)
		{
			creep.state.Burst = false;
			creep.state.Burn = false;
			MakeCreepResistantToType(creep, Tower.TowerType.Fire);
		}

		public static void CheckChanceForSudden(Creep2D creep)
		{
			CheckChance(creep);
		}

		private static bool CheckChance(Creep2D creep)
		{
			var chanceForShather = Randomizer.Current.Get(0, 100);
			if (chanceForShather >= 10)
				return false;
			creep.Hitpoints = 0;
			creep.data.CurrentHp = 0;
			return true;
		}

		public static void CheckChanceForShatter(Creep2D creep)
		{
			if (CheckChance(creep))
				creep.Shatter();
		}

		public static void MakeCreepHardBoiledToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.HardBoiled);
		}

		public static void MakeCreepNormalToType(Creep2D creep, Tower.TowerType type)
		{
			creep.state.SetVulnerability(type, CreepState.VulnerabilityType.Normal);
		}
	}
}
