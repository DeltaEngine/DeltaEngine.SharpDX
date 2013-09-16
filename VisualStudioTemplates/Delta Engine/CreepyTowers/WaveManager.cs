using System;
using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace $safeprojectname$
{
	public class WaveManager : Entity
	{
		public WaveManager(List<Wave> waveList, Manager manager)
		{
			this.waveList = waveList;
			this.manager = manager;
			Start<WaveCreation>();
		}

		public readonly List<Wave> waveList;
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

					CreateWave(waveManager);
				}
			}

			private void CreateWave(WaveManager waveManager)
			{
				timeSinceLastWave += Time.Delta;
				var wave = waveManager.waveList [0];
				var totalTimeTillNextWave = wave.WaitTime + (wave.MaxCreeps * wave.CreepSpawnInterval);
				if (totalTimeTillNextWave > wave.MaxTimeTillNextWave)
					totalTimeTillNextWave = wave.MaxTimeTillNextWave;

				if (timeSinceLastWave >= totalTimeTillNextWave)
				{
					waveManager.waveList.RemoveAt(0);
					timeSinceLastWave = 0;
				}
				SpawnCreep(wave, waveManager.manager);
			}

			private void SpawnCreep(Wave wave, Manager manager)
			{
				if (creepCountForCurrentWave > wave.MaxCreeps)
				{
					creepCountForCurrentWave = 0;
					return;
				}
				creepCountForCurrentWave++;
				timeSinceLastCreep += Time.Delta;
				var startPos = new Tuple<int, int>(1, 1);
				var wayPoints = new List<Tuple<int, int>> {
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(3, 5)
				};
				if (timeSinceLastCreep < wave.CreepSpawnInterval)
					return;

				manager.CreateCreep(Vector.One, Names.CreepCottonMummy, MovementData(startPos, wayPoints));
				timeSinceLastCreep = 0.0f;
			}
		}
		private static List<Tuple<int, int>> SelectRandomWaypointList(Level.GridData data)
		{
			var randomNo = Randomizer.Current.Get(0, 1);
			return data.CreepPathsList [randomNo];
		}

		private static MovementInGrid.MovementData MovementData(Tuple<int, int> startPos, 
			List<Tuple<int, int>> waypoints)
		{
			return new MovementInGrid.MovementData {
				Velocity = new Vector(0.2f, 0.0f, 0.2f),
				StartGridPos = startPos,
				Waypoints = waypoints
			};
		}
	}
}