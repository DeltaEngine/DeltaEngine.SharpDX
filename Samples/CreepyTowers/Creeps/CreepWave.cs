using System;
using System.Collections.Generic;
using DeltaEngine.GameLogic;

namespace CreepyTowers.Creeps
{
	public class CreepWave : Wave
	{
		public CreepWave(float waitTime, float spawnInterval, string thingsToSpawn)
			: base(waitTime, spawnInterval, thingsToSpawn)
		{
			CreepSpawnList = new List<Object>();
			TotalCreepCount = 0;
			GetCreepSpawnList();
		}

		public CreepWave(Wave wave)
			: base(wave)
		{
			CreepSpawnList = new List<Object>();
			TotalCreepCount = 0;
			GetCreepSpawnList();
		}

		public List<Object> CreepSpawnList { get; private set; }
		public int TotalCreepCount { get; private set; }

		private void GetCreepSpawnList()
		{
			foreach (var name in SpawnTypeList)
				if (creepList.Contains(name))
					CreepSpawnList.Add(PopulateFromCreepList(name));
				else if (groupList.Contains(name))
					PopulateFromGroupList(name);
		}

		private readonly List<string> creepList = new List<string>
		{
			"Cloth",
			"Iron",
			"Paper",
			"Wood",
			"Glass",
			"Plastic",
			"Sand"
		};

		private readonly List<string> groupList = new List<string>
		{
			"Squad",
			"IronMen",
			"WoodPeople",
			"Sandman",
			"Garbage",
			"Mix"
		};

		private CreepType PopulateFromCreepList(string name)
		{
			switch (name)
			{
			case "Cloth":
				creepType = CreepType.Cloth;
				TotalCreepCount++;
				break;
			case "Iron":
				creepType = CreepType.Iron;
				TotalCreepCount++;
				break;
			case "Paper":
				creepType = CreepType.Paper;
				TotalCreepCount++;
				break;
			case "Wood":
				creepType = CreepType.Wood;
				TotalCreepCount++;
				break;
			case "Glass":
				creepType = CreepType.Glass;
				TotalCreepCount++;
				break;
			case "Sand":
				creepType = CreepType.Sand;
				TotalCreepCount++;
				break;
			case "Plastic":
				creepType = CreepType.Plastic;
				TotalCreepCount++;
				break;
			default:
				throw new InvalidCreepName();
			}
			return creepType;
		}

		private CreepType creepType;

		private void PopulateFromGroupList(string name)
		{
			switch (name)
			{
			case "Squad":
				creepNamesInGroup = new List<string> { "Plastic", "Cloth", "Plastic" };
				break;
			case "IronMen":
				creepNamesInGroup = new List<string> { "Iron", "Iron" };
				break;
			case "WoodPeople":
				creepNamesInGroup = new List<string> { "Paper", "Paper", "Wood", "Wood", "Wood" };
				break;
			case "Sandman":
				creepNamesInGroup = new List<string> { "Sand", "Sand", "Glass", "Cloth", "Sand" };
				break;
			case "Garbage":
				creepNamesInGroup = new List<string> { "Plastic", "Plastic", "Cloth", "Cloth", "Plastic" };
				break;
			case "Mix":
				creepNamesInGroup = new List<string>
				{
					"Plastic",
					"Paper",
					"Wood",
					"Wood",
					"Cloth",
					"Plastic"
				};
				break;
			default:
				throw new InvalidGroupName();
			}
			if (creepNamesInGroup == null || creepNamesInGroup.Count == 0)
				return;
			creepGroup = new CreepGroup
			{
				CreepList = new List<CreepType>(),
				SpawnInterval = 0.5f
			};
			foreach (string creepName in creepNamesInGroup)
				creepGroup.CreepList.Add(PopulateFromCreepList(creepName));
			CreepSpawnList.Add(creepGroup);
		}

		private List<string> creepNamesInGroup;
		private CreepGroup creepGroup;

		public class InvalidCreepName : Exception {}
		public class InvalidGroupName : Exception {}
	}
}