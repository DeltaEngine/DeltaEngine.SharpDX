using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Particles;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	//ncrunch: no coverage start
	/// <summary>
	/// Game object representing the projectiles fired by the player
	/// </summary>
	public class Projectile : Entity2D
	{
		public Projectile(Vector2D startPosition, float angle)
			: base(Rectangle.FromCenter(startPosition, new Size(.02f)))
		{
			Rotation = angle;
			RenderLayer = (int)AsteroidsRenderLayer.Rockets;
			missileAndTrails =
				new ParticleSystem(ContentLoader.Load<ParticleSystemData>("MissileEffect"));
			missileAndTrails.AttachedEmitters[0].EmitterData.DoParticlesTrackEmitter = true;
			missileAndTrails.AttachedEmitters[1].EmitterData.DoParticlesTrackEmitter = true;
			foreach (var emitter in missileAndTrails.AttachedEmitters)
				emitter.EmitterData.StartRotation =
					new RangeGraph<ValueRange>(new ValueRange(Rotation, Rotation),
						new ValueRange(Rotation, Rotation));
			Add(new SimplePhysics.Data
			{
				Gravity = Vector2D.Zero,
				Velocity =
					new Vector2D(MathExtensions.Sin(angle) * ProjectileVelocity,
						-MathExtensions.Cos(angle) * ProjectileVelocity)
			});
			Start<MoveAndDisposeOnBorderCollision>();
		}

		private const float ProjectileVelocity = .5f;
		private readonly ParticleSystem missileAndTrails;

		public void Dispose()
		{
			missileAndTrails.DisposeSystem();
			IsActive = false;
		}

		private class MoveAndDisposeOnBorderCollision : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Projectile projectile in entities.OfType<Projectile>())
				{
					projectile.missileAndTrails.Position = new Vector3D(projectile.Center);
					projectile.missileAndTrails.Rotation = Quaternion.FromAxisAngle(Vector3D.UnitZ,
						projectile.Rotation);
					projectile.DrawArea = CalculateFutureDrawArea(projectile, Time.Delta);
					if (ObjectHasCrossedScreenBorder(projectile.DrawArea, ScreenSpace.Current.Viewport))
						projectile.Dispose();
				}
			}

			private static Rectangle CalculateFutureDrawArea(Projectile projectile, float deltaT)
			{
				return
					new Rectangle(
						projectile.DrawArea.TopLeft + projectile.Get<SimplePhysics.Data>().Velocity * deltaT,
						projectile.DrawArea.Size);
			}

			private static bool ObjectHasCrossedScreenBorder(Rectangle objectArea, Rectangle borders)
			{
				return (objectArea.Right <= borders.Left || objectArea.Left >= borders.Right ||
					objectArea.Bottom <= borders.Top || objectArea.Top >= borders.Bottom);
			}
		}
	}

	//ncrunch: no coverage end
}