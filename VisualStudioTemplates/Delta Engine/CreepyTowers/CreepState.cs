using DeltaEngine.Entities;

namespace $safeprojectname$
{
	public class CreepState
	{
		public CreepState()
		{
			SetVulnerabilitiesToNormal();
		}

		public void SetVulnerabilitiesToNormal()
		{
			for (int i = 0; i < 6; i++)
				VulnerabilityState [i] = VulnerabilityType.Normal;
		}

		public VulnerabilityType[] VulnerabilityState = new VulnerabilityType[6];
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
		public VulnerabilityType GetVulnerability(Tower.TowerType type)
		{
			return VulnerabilityState [(int)type];
		}

		public void SetVulnerability(Tower.TowerType towerType, VulnerabilityType type)
		{
			VulnerabilityState [(int)towerType] = type;
		}

		public bool Slow
		{
			get;
			set;
		}

		public float SlowTimer
		{
			get;
			set;
		}

		public bool Delayed
		{
			get;
			set;
		}

		public float DelayedTimer
		{
			get;
			set;
		}

		public bool Burn
		{
			get;
			set;
		}

		public float BurnTimer
		{
			get;
			set;
		}

		public bool Burst
		{
			get;
			set;
		}

		public float BurstTimer
		{
			get;
			set;
		}

		public bool Paralysed
		{
			get;
			set;
		}

		public float ParalysedTimer
		{
			get;
			set;
		}

		public bool Frozen
		{
			get;
			set;
		}

		public float FrozenTimer
		{
			get;
			set;
		}

		public bool Fast
		{
			get;
			set;
		}

		public float FastTimer
		{
			get;
			set;
		}

		public bool Enfeeble
		{
			get;
			set;
		}

		public float EnfeebleTimer
		{
			get;
			set;
		}

		public bool Melt
		{
			get;
			set;
		}

		public float MeltTimer
		{
			get;
			set;
		}

		public bool Rust
		{
			get;
			set;
		}

		public float RustTimer
		{
			get;
			set;
		}

		public bool Wet
		{
			get;
			set;
		}

		public float WetTimer
		{
			get;
			set;
		}

		public bool Healing
		{
			get;
			set;
		}

		public bool Sudden
		{
			get;
			set;
		}

		public float MaxTimeShort = 2;
		public float MaxTimeMedium = 5;
		public float MaxTimeLong = 10;

		public bool UpdateSlowState(float timer, float maxTime)
		{
			timer += Time.Delta;
			return !(timer > maxTime);
		}
	}
}