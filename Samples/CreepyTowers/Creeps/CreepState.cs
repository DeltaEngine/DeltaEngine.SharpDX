using System;
using CreepyTowers.Towers;
using DeltaEngine.Entities;

namespace CreepyTowers.Creeps
{
	public class CreepState : IEquatable<CreepState>
	{
		public CreepState()
		{
			VulnerabilityState = new VulnerabilityType[numberOfTowerTypes];
			SetVulnerabilitiesToNormal();
		}

		private readonly int numberOfTowerTypes = Enum.GetNames(typeof(TowerType)).Length;
		public readonly VulnerabilityType[] VulnerabilityState;

		public void SetVulnerabilitiesToNormal()
		{
			for (int i = 0; i < numberOfTowerTypes; i++)
				VulnerabilityState[i] = VulnerabilityType.Normal;
		}

		public enum VulnerabilityType
		{
			Vulnerable,
			Weak,
			Sudden,
			Normal,
			Resistant,
			HardBoiled,
			Immune
		}

		public VulnerabilityType GetVulnerability(TowerType type)
		{
			return VulnerabilityState[(int)type];
		}

		public void SetVulnerability(TowerType towerType, VulnerabilityType type)
		{
			VulnerabilityState[(int)towerType] = type;
		}

		public bool Slow { get; set; }
		public float SlowTimer { get; set; }
		public bool Delayed { get; set; }
		public float DelayedTimer { get; set; }
		public bool Burn { get; set; }
		public float BurnTimer { get; set; }
		public bool Burst { get; set; }
		public float BurstTimer { get; set; }
		public bool Paralysed { get; set; }
		public float ParalysedTimer { get; set; }
		public bool Frozen { get; set; }
		public float FrozenTimer { get; set; }
		public bool Unfreezable { get; set; }
		public float UnfreezableTimer { get; set; }
		public bool Fast { get; set; }
		public float FastTimer { get; set; }
		public bool Enfeeble { get; set; }
		public float EnfeebleTimer { get; set; }
		public bool Melt { get; set; }
		public float MeltTimer { get; set; }
		public bool Rust { get; set; }
		public float RustTimer { get; set; }
		public bool Wet { get; set; }
		public float WetTimer { get; set; }
		public bool Healing { get; set; }
		public bool Sudden { get; set; }
		public float MaxTimeShort = 2;
		public float MaxTimeMedium = 5;
		public float MaxTimeLong = 10;

		public bool RecachedMaxTime(float timer, float maxTime)
		{
			return timer + Time.Delta <= maxTime;
		}

		public bool Equals(CreepState other)
		{
			return Slow == other.Slow && SlowTimer == other.SlowTimer && Delayed == other.Delayed &&
				DelayedTimer == other.DelayedTimer && Burn == other.Burn && BurnTimer == other.BurnTimer &&
				Burst == other.Burst && BurstTimer == other.BurstTimer && Paralysed == other.Paralysed &&
				ParalysedTimer == other.ParalysedTimer && Frozen == other.Frozen &&
				FrozenTimer == other.FrozenTimer && Unfreezable == other.Unfreezable &&
				UnfreezableTimer == other.UnfreezableTimer && Fast == other.Fast &&
				FastTimer == other.FastTimer && Enfeeble == other.Enfeeble &&
				EnfeebleTimer == other.EnfeebleTimer && Melt == other.Melt && 
				MeltTimer == other.MeltTimer && Rust == other.Rust && RustTimer == other.RustTimer && 
				Wet == other.Wet && WetTimer == other.WetTimer && Healing == other.Healing &&
				Sudden == other.Sudden && MaxTimeShort == other.MaxTimeShort && 
				MaxTimeMedium == other.MaxTimeMedium && MaxTimeLong == other.MaxTimeLong;
		}
	}
}