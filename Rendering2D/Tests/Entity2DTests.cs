using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Tests
{
	public class Entity2DTests
	{
		[SetUp]
		public void InitializeEntityRunner()
		{
			new MockEntitiesRunner(typeof(MockUpdateBehavior), typeof(MockDrawBehavior));
		}

		private class MockDrawBehavior : DrawBehavior
		{
			public void Draw(IEnumerable<DrawableEntity> entities) {}
		}

		[Test]
		public void CreateEntity2D()
		{
			var entity = new Entity2D(DoubleSizedRectangle) { Color = Color.Green, Rotation = 15 };
			Assert.AreEqual(DoubleSizedRectangle, entity.DrawArea);
			Assert.AreEqual(Color.Green, entity.Color);
			Assert.AreEqual(15, entity.Rotation);
			Assert.AreEqual(DrawableEntity.DefaultRenderLayer, entity.RenderLayer);
			Assert.AreEqual(Vector2D.One, entity.Center);
			Assert.AreEqual(new Size(2, 2), entity.Size);
		}

		private static readonly Rectangle DoubleSizedRectangle = new Rectangle(0, 0, 2, 2);

		[Test]
		public void AddNewComponent()
		{
			var entity = new Entity2D(Rectangle.Zero);
			Assert.AreEqual(Rectangle.Zero, entity.DrawArea);
			Assert.AreEqual(Color.White, entity.Color);
			Assert.AreEqual(0, entity.NumberOfComponents);
			entity.Add(Size.Zero);
			Assert.AreEqual(1, entity.NumberOfComponents);
		}

		[Test]
		public void SetDrawAreaProperties()
		{
			var entity = new Entity2D(Rectangle.One)
			{
				Color = Color.Blue,
				Center = Vector2D.One,
				Size = new Size(2)
			};
			Assert.AreEqual(DoubleSizedRectangle, entity.DrawArea);
			entity.DrawArea = new Rectangle(-1, -1, 2, 2);
			Assert.AreEqual(Vector2D.Zero, entity.Center);
			entity.TopLeft = Vector2D.Zero;
			Assert.AreEqual(Vector2D.Zero, entity.TopLeft);
			Assert.AreEqual(DoubleSizedRectangle, entity.DrawArea);
		}

		[Test]
		public void SetColorRotationAndRenderLayerProperties()
		{
			var entity = new Entity2D(Rectangle.One) { Color = Color.Blue };
			entity.Color = Color.Teal;
			Assert.AreEqual(Color.Teal, entity.Color);
			entity.Alpha = 0.5f;
			Assert.AreEqual(0.5f, entity.Alpha, 0.05f);
			entity.Rotation = MathExtensions.Pi;
			Assert.AreEqual(MathExtensions.Pi, entity.Rotation);
			entity.RenderLayer = 10;
			Assert.AreEqual(10, entity.RenderLayer);
			entity.RenderLayer = 1;
			Assert.AreEqual(1, entity.RenderLayer);
		}

		[Test]
		public void SetAndGetEntity2DComponentsDirectly()
		{
			var entity = new Entity2D(DoubleSizedRectangle) { Color = Color.Red };
			entity.Set(Color.Green);
			Assert.AreEqual(Color.Green, entity.Get<Color>());
			entity.Set(Rectangle.One);
			Assert.AreEqual(Rectangle.One, entity.Get<Rectangle>());
			entity.Set(5.0f);
			Assert.AreEqual(5.0f, entity.Get<float>());
			entity.Set(Vector2D.One);
			Assert.AreEqual(Vector2D.One, entity.Get<Vector2D>());
			entity.RenderLayer = -10;
			Assert.AreEqual(-10, entity.RenderLayer);
		}

		[Test]
		public void CannotAddTheSameTypeOfComponentTwice()
		{
			var entity = new Entity2D(Rectangle.Zero);
			Assert.Throws<Entity.ComponentOfTheSameTypeAddedMoreThanOnce>(() => entity.Add(Rectangle.One));
		}

		[Test]
		public void LastColorAddsColorComponentIfNotAddedBefore()
		{
			var entity = new Entity2D(Rectangle.Zero);
			entity.LastColor = Color.Red;
			Assert.AreEqual(Color.Red, entity.LastColor);
		}

		[Test]
		public void LastColorReplacesColorComponentValue()
		{
			var entity = new Entity2D(Rectangle.Zero) { Color = Color.Red };
			Assert.AreEqual(Color.Red, entity.LastColor);
			entity.LastColor = Color.Blue;
			Assert.AreEqual(Color.Blue, entity.LastColor);
		}

		[Test]
		public void GetComponentValuesInDrawState()
		{
			var entity = new Entity2D(Rectangle.One)
			{
				Color = Color.Red,
				Rotation = 1.0f,
				RotationCenter = Vector2D.One
			};
			EntitiesRunner.Current.State = UpdateDrawState.Draw;
			Assert.AreEqual(entity.DrawArea, entity.Get<Rectangle>());
			Assert.AreEqual(entity.Color, entity.Get<Color>());
			Assert.AreEqual(entity.Rotation, entity.Get<float>());
			Assert.AreEqual(entity.RotationCenter, entity.Get<Vector2D>());
		}

		[Test]
		public void SaveAndLoadFromMemoryStream()
		{
			var entity = new Entity2D(Rectangle.HalfCentered);
			entity.OnDraw<MockDrawBehavior>();
			var data = BinaryDataExtensions.SaveToMemoryStream(entity);
			byte[] savedBytes = data.ToArray();
			int bytesForName = "Entity2D".Length + 1;
			const int VersionNumberBytes = 4;
			int componentBytes = 1 + "Rectangle".Length + 1 + 16 + "Visibility".Length + 1 + 4 + 1;
			const int BehaviorBytes = 27;
			Assert.AreEqual(bytesForName + VersionNumberBytes + componentBytes + BehaviorBytes,
				savedBytes.Length);
			var loadedEntity = data.CreateFromMemoryStream() as Entity2D;
			Assert.AreEqual(0, loadedEntity.NumberOfComponents);
			Assert.IsTrue(loadedEntity.IsActive);
			Assert.AreEqual(Rectangle.HalfCentered, loadedEntity.DrawArea);
			Assert.AreEqual(1, loadedEntity.GetDrawBehaviors().Count);
			Assert.AreEqual("MockDrawBehavior",
				loadedEntity.GetDrawBehaviors()[0].GetShortNameOrFullNameIfNotFound());
		}

		[Test]
		public void RotatedDrawAreaContainsWithNoRotation()
		{
			var entity = new Entity2D(new Rectangle(0.4f, 0.4f, 0.2f, 0.1f));
			Assert.IsTrue(entity.RotatedDrawAreaContains(new Vector2D(0.45f, 0.45f)));
			Assert.IsTrue(entity.RotatedDrawAreaContains(new Vector2D(0.55f, 0.45f)));
			Assert.IsFalse(entity.RotatedDrawAreaContains(new Vector2D(0.55f, 0.55f)));
		}

		[Test]
		public void RotatedDrawAreaContainsRotatedAroundItsCenter()
		{
			var entity = new Entity2D(new Rectangle(0.4f, 0.4f, 0.2f, 0.1f)) { Rotation = 90 };
			Assert.IsTrue(entity.RotatedDrawAreaContains(new Vector2D(0.5f, 0.45f)));
			Assert.IsTrue(entity.RotatedDrawAreaContains(new Vector2D(0.55f, 0.55f)));
			Assert.IsFalse(entity.RotatedDrawAreaContains(new Vector2D(0.45f, 0.45f)));
		}

		[Test]
		public void RotatedDrawAreaContainsRotatedAroundTheScreenCenter()
		{
			var entity = new Entity2D(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f))
			{
				Rotation = 180,
				RotationCenter = Vector2D.Half
			};
			Assert.IsFalse(entity.RotatedDrawAreaContains(new Vector2D(0.15f, 0.15f)));
			Assert.IsTrue(entity.RotatedDrawAreaContains(new Vector2D(0.85f, 0.85f)));
			EntitiesRunner.Current.UpdateAndDrawAllEntities(() => {});
		}
	}
}