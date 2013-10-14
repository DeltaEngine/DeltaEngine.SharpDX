using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace $safeprojectname$.Creeps
{
	public class WaveManager : Entity
	{
		public WaveManager(List<CreepWave> waveList, Manager manager)
		{
			this.waveList = waveList;
			this.manager = manager;
			Start<WaveCreation>();
		}

		public readonly List<CreepWave> waveList;
		public readonly Manager manager;
		public class WaveCreation : UpdateBehavior
		{
			public WaveCreation()
			{
				timeSinceLastWave = 0.0f;
				timeSinceLastCreep = 0.0f;
				creepCountForCurrentWave = 0;
			}

			private float timeSinceLastWave;
			private float timeSinceLastCreep;
			private int creepCountForCurrentWave;

			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (WaveManager waveManager in entities)
				{
					if (waveManager.waveList.Count == 0)
						return;

					var wave = waveManager.waveList [0];
					if (timeSinceLastWave < wave.WaitTime)
					{
						timeSinceLastWave += Time.Delta;
						return;
					}
					CreateWave(wave, waveManager);
				}
			}

			private void CreateWave(CreepWave wave, WaveManager waveManager)
			{
				var creepList = wave.CreepSpawnList;
				if (creepList == null || creepList.Count == 0)
					return;

				if (creepCountForCurrentWave >= wave.TotalCreepCount)
				{
					timeSinceLastWave = 0.0f;
					creepCountForCurrentWave = 0;
					waveManager.waveList.RemoveAt(0);
					return;
				}
				if (timeSinceLastCreep < wave.SpawnInterval)
				{
					timeSinceLastCreep += Time.Delta;
					return;
				}
				var item = creepList [creepCountForCurrentWave];
				if (item.GetType() == typeof(CreepType))
					SpawnCreep((CreepType)item, waveManager.manager);
				else if (item.GetType() == typeof(CreepGroup))
					SpawnGroup((CreepGroup)item, waveManager.manager);
			}

			private void SpawnCreep(CreepType creepType, Manager manager)
			{
				timeSinceLastCreep = 0.0f;
				creepCountForCurrentWave++;
				var startPos = new Tuple<int, int>(1, 1);
				var wayPoints = new List<Tuple<int, int>> {
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(3, 5)
				};
				var creepPos = Vector3D.Zero + new Vector3D(0.0f, creepCountForCurrentWave / 2.0f, 0.0f);
				manager.CreateCreep(creepPos, creepType, MovementData(startPos, wayPoints));
			}

			private void SpawnGroup(CreepGroup group, Manager manager)
			{
				foreach (CreepType creepType in group.CreepList)
					SpawnCreep(creepType, manager);
			}
		}
		private static MovementInGrid.MovementData MovementData(Tuple<int, int> startPos, 
			List<Tuple<int, int>> waypoints)
		{
			return new MovementInGrid.MovementData {
				Velocity = new Vector3D(0.5f, 0.5f, 0.0f),
				StartGridPos = startPos,
				Waypoints = waypoints
			};
		}
	}
}