using System.IO;
using DeltaEngine;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class GameTests : TestWithMocksOrVisually
	{
		[Test]
		public void GameOver()
		{
			var game = new Game(Resolve<Window>());
			game.StartGame();
			game.GameOver();
			Assert.AreEqual(GameState.GameOver, game.GameState);
			Assert.IsFalse(game.InteractionLogics.Player.IsActive);
		}

		[Test, Ignore, CloseAfterFirstFrame]
		public void CreateAsteroidsMaterials()
		{
			SaveMaterialWithNameAndImageName("PlayerMaterial", "ship1");
			SaveMaterialWithNameAndImageName("AsteroidMaterial", "asteroid");
			SaveMaterialWithNameAndImageName("BackgroundMaterial", "black-background");
			SaveMaterialWithNameAndImageName("ProjectileMaterial", "projectile");
		}

		private static void SaveMaterialWithNameAndImageName(string materialName, string imageName)
		{
			var materialData = new Material(Shader.Position2DColorUv, imageName);
			using (var file = File.Create(Path.Combine("Content", materialName + ".deltamaterial")))
				BinaryDataExtensions.Save(materialData, new BinaryWriter(file));
		}
	}
}