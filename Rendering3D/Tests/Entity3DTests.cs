using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Tests
{
	public class Entity3DTests
	{
		[SetUp]
		public void InitializeEntitiesRunner()
		{
			entities = new MockEntitiesRunner(typeof(MockUpdateBehavior));
		}

		private EntitiesRunner entities;

		[TearDown]
		public void DisposeEntitiesRunner()
		{
			entities.Dispose();
		}

		[Test]
		public void CreateEntity3D()
		{
			var entity = new Entity3D(Vector3D.Zero);
			Assert.AreEqual(Vector3D.Zero, entity.Position);
			Assert.AreEqual(Quaternion.Identity, entity.Orientation);
			Assert.AreEqual(Visibility.Show, entity.Visibility);
		}

		[Test]
		public void CreateEntity3DPositionAndOrientation()
		{
			var position = new Vector3D(10.0f, -3.0f, 27.0f);
			var orientation = Quaternion.Identity;
			var entity = new Entity3D(position, orientation);
			Assert.AreEqual(position, entity.Position);
			Assert.AreEqual(orientation, entity.Orientation);
		}

		[Test]
		public void SetAndGetEntity3DComponentsDirectly()
		{
			var entity = new Entity3D(Vector3D.Zero);
			entity.Set(Vector3D.One);
			Assert.AreEqual(Vector3D.One, entity.Get<Vector3D>());
			entity.Set(Quaternion.Identity);
			Assert.AreEqual(Quaternion.Identity, entity.Get<Quaternion>());
		}

		[Test]
		public void CannotAddTheSameTypeOfComponentTwice()
		{
			var entity = new Entity3D(Vector3D.Zero);
			Assert.Throws<Entity.ComponentOfTheSameTypeAddedMoreThanOnce>(() => entity.Add(Vector3D.One));
		}

		[Test]
		public void SetPositionProperty()
		{
			var entity = new Entity3D(Vector3D.Zero) { Position = Vector3D.One };
			Assert.AreEqual(Vector3D.One, entity.Position);
		}

		[Test]
		public void SetOrientationProperty()
		{
			var entity = new Entity3D(Vector3D.Zero) { Orientation = Quaternion.Identity };
			Assert.AreEqual(Quaternion.Identity, entity.Orientation);
		}

		[Test]
		public void SetVisibilityProperty()
		{
			var entity = new Entity3D(Vector3D.Zero) { Visibility = Visibility.Hide };
			Assert.AreEqual(Visibility.Hide, entity.Visibility);
		}
	}
}
