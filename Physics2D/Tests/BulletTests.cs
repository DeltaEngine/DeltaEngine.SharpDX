using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Physics2D.Tests
{
	public class BulletTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void CreateBullet()
		{
			var bulletImpulse = new Vector2D(0.3f, 0.3f);
			const int BulletDamage = 10;
			var bullet = CreateBullet(bulletImpulse, BulletDamage);
			Assert.AreEqual(bulletImpulse / 1000000f, bullet.PhysicsBody.LinearVelocity);
			Assert.AreEqual(BulletDamage, bullet.Damage);
		}

		private Bullet CreateBullet(Vector2D impulse, int damage)
		{
			return new Bullet(Resolve<Physics>(), impulse,
				Rectangle.FromCenter(Vector2D.Half, new Size(0.1f)), damage);
		}
	}
}