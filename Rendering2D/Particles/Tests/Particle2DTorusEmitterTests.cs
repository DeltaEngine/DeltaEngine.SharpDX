using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Particles.Tests
{
	public class Particle2DTorusEmitterTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUpTorusEmitter()
		{
			emitterData = new ParticleEmitterData
			{
				MaximumNumberOfParticles = 50,
				SpawnInterval = 0.01f,
				LifeTime = 0.5f,
				StartVelocity =
					new RangeGraph<Vector3D>(new Vector3D(-0.05f, -0.1f, 0), new Vector3D(0.05f, -0.015f, 0)),
				Size = new RangeGraph<Size>(new Size(0.01f)),
				Color = new RangeGraph<Color>(Color.White),
				ParticleMaterial = new Material(Shader.Position2DColorUV, "DeltaEngineLogo")
			};
			emitter = new Particle2DTorusEmitter(emitterData, Vector2D.Half, 0.1f);
		}

		private ParticleEmitterData emitterData;
		private Particle2DTorusEmitter emitter;

		[Test]
		public void CheckTorusEmitterWithEscapingParticlesFromCenter()
		{
			emitter.ParticleMovement = TypeOfMovement.Escaping;
			emitter.Update();
			Assert.AreEqual(TypeOfMovement.Escaping, emitter.ParticleMovement);
		}

		[Test]
		public void CheckTorusEmitterWithRoundingParticles()
		{
			emitter.EmitterData.StartVelocity = new RangeGraph<Vector3D>(new Vector3D(-0.55f, -0.1f, 0),
				new Vector3D(0.55f, -0.015f, 0));
			emitter.ParticleMovement = TypeOfMovement.AroundCenter;
			emitter.Update();
			Assert.AreEqual(TypeOfMovement.AroundCenter, emitter.ParticleMovement);
		}

		[Test]
		public void CheckTorusEmitterWithNormalParticles()
		{
			emitter.Update();
			Assert.AreEqual(TypeOfMovement.Normal, emitter.ParticleMovement);
		}
	}
}
