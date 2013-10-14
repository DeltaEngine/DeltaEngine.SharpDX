using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;

using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			game = new Game(Resolve<Window>());
		}

		private Game game;

		[Test]
		public void CheckGameCreation()
		{
			Assert.AreEqual("Creepy Towers", Game.window.Title);
			Assert.AreEqual(new Size(1920, 1080), Game.window.ViewportPixelSize);
			Assert.IsFalse(Game.window.IsFullscreen);
			Assert.AreEqual(6.0f, game.MaxZoomedOutFovSize);
		}

		[Test, CloseAfterFirstFrame]
		public void EndGameClosesGameWindow()
		{
			Game.EndGame();
			Assert.IsTrue(Game.window.IsClosing);
		}

		[Test]
		public void CheckGameWithPathFinding()
		{
			var level = ContentLoader.Load<GameLevelRoom>("LevelsChildrensRoomInfo");
			level.RenderIn3D = true;
			new LevelRenderer(level);
			var camera = (LookAtCamera)Camera.Current;
			camera.Position = new Vector3D(-3, -3, 5);
			var newCenter = level.Size / 2.0f;
			camera.Target = new Vector3D(newCenter.Width, newCenter.Height, 0.0f);
			var path = level.GetPath(level.SpawnPoints[0] + Vector2D.Half, level.GoalPoints[0] + Vector2D.Half);
			PaintPath(path);
			level.SpawnCreep(CreepType.Glass);
		}

		private static void PaintPath(List<Vector3D> path)
		{
			for (int i = 0; i < path.Count - 1; i++)
				new Line3D(path[i] + Vector2D.Half, path[i + 1] + Vector2D.Half, Color.Green);
		}
	}
}