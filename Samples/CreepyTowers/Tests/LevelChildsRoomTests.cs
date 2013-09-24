using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering3D.Models;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class LevelChildsRoomTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateChildsRoom()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			level = new LevelChildsRoom();
		}

		private LevelChildsRoom level;

		[Test]
		public void DisposeLevel()
		{
			level.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Model>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Entity2D>().Count);
		}

		//[Test]
		//public void MoveTutorialDialogueBackward()
		//{
		//	var scene = new CreateSceneFromXml(Names.XmlSceneChildsRoom, Messages.ChildsRoomMessages());
		//	scene.MessageCount = 1;
		//	scene.DialogueBack();
		//}
	}
}