using System.Collections.Generic;

namespace CreepyTowers.Creeps
{ 
	//TODO: isn't this the same as Group?
	public class CreepGroup
	{
		public List<CreepType> CreepList { get; set; }
		public float SpawnInterval { get; set; }
	}
}