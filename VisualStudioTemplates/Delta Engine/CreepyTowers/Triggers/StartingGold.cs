using $safeprojectname$.Levels;
using DeltaEngine.Extensions;
using DeltaEngine.GameLogic;

namespace $safeprojectname$.Triggers
{
	public class StartingGold : GameTrigger
	{
		public StartingGold(string startingGoldAmount)
		{
			Gold = startingGoldAmount.Convert<int>();
		}

		public int Gold
		{
			get;
			private set;
		}

		protected override void StartingLevel()
		{
			if (Level.Current is GameLevelRoom)
				(Level.Current as GameLevelRoom).Gold = Gold;
		}
	}
}