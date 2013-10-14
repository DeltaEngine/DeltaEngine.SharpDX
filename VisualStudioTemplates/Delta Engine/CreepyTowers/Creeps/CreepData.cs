using System.Collections.Generic;
using $safeprojectname$.Towers;

namespace $safeprojectname$.Creeps
{
	public class CreepData
	{
		public CreepData(CreepType type, string name, float maxHp, float speed, float resistance, int 
			goldReward, Dictionary<TowerType, float> typeDamageModifier)
		{
			Type = type;
			Name = name;
			MaxHp = maxHp;
			Speed = speed;
			Resistance = resistance;
			GoldReward = goldReward;
			TypeDamageModifier = typeDamageModifier;
		}

		public CreepType Type
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public float MaxHp
		{
			get;
			private set;
		}

		public float Speed
		{
			get;
			private set;
		}

		public float Resistance
		{
			get;
			private set;
		}

		public int GoldReward
		{
			get;
			private set;
		}

		public readonly Dictionary<TowerType, float> TypeDamageModifier;
	}
}