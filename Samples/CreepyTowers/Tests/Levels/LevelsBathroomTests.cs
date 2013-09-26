using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	public class LevelsBathroomTests : TestWithMocksOrVisually
	{
		
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			level = null;
		}

		private LevelBathRoom level;

		[Test]
		public void ShowBathroomLevelWithGrid()
		{
			level = new LevelBathRoom();
		}

		[Test]
		public void DiposingLevelRemovesLevel()
		{
			level = new LevelBathRoom();
			level.Dispose();
		}
	}
}