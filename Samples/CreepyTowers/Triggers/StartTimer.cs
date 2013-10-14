using CreepyTowers.Levels;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.GameLogic;

namespace CreepyTowers.Triggers
{
	public class StartTimer : GameTrigger
	{
		public StartTimer(string startingTime)
		{
			StartingTime = startingTime.Convert<int>();
		}

		public int StartingTime { get; private set; }

		protected override void StartingLevel()
		{
			if (Level.Current is GameLevelRoom)
				(Level.Current as GameLevelRoom).Time = StartingTime;
		}

		protected override void UpdateAfterOneSecond()
		{
			deltaTime += Time.Delta;
			if (deltaTime < 1.0f)
				return;
			if (!(Level.Current is GameLevelRoom))
				return;
			(Level.Current as GameLevelRoom).Time--;
			if ((Level.Current as GameLevelRoom).Time <= 0)
				OnGameOver();
			deltaTime -= 1.0f;
		}

		private float deltaTime;
	}
}