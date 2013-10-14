using $safeprojectname$.Levels;
using DeltaEngine.GameLogic;

namespace $safeprojectname$.Triggers
{
	public class SubstractLife : GameTrigger
	{
		public SubstractLife(string value)
		{
		}

		protected override void EnemyReachGoal()
		{
			if (!(Level.Current is GameLevelRoom))
				return;

			(Level.Current as GameLevelRoom).Lives--;
			if ((Level.Current as GameLevelRoom).Lives <= 0)
				OnGameOver();
		}
	}
}