using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace CreepyTowers
{
	public class MovementInGrid : UpdateBehavior
	{
		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (Creep creep in entities)
			{
				movementData = creep.Get<MovementData>();
				wayPoints = movementData.Waypoints;

				var grid = Game.CameraAndGrid.Grid;
				if (wayPoints.Count == 0)
					return;

				movementData.FinalGridPos = wayPoints[0];
				var destination =
					grid.PropertyMatrix[movementData.FinalGridPos.Item1, movementData.FinalGridPos.Item2].
						MidPoint;

				if (movementData.StartGridPos.Item2 == movementData.FinalGridPos.Item2)
					MoveCreepAlongXAxis(creep, destination, GetVelocity(creep, movementData.Velocity.X));

				else if (movementData.StartGridPos.Item1 == movementData.FinalGridPos.Item1)
					MoveCreepAlongZAxis(creep, destination, GetVelocity(creep, movementData.Velocity.Z));

				if (movementData.StartGridPos.Equals(movementData.FinalGridPos) && wayPoints.Count > 0)
					wayPoints.RemoveAt(0);

				if (creep.Position ==
					grid.PropertyMatrix[movementData.FinalGridPos.Item1, movementData.FinalGridPos.Item2].
						MidPoint && wayPoints.Count < 1)
					creep.Dispose();
			}
		}

		private MovementData movementData;
		private List<Tuple<int, int>> wayPoints;

		private void MoveCreepAlongXAxis(Creep creep, Vector finalStop, float velocity)
		{
			var dir = finalStop - creep.Position;
			var distToMovePerFrame = Vector.Normalize(dir).X * velocity * Time.Delta;
			creep.Position = new Vector(creep.Position.X + distToMovePerFrame, creep.Position.Y,
				creep.Position.Z);

			creep.UpdateHealthBarPositionAndImage();
			CheckIfCreepReachedFinalStop(creep, finalStop);
		}

		private static float GetVelocity(Creep creep, float velocity)
		{
			if (creep.state.Paralysed || creep.state.Frozen)
				return 0;
			if (creep.state.Delayed)
				return velocity / 3;
			if (creep.state.Slow)
				return velocity / 2;
			if (creep.state.Fast)
				return velocity * 2;
			return velocity;
		}

		//private void CheckIfCreepReachedFinalStop(Creep creep, Vector finalStop)
		//{
		//	var dist = (finalStop - creep.Position).Length;

		//	if (!(dist < Game.CameraAndGrid.Grid.GridScale * 0.25f))
		//		return;

		//	movementData.StartGridPos = movementData.FinalGridPos;
		//	creep.Position = finalStop;
		//	creep.Dispose();
		//}

		private void CheckIfCreepReachedFinalStop(Creep creep, Vector finalStop)
		{
			var dist = (finalStop - creep.Position).Length;
			if (dist > Game.CameraAndGrid.Grid.GridScale * 0.25)
				return;

			movementData.StartGridPos = movementData.FinalGridPos;
			creep.Position = finalStop;
		}

		private void MoveCreepAlongZAxis(Creep creep, Vector finalStop, float velocity)
		{
			var dir = finalStop - creep.Position;
			var distToMovePerFrame = Vector.Normalize(dir).Z * velocity * Time.Delta;
			creep.Position = new Vector(creep.Position.X, creep.Position.Y,
				creep.Position.Z + distToMovePerFrame);

			creep.UpdateHealthBarPositionAndImage();
			CheckIfCreepReachedFinalStop(creep, finalStop);
		}

		public class MovementData
		{
			public Vector Velocity { get; set; }
			public Tuple<int, int> StartGridPos { get; set; }
			public Tuple<int, int> FinalGridPos { get; set; }
			public List<Tuple<int, int>> Waypoints { get; set; }
		}
	}
}