using System.Collections.Generic;

namespace CreepyTowers
{
	public class CreepProperties
	{
		public string Name { get; set; }
		public float MaxHp { get; set; }
		public float CurrentHp { get; set; }
		public float Speed { get; set; }
		public float Resistance { get; set; }
		public Creep.CreepType CreepType { get; set; }
		public int GoldReward { get; set; }
		public Dictionary<Tower.TowerType, float> TypeDamageModifier;
	}
}