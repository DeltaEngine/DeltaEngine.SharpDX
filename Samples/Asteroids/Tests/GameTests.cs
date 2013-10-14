using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Particles;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class GameTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void GameOver()
		{
			var game = new Game(Resolve<Window>());
			game.StartGame();
			game.GameOver();
			Assert.AreEqual(GameState.GameOver, game.GameState);
			Assert.IsFalse(game.InteractionLogics.Player.IsActive);
		}

		[Test]
		public void LoadExplosion()
		{
			var emitterData = ContentLoader.Load<ParticleEmitterData>("ExplosionSpaceship");
			new ParticleEmitter(emitterData, Vector3D.Zero);
		}
	}
}