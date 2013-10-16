using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;

namespace ShadowShot
{
	public class Asteroid : Sprite, IDisposable
	{
		public Asteroid(Material asteroidMaterial, Rectangle drawArea, float borderY)
			: base(asteroidMaterial, drawArea)
		{
			this.borderY = borderY;
			RenderLayer = (int)Constants.RenderLayer.Asteroids;
			Add(new SimplePhysics.Data
			{
				Gravity = Vector2D.Zero,
				Velocity = new Vector2D(0.0f, 0.1f),
				RotationSpeed = 30.0f
			});
			Start<SimplePhysics.Move>();
			Start<MoveAndDisposeOnBorderCollision>();
		}

		private float borderY;

		private class MoveAndDisposeOnBorderCollision : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach(Asteroid asteroid in entities)
				if (ObjectHasCrossedScreenBorder(asteroid.DrawArea, asteroid.borderY))
					asteroid.IsActive = false;
			}

			private static bool ObjectHasCrossedScreenBorder(Rectangle objectArea, float borderLower)
			{
				return (objectArea.Top >= borderLower);
			}
		}

		public void Dispose()
		{
			Remove<SimplePhysics.Data>();
			Stop<SimplePhysics.Move>();
			Stop<MoveAndDisposeOnBorderCollision>();
			IsActive = false;
		}
	}
}