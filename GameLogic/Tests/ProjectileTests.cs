using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.GameLogic.Tests
{
	class ProjectileTests
	{
		[SetUp]
		public void CreateProjectileAndAddParticleSystem()
		{
			projectile = new Projectile();
		}

		private Projectile projectile;

		[Test, Ignore]
		public void PositionAndRotationOfProjectileAlsoModifiesParticleSystem()
		{
			projectile.Children[0].PositionRelativeToParent = Vector3D.UnitX;
			projectile.Rotation = Quaternion.FromAxisAngle(-Vector3D.UnitZ, 90);
			projectile.Position = Vector3D.UnitY;
			Assert.AreEqual(Quaternion.FromAxisAngle(-Vector3D.UnitZ, 90), projectile.Children[0].Rotation);
			Assert.AreEqual(0, projectile.Children[0].Position.X, 0.0001f);
			Assert.AreEqual(0, projectile.Children[0].Position.Y, 0.0001f);
			Assert.AreEqual(0, projectile.Children[0].Position.Z, 0.0001f);
		}

		[Test]
		public void WithoutParentLocalEqualsGlobal()
		{
			projectile.UpdateAbsolutePositionAndRotationFromParent();
			Assert.AreEqual(Quaternion.Identity, projectile.Rotation);
			Assert.AreEqual(projectile.PositionRelativeToParent, projectile.Position);
		}
	}
}