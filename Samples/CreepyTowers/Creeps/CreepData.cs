using System.Collections.Generic;
using CreepyTowers.Towers;

namespace CreepyTowers.Creeps
{
	/// <summary>
	/// Initial data describing a creep, has no current in game values.
	/// </summary>
	public class CreepData
	{
		public CreepData(CreepType type, string name, float maxHp, float speed, float resistance,
			int goldReward, Dictionary<TowerType, float> typeDamageModifier)
		{
			Type = type;
			Name = name;
			MaxHp = maxHp;
			Speed = speed;
			Resistance = resistance;
			GoldReward = goldReward;
			TypeDamageModifier = typeDamageModifier;
		}

		public CreepType Type { get; private set; }
		public string Name { get; private set; }
		public float MaxHp { get; private set; }
		public float Speed { get; private set; }
		public float Resistance { get; private set; }
		public int GoldReward { get; private set; }
		public readonly Dictionary<TowerType, float> TypeDamageModifier;
	}
}