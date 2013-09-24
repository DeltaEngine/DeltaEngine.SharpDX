using System.Collections.Generic;
using $safeprojectname$.Creeps;
using DeltaEngine.Entities;

namespace $safeprojectname$.Towers
{
	public class FindPossibleTargets : UpdateBehavior
	{
		public override void Update(IEnumerable<Entity> entities)
		{
			listOfCreeps = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			listOfTowers = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			foreach (Manager manager in entities)
			{
				if (listOfCreeps.Count < 1 || listOfTowers.Count < 1)
					return;

				foreach (Tower tower in listOfTowers)
				{
					if (listOfCreeps.Count < 1)
						return;

					tower.RemoveFireLine();
					var closestCreep = FindClosestCreepToAttack(tower);
					tower.FireAtCreep(closestCreep);
					if (closestCreep == null)
						continue;

					closestCreep.CreepIsDead += closestCreep.Dispose;
					var currentManager = manager;
					closestCreep.UpdateHealthBar += () => currentManager.UpdateCreepHealthBar(closestCreep);
				}
				foreach (Creep creep in listOfCreeps)
				{
					creep.UpdateStateTimersAndTimeBasedDamage();
				}
			}
		}

		private List<Creep> listOfCreeps;
		private List<Tower> listOfTowers;

		private Creep FindClosestCreepToAttack(Tower tower)
		{
			var closestCreep = listOfCreeps [0];
			var dist = (closestCreep.Position - tower.Position).Length;
			foreach (Creep creep in listOfCreeps)
			{
				var distVec = creep.Position - tower.Position;
				if (distVec.Length < dist)
					closestCreep = creep;
			}
			if (tower.Get<TowerProperties>().Range < DistanceBetweenClosestCreepAndTower(closestCreep, 
				tower))
				closestCreep = null;

			return closestCreep;
		}

		private static float DistanceBetweenClosestCreepAndTower(Creep closestCreep, Tower tower)
		{
			return (closestCreep.Position - tower.Position).Length;
		}
	}
}