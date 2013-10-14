namespace CreepyTowers.Towers
{
	/// <summary>
	/// Initial data describing a tower, has no current in game values.
	/// </summary>
	public class TowerData
	{
		public TowerData(TowerType type, string name, AttackType attackType, float range,
			float frequency, float damage, int cost)
		{
			Type = type;
			Name = name;
			AttackType = attackType;
			Range = range;
			AttackFrequency = frequency;
			AttackDamage = damage;
			Cost = cost;
		}

		public TowerType Type { get; private set; }
		public string Name { get; private set; }
		public AttackType AttackType { get; private set; }
		public float Range { get; private set; }
		public float AttackFrequency { get; private set; }
		public float AttackDamage { get; private set; }
		public int Cost { get; private set; }
	}
}