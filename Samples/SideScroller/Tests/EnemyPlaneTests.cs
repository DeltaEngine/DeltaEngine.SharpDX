using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace SideScroller.Tests
{
	public class EnemyPlaneTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateEnemyPlane()
		{
			var foeTexture = new Material(Shader.Position2DColorUV, EnemyTextureName);
			enemy = new EnemyPlane(foeTexture, new Vector2D(1.2f, 0.5f));
			enemy.EnemyFiredShot += d => DoSomehting();
		}

		private const string EnemyTextureName = "EnemyPlane";
		private EnemyPlane enemy;
		private static void DoSomehting() {}

		[Test]
		public void LowerLifeWhenHitByBullet()
		{
			var foeTexture = new Material(Shader.Position2DColorUV, EnemyTextureName);
			enemy = new EnemyPlane(foeTexture, new Vector2D(1.2f, 0.5f));
			Assert.AreEqual(5, enemy.Hitpoints);
			enemy.CheckIfHitAndReact(new Vector2D(1.2f, 0.5f));
			enemy.EnemyFiredShot += d => DoSomehting();
			Assert.AreEqual(4, enemy.Hitpoints);
		}

		[Test]
		public void DefeatEnemyPlane()
		{
			var foeTexture = new Material(Shader.Position2DColorUV, EnemyTextureName);
			enemy = new EnemyPlane(foeTexture, new Vector2D(1.2f, 0.9f));
			for (int i = 0; i < 7; i++)
				enemy.CheckIfHitAndReact(new Vector2D(1.2f, 0.9f));
			AdvanceTimeAndUpdateEntities(0.3f);
			Assert.LessOrEqual(enemy.Hitpoints, 0);
			Assert.IsFalse(enemy.IsActive);
		}

		[Test]
		public void EnemyPlaneMovesThroughLeftSideOfScreen()
		{
			var foeTexture = new Material(Shader.Position2DColorUV, EnemyTextureName);
			enemy = new EnemyPlane(foeTexture, new Vector2D(0.2f, 0.9f));
			enemy.EnemyFiredShot += d => DoSomehting();
			AdvanceTimeAndUpdateEntities(0.7f);
			Assert.IsFalse(enemy.IsActive);
		}
	}
}