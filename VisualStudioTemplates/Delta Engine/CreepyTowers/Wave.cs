using System.Collections.Generic;

namespace $safeprojectname$
{
	public class Wave
	{
		public float WaitTime
		{
			get;
			set;
		}

		public float CreepSpawnInterval
		{
			get;
			set;
		}

		public int MaxCreeps
		{
			get;
			set;
		}

		public float MaxTimeTillNextWave
		{
			get;
			set;
		}

		public List<string> CreepList
		{
			get;
			set;
		}

		public List<Creep.CreepType> GetCreepList()
		{
			var list = new List<Creep.CreepType>();
			foreach (var creepName in CreepList)
				if (creepName == "Cloth")
					list.Add(Creep.CreepType.Cloth);
				else if (creepName == "Iron")
					list.Add(Creep.CreepType.Iron);
				else if (creepName == "Paper")
					list.Add(Creep.CreepType.Paper);
				else if (creepName == "Wood")
					list.Add(Creep.CreepType.Wood);
				else if (creepName == "Glass")
					list.Add(Creep.CreepType.Glass);
				else if (creepName == "Sand")
					list.Add(Creep.CreepType.Sand);
				else if (creepName == "Plastic")
					list.Add(Creep.CreepType.Plastic);

			return list;
		}
	}
}