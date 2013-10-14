using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Physics2D;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace ShadowShot.Tests
{
	public class ProjectileTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			Resolve<Window>().ViewportPixelSize = new Size(500, 500);
			projectile = new Projectile(new Material(Shader.Position2DColorUV, "projectile"),
				Vector2D.Half, Resolve<ScreenSpace>().Viewport);
		}

		private Projectile projectile;

		[Test]
		public void CreateProjectileInScreen()
		{
			//projectile.Size = new Size(0.05f, 0.1f);
			projectile.Get<SimplePhysics.Data>().Velocity = Vector2D.Zero;
			Assert.IsTrue(projectile.IsActive);
		}

		[Test]
		public void MoveProjectileUp()
		{
			AdvanceTimeAndUpdateEntities();
			var newProjectileCenter = projectile.DrawArea.Center;
			Assert.AreNotEqual(Vector2D.Half, newProjectileCenter);
		}

		[Test]
		public void MoveProjectileOutsideBorder()
		{
			AdvanceTimeAndUpdateEntities();
			var newProjectileCenter = projectile.DrawArea.Center;
			Assert.AreNotEqual(Vector2D.Half, newProjectileCenter);
			AdvanceTimeAndUpdateEntities(5.0f);
			newProjectileCenter = projectile.DrawArea.Center;
			Assert.GreaterOrEqual(0.0f, newProjectileCenter.Y);
			Assert.IsFalse(projectile.IsActive);
		}

		[Test]
		public void DisposeProjectile()
		{
			projectile.Dispose();
			Assert.IsFalse(projectile.IsActive);
		}
	}
}