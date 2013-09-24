using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Platforms.Mocks;
using DeltaEngine.Rendering2D.Sprites.Tests;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Particles.Tests
{
	internal class Particle2DEmitterTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void ShowFps()
		{
			new PerformanceTests.FpsDisplay();
		}

		[Test]
		public void CreateEmitterAndKeepRunning()
		{
			CreateDataAndEmitter(512, 0.01f, 5);
			emitter.Position = new Vector2D(0.5f, 0.4f);
		}

		private void CreateDataAndEmitter(int maxParticles = 1, float spawnInterval = 0.01f,
			float lifeTime = 0.2f)
		{
			emitterData = new ParticleEmitterData
			{
				MaximumNumberOfParticles = maxParticles,
				SpawnInterval = spawnInterval,
				LifeTime = lifeTime,
				Size = new RangeGraph<Size>(new Size(0.02f), new Size(0.06f)),
				Color =
					new RangeGraph<Color>(
						new List<Color>(new[]
						{ Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple })),
				Acceleration = new RangeGraph<Vector3D>(new Vector3D(0, 0.1f, 0), new Vector3D(0.2f, 0.1f, 0)),
				StartVelocity =
					new RangeGraph<Vector3D>(new Vector3D(-0.05f, -0.1f, 0), new Vector3D(0.05f, -0.015f, 0)),
				StartRotation =
					new RangeGraph<ValueRange>(new ValueRange(20f, 100f), new ValueRange(60f, 300f)),
				RotationSpeed = new RangeGraph<ValueRange>(new ValueRange(0, 10), new ValueRange(55, 75)),
				ParticleMaterial = new Material(Shader.Position2DColorUv, "DeltaEngineLogo"),
				StartPosition = new RangeGraph<Vector3D>(new Vector2D(-0.1f, -0.1f), new Vector2D(0.1f, 0.1f))
			};
			emitter = new Particle2DEmitter(emitterData, Vector2D.Half);
		}

		private ParticleEmitterData emitterData;
		private Particle2DEmitter emitter;

		[Test]
		public void CreateEmitterWithJustOneParticle()
		{
			CreateDataAndEmitter(1, 0.01f, 5);
			emitter.Position = new Vector2D(0.5f, 0.7f);
			RunAfterFirstFrame(() =>
			{
				//Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame);
				//Assert.AreEqual(4, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame);
			});
		}

		[Test, CloseAfterFirstFrame]
		public void AdvanceCreatingOneParticle()
		{
			CreateDataAndEmitter();
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(1, emitter.NumberOfActiveParticles);
		}

		[Test]
		public void MultipleEmittersShallNotInterfere()
		{
			CreateDataAndEmitter(12, 0.1f, 5);
			var data = new ParticleEmitterData
			{
				MaximumNumberOfParticles = 12,
				SpawnInterval = 0.4f,
				LifeTime = 2,
				Size = new RangeGraph<Size>(new Size(0.03f), new Size(0.03f)),
				Color = new RangeGraph<Color>(Color.Gray, Color.Gray),
				Acceleration = new RangeGraph<Vector3D>(-Vector2D.UnitY, -Vector2D.UnitY),
				StartVelocity = new RangeGraph<Vector3D>(new Vector2D(0.3f, -0.1f), new Vector2D(0.3f, -0.1f)),
				ParticleMaterial = new Material(Shader.Position2DColorUv, "DeltaEngineLogo")
			};
			new Particle2DEmitter(data, Vector2D.Half);
		}

		[Test, CloseAfterFirstFrame]
		public void ParticlesUpdatingPosition()
		{
			CreateDataAndEmitter();
			if (resolver.GetType() == typeof(MockResolver))
				AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(emitter.Position, emitter.particles[0].Position);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateParticleEmitterAddingDefaultComponents()
		{
			var emptyMaterial = new Material(Shader.Position2DColor, "");
			new Particle2DEmitter(new ParticleEmitterData { ParticleMaterial = emptyMaterial },
				Vector2D.Zero);
		}

		[Test]
		public void CreateEmitterAndKeepRunningWithAnimation()
		{
			var newEmitter = new Particle2DEmitter(CreateDataAndEmitterWithAnimation("ImageAnimation"),
				Vector2D.Half);
			newEmitter.Position = new Vector2D(0.5f, 0.7f);
		}

		private ParticleEmitterData CreateDataAndEmitterWithAnimation(string contentName)
		{
			emitterData = new ParticleEmitterData
			{
				MaximumNumberOfParticles = 512,
				SpawnInterval = 0.1f,
				LifeTime = 5f,
				Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.10f)),
				Color = new RangeGraph<Color>(Color.White, Color.White),
				Acceleration = new RangeGraph<Vector3D>(new Vector2D(0, 0.1f), new Vector2D(0, 0.1f)),
				StartVelocity = new RangeGraph<Vector3D>(new Vector2D(0, -0.3f), new Vector2D(0.05f, 0.01f)),
				ParticleMaterial = new Material(Shader.Position2DColorUv, contentName)
			};
			return emitterData;
		}

		[Test]
		public void CreateEmitterAndKeepRunningWithSpriteSheetAnimation()
		{
			emitterData = CreateDataAndEmitterWithAnimation("EarthSpriteSheet");
			emitter = new Particle2DEmitter(emitterData, Vector2D.Half);
			emitter.Position = new Vector2D(0.5f, 0.7f);
		}

		[Test]
		public void CreateRotatedParticles()
		{
			emitterData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			emitterData.StartRotation = new RangeGraph<ValueRange>(new ValueRange(45, 50),
				new ValueRange(65, 70));
			emitterData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.05f));
			emitter = new Particle2DEmitter(emitterData, Vector2D.Half);
			emitter.Position = new Vector2D(0.5f, 0.7f);
		}

		[Test]
		public void CreateRotatingParticles()
		{
			emitterData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			emitterData.StartRotation = new RangeGraph<ValueRange>(new ValueRange(0, 5),
				new ValueRange(45, 50));
			emitterData.RotationSpeed = new RangeGraph<ValueRange>(new ValueRange(300.0f, 320.0f),
				new ValueRange(50f, 80f));
			emitterData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.05f));
			emitter = new Particle2DEmitter(emitterData, Vector2D.Half);
			emitter.Position = new Vector2D(0.5f, 0.7f);
		}

		[Test, CloseAfterFirstFrame]
		public void SpawnSingleBursts()
		{
			CreateDataAndEmitter(512, 0.01f, 5);
			emitter.SpawnBurst(20);
		}

		[Test, CloseAfterFirstFrame]
		public void DisposeEmitterAfterSetTime()
		{
			emitterData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			emitter = new Particle2DEmitter(emitterData, Vector2D.Half);
			emitter.DisposeAfterSeconds(0.2f);
			AdvanceTimeAndUpdateEntities(0.25f);
			Assert.IsFalse(emitter.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void ParticleWithNoMaterialThrowsException()
		{
			emitterData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			emitterData.ParticleMaterial = null;
			Assert.Throws<ParticleEmitter.UnableToCreateWithoutMaterial>(
				() => new Particle2DEmitter(emitterData, new Vector2D(0.5f, 0.5f)));
		}

		[Test]
		public void SpawnSeveralParticlesPerInterval()
		{
			CreateDataAndEmitter(128, 2.2f, 2f);
			emitterData.ParticlesPerSpawn.Start = new ValueRange(64, 128);
			emitterData.StartPosition = new RangeGraph<Vector3D>(Vector3D.UnitX * -0.4f,
				Vector3D.UnitX * 0.4f);
		}
	}
}