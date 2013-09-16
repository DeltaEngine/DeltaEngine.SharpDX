using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Tests.Entities
{
	internal class DrawableEntityTest
	{
		[SetUp]
		public void InitializeEntitiesRunner()
		{
			new MockEntitiesRunner(typeof(Draw), typeof(DrawToCopyArrayListLenght));
			new MockEntity();
		}

		[Test]
		public void TryToGetListWillThrowExeptionIfNoListsAvailable()
		{
			var draw = new MockDrawableEntity();
			draw.OnDraw<Draw>();
			EntitiesRunner.Current.UpdateAndDrawAllEntities(() => { });
		}

		private class Draw : DrawBehavior
		{
			void DrawBehavior.Draw(IEnumerable<DrawableEntity> entities)
			{
				foreach (var drawableEntity in entities)
					ThrowExeptionsWhenInterpolationElementsAreNotFound(drawableEntity);
			}

			private static void ThrowExeptionsWhenInterpolationElementsAreNotFound(
				DrawableEntity drawableEntity)
			{
				Assert.Throws<DrawableEntity.ListWithLerpElementsForInterpolationWasNotFound>(
					() => { drawableEntity.GetInterpolatedList<MockLerp>(); });
				Assert.Throws<DrawableEntity.ArrayWithLerpElementsForInterpolationWasNotFound>(
					() => { drawableEntity.GetInterpolatedArray<MockLerp>(); });
			}
		}

		private class MockLerp : Lerp<MockLerp>
		{
			public MockLerp Lerp(MockLerp other, float interpolation)
			{
				return new MockLerp();
			}
		}

		[Test]
		public void CHangeLenghtToCopyLimit()
		{
			var draw = new MockDrawableEntity();
			draw.OnDraw<DrawToCopyArrayListLenght>();
			var mockLerp = new MockLerp().Lerp(new MockLerp(), 1);
			var lerp = new MockLerp[3];
			lerp[0] = mockLerp;
			lerp[1] = mockLerp;
			lerp[2] = mockLerp;
			draw.Add(lerp);
			EntitiesRunner.Current.UpdateAndDrawAllEntities(() => { });
		}

		private class DrawToCopyArrayListLenght : DrawBehavior
		{
			void DrawBehavior.Draw(IEnumerable<DrawableEntity> entities)
			{
				foreach (var drawableEntity in entities)
					drawableEntity.GetInterpolatedArray<MockLerp>(2);
			}
		}
	}
}