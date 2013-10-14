using CreepyTowers.Levels;
using DeltaEngine.Extensions;
using DeltaEngine.GameLogic;

namespace CreepyTowers.Triggers
{
	public class StartingLifes : GameTrigger
	{
		public StartingLifes() {}

		public StartingLifes(string startingLifesAmount)
		{
			Lifes = startingLifesAmount.Convert<int>();
		}

		public int Lifes { get; private set; }

		protected override void StartingLevel()
		{
			if (Level.Current is GameLevelRoom)
				(Level.Current as GameLevelRoom).Lives = Lifes;
		}
	}
}