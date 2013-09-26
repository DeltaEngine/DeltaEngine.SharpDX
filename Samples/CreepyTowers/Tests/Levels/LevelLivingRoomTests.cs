using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Models;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	public class LevelLivingRoomTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			level = null;
		}

		private LevelLivingRoom level;

		[Test]
		public void ShowLivingRoomLevelWithGrid()
		{
			level = new LevelLivingRoom();
		}

		[Test]
		public void DiposingLevelRemovesModel()
		{
			level = new LevelLivingRoom();
			level.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Model>().Count);
		}
	}
}
