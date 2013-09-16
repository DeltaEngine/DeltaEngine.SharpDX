using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Particles;
using DeltaEngine.Rendering.Sprites;

namespace $safeprojectname$
{
	public static class Effects
	{
		public static Sprite CreateArrow(Point start, Point target)
		{
			var material = new Material(Shader.Position2DColorUv, "Arrow");
			var newSprite = new Sprite(material, CalculateArrowDrawArea(material, start, target));
			newSprite.Rotation = target.RotationTo(start);
			return newSprite;
		}

		private static Rectangle CalculateArrowDrawArea(Material material, Point start, Point target)
		{
			start += Point.Normalize(start.DirectionTo(target)) * 0.033f;
			target -= Point.Normalize(start.DirectionTo(target)) * 0.033f;
			var distance = start.DistanceTo(target);
			var size = new Size(distance, distance / material.DiffuseMap.PixelSize.AspectRatio);
			return Rectangle.FromCenter((start + target) / 2, size * GameLogic.ArrowSize);
		}

		public static ParticleEmitter CreateDeathEffect(Point position)
		{
			var material = new Material(Shader.Position2DColorUv, "DeathSkull");
			var deathEffect = new ParticleEmitterData {
				ParticleMaterial = material,
				MaximumNumberOfParticles = 1,
				SpawnInterval = 0,
				Size = new RangeGraph<Size>(new Size(GameLogic.DeathSkullSize), new 
					Size(GameLogic.DeathSkullSize - 0.005f)),
				Acceleration = new RangeGraph<Vector>(new Point(0, -0.04f), new Point(0, -0.04f)),
				LifeTime = 2f,
				StartVelocity = new RangeGraph<Vector>(Point.Zero, new Point(0.01f, 0.01f))
			};
			return new Particle2DEmitter(deathEffect, position);
		}

		public static ParticleEmitter CreateHitEffect(Point position)
		{
			var material = new Material(Shader.Position2DColorUv, "Hit");
			var deathEffect = new ParticleEmitterData {
				ParticleMaterial = material,
				MaximumNumberOfParticles = 1,
				SpawnInterval = 0,
				Size = new RangeGraph<Size>(new Size(0.06f), new Size(0.09f)),
				LifeTime = 0.5f
			};
			return new Particle2DEmitter(deathEffect, position);
		}

		public static ParticleEmitter CreateSparkleEffect(Team team, Point position, int sparkles)
		{
			return null;
		}
	}
}