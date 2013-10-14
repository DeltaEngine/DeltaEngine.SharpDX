using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace DeltaEngine.Rendering3D.Particles.Tests
{
	public class Particle3DEmitterTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateCamera()
		{
			window = Resolve<Window>();
			window.BackgroundColor = Color.DarkGray;
			camera = (LookAtCamera)Camera.Current;
			camera.Position = Vector3D.One;
			logo = new Material(Shader.Position3DColorUV, "DeltaEngineLogo");
			spark = new Material(Shader.Position3DColorUV, "ParticleSpark");
			fire = new Material(Shader.Position3DColorUV, "ParticleFire");
			water = new Material(Shader.Position3DColorUV, "EffectsWaterRanged");
		}

		private Window window;
		private LookAtCamera camera;
		private Material logo;
		private Material spark;
		private Material fire;
		private Material water;

		[Test, CloseAfterFirstFrame]
		public void CreateMultiMaterialEmittersAndAdvanceTime()
		{
			new Particle3DPointEmitter(GetEmitterData(logo), Vector3D.Zero);
			new Particle3DPointEmitter(GetEmitterData(fire), Vector3D.Zero);
			AdvanceTimeAndUpdateEntities(0.3f);
		}

		[Test]
		public void PointEmitter()
		{
			new Grid3D(10);
			new Particle3DPointEmitter(GetEmitterData(logo), Vector3D.Zero);
		}

		private static ParticleEmitterData GetEmitterData(Material material, int maxParticles = 64,
			float lifeTime = 1.0f)
		{
			return new ParticleEmitterData
			{
				MaximumNumberOfParticles = maxParticles,
				SpawnInterval = lifeTime / maxParticles,
				LifeTime = lifeTime,
				Size =
					new RangeGraph<Size>(
						new List<Size>(new[] { new Size(0.1f), new Size(0.2f), new Size(0.1f) })),
				Color =
					new RangeGraph<Color>(
						new List<Color>(new[] { Color.Red, Color.Orange, Color.TransparentBlack })),
				Acceleration =
					new RangeGraph<Vector3D>(
						new List<Vector3D>(new[]
						{ Vector3D.UnitZ, new Vector3D(0.25f, 0.25f, 0.5f), new Vector3D(-0.25f, -0.25f, -4.0f) })),
				StartVelocity =
					new RangeGraph<Vector3D>(new Vector3D(-0.5f, -0.5f, 0.1f), new Vector3D(0.5f, 0.5f, 0.1f)),
				ParticleMaterial = material,
				BillboardMode = BillboardMode.CameraFacing
			};
		}

		[Test]
		public void PlaneEmitter()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero);
			emitterData.StartVelocity = new RangeGraph<Vector3D>(Vector3D.Zero);
			new Particle3DBoxEmitter(emitterData,
				new Range<Vector3D>(new Vector3D(-1.0f, -1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f)));
		}

		[Test]
		public void SphericalEmitter()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero);
			emitterData.StartVelocity = new RangeGraph<Vector3D>(Vector3D.Zero);
			new Particle3DSphericalEmitter(emitterData, Vector3D.Zero, 0.6f);
		}

		[Test]
		public void DifferentMaterials()
		{
			new Grid3D(10);
			new Particle3DPointEmitter(GetEmitterData(logo), new Vector3D(-0.5f, -0.5f, 0.0f));
			new Particle3DPointEmitter(GetEmitterData(spark), new Vector3D(-0.5f, 0.5f, 0.0f));
			new Particle3DPointEmitter(GetEmitterData(fire), new Vector3D(0.5f, -0.5f, 0.0f));
		}

		[Test]
		public void SquareEmitter()
		{
			new Grid3D(10);
			new Particle3DLineEmitter(GetEmitterData(logo),
				new Range<Vector3D>(new Vector3D(-1.0f, 1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f)));
			new Particle3DLineEmitter(GetEmitterData(logo),
				new Range<Vector3D>(new Vector3D(-1.0f, -1.0f, 0.0f), new Vector3D(1.0f, -1.0f, 0.0f)));
			new Particle3DLineEmitter(GetEmitterData(logo),
				new Range<Vector3D>(new Vector3D(-1.0f, -1.0f, 0.0f), new Vector3D(-1.0f, 1.0f, 0.0f)));
			new Particle3DLineEmitter(GetEmitterData(logo),
				new Range<Vector3D>(new Vector3D(1.0f, -1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f)));
		}

		[Test]
		public void SpawnOneBurst()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.SpawnInterval = 0.0f;
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			new Command(() => emitter.Spawn(64)).Add(new KeyTrigger(Key.Space));
		}

		[Test, CloseAfterFirstFrame]
		public void DisposingTwiceDoesNotError()
		{
			var emitter = new Particle3DPointEmitter(GetEmitterData(spark, 512), Vector3D.Zero);
			emitter.DisposeAfterSeconds(1);
			emitter.DisposeAfterSeconds(1);
		}

		[Test]
		public void SpawnOneTimedBurst()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.SpawnInterval = 0.0007f;
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			emitter.DisposeAfterSeconds(emitterData.LifeTime);
		}

		[Test]
		public void SmokeAndWind()
		{
			new Grid3D(10);
			window.BackgroundColor = new Color(40, 64, 20);
			var defaultForce = new RangeGraph<Vector3D>(Vector3D.Zero);
			var windForce = new RangeGraph<Vector3D>(new Vector3D(-0.5f, -0.01f, 0.0f),
				new Vector3D(-1.0f, 0.01f, 0.0f));
			var emitterData = GetEmitterData(spark, 256, 2.0f);
			emitterData.Color = new RangeGraph<Color>(Color.White, Color.Transparent(Color.DarkGray));
			emitterData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.2f));
			emitterData.Acceleration = defaultForce;
			emitterData.StartVelocity = new RangeGraph<Vector3D>(new Vector3D(0.0f, 0.0f, 0.35f),
				new Vector3D(0.1f, 0.1f, 0.1f));
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			new Command(() => emitter.SetAcceleration(windForce)).Add(new KeyTrigger(Key.Space));
			new Command(() => emitter.SetAcceleration(defaultForce)).Add(new KeyTrigger(Key.Space,
				State.Releasing));
		}

		[Test]
		public void Fire()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(fire, 512, 2.0f);
			emitterData.Color = new RangeGraph<Color>(new Color(16, 16, 16), new Color(255, 64, 64, 0));
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.UnitZ * 0.1f);
			emitterData.Size = new RangeGraph<Size>(new Size(0.2f), new Size(0.1f));
			emitterData.StartVelocity = new RangeGraph<Vector3D>(new Vector3D(0.0f, 0.0f, 0.3f),
				new Vector3D(0.1f, 0.1f, 0.1f));
			new Particle3DLineEmitter(emitterData,
				new RangeGraph<Vector3D>(new Vector3D(-0.1f, 0.0f, 0.0f), new Vector3D(0.1f, 0.0f, 0.0f)));
			new Particle3DLineEmitter(emitterData,
				new RangeGraph<Vector3D>(new Vector3D(0.0f, -0.1f, 0.0f), new Vector3D(0.0f, 0.1f, 0.0f)));
		}

		[Test, CloseAfterFirstFrame]
		public void SetForce()
		{
			var emitter = new Particle3DPointEmitter(GetEmitterData(spark, 512), Vector3D.Zero);
			var force = new RangeGraph<Vector3D>(Vector3D.One);
			emitter.SetAcceleration(force);
			Assert.AreEqual(force, emitter.EmitterData.Acceleration);
		}

		[Test, CloseAfterFirstFrame]
		public void TooManyParticlesThrowsError()
		{
			var emitterData = GetEmitterData(spark, ParticleEmitter.MaxParticles + 1);
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			Assert.Throws<ParticleEmitter.MaximumNumberOfParticlesExceeded>(() => emitter.Spawn());
			emitter.IsActive = false; //Have to get rid of it, since it would update further and crash
			emitterData.MaximumNumberOfParticles = 1; // So an exception isn't thrown later
		}

		[Test, CloseAfterFirstFrame]
		public void ParticlesUpdatingPosition()
		{
			Randomizer.Use(new FixedRandom());
			var emitter = new Particle3DPointEmitter(GetEmitterData(logo), Vector3D.Zero);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.IsTrue(
				emitter.particles[0].Position.IsNearlyEqual(new Vector3D(-0.03333334f, -0.03333334f,
					0.0025f)));
		}

		[Test, CloseAfterFirstFrame]
		public void ParticlesTrackingEmitterUpdatingPosition()
		{
			Randomizer.Use(new FixedRandom());
			var emitterData = GetEmitterData(logo);
			emitterData.DoParticlesTrackEmitter = true;
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			AdvanceTimeAndUpdateEntities(0.1f);
			emitter.Position = Vector3D.One;
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.IsTrue(
				emitter.particles[0].Position.IsNearlyEqual(new Vector3D(0.90025f, 0.90025f, 1.0145f)));
		}

		[Test]
		public void ParticleTracksEmitterAcrossScreenFor4Seconds()
		{
			new Grid3D(10);
			var emitter = new Particle3DPointEmitter(CreateTrackingParticleData(), Vector3D.Zero);
			emitter.Start<MoveAcrossScreen>();
			emitter.DisposeAfterSeconds(4);
		}

		private ParticleEmitterData CreateTrackingParticleData()
		{
			return new ParticleEmitterData
			{
				MaximumNumberOfParticles = 1,
				SpawnInterval = 0.001f,
				Size = new RangeGraph<Size>(new Size(0.1f)),
				Color = new RangeGraph<Color>(Color.White),
				ParticleMaterial = logo,
				DoParticlesTrackEmitter = true,
				BillboardMode = BillboardMode.CameraFacing
			};
		}

		private class MoveAcrossScreen : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (ParticleEmitter emitter in entities)
					emitter.Position += new Vector3D(Time.Delta / 2, 0.0f, 0.0f);
			}
		}

		[Test]
		public void ProjectileMovesAcrossScreenEmittingFire()
		{
			new Grid3D(10);
			var start = -3 * Vector3D.UnitX;
			var emitter = new Particle3DPointEmitter(CreateTrackingParticleData(), start);
			var emitter2 = new Particle3DPointEmitter(CreateFireExhaustParticleData(), start);
			emitter.Start<MoveAcrossScreen>();
			emitter2.Start<MoveAcrossScreen>();
		}

		private ParticleEmitterData CreateFireExhaustParticleData()
		{
			return new ParticleEmitterData
			{
				MaximumNumberOfParticles = 200,
				SpawnInterval = 0.01f,
				LifeTime = 2.0f,
				Size =
					new RangeGraph<Size>(
						new List<Size>(new[] { new Size(0.1f), new Size(0.2f), new Size(0.1f) })),
				Color =
					new RangeGraph<Color>(
						new List<Color>(new[] { Color.Red, Color.Orange, Color.TransparentBlack })),
				Acceleration = new RangeGraph<Vector3D>(-Vector3D.UnitZ / 5),
				StartVelocity = new RangeGraph<Vector3D>(-Vector3D.UnitX / 2, Vector3D.One / 20),
				ParticleMaterial = fire,
				BillboardMode = BillboardMode.CameraFacing
			};
		}

		[Test]
		public void FireOneBullet()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(water, 512, 2.0f);
			emitterData.SpawnInterval = 0.0f;
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
			emitterData.Color = new RangeGraph<Color>(new Color(255, 255, 255), new Color(255, 255, 255));
			emitterData.Size = new RangeGraph<Size>(new Size(0.5f), new Size(0.5f));
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			var enemy = new MockEnemy(new Vector3D(0, -3, 0), Size.Half, spark);
			new Command(() => //ncrunch: no coverage start
			{
				emitter.EmitterData.BillboardMode = BillboardMode.Ground;
				emitter.EmitterData.StartVelocity.Start = enemy.Position * 4.0f + enemy.direction * 0.5f;
				emitter.EmitterData.StartVelocity.End = Vector3D.Zero;
				var angle = MathExtensions.Atan2(enemy.Position.Y, enemy.Position.X);
				emitter.EmitterData.StartRotation = new RangeGraph<ValueRange>(new ValueRange(-angle, -angle),
					new ValueRange(-angle, -angle));
				emitter.Spawn();
			}).Add(new KeyTrigger(Key.Space));
			//ncrunch: no coverage end
		}

		[Test, CloseAfterFirstFrame]
		public void LoadParticle()
		{
			var data = ContentLoader.Load<ParticleEmitterData>("FireEmitter");
			Assert.IsNotNull(data.ParticleMaterial.DiffuseMap);
			Assert.AreEqual(256, data.MaximumNumberOfParticles);
			Assert.AreEqual(5.0f, data.LifeTime);
			Assert.AreEqual(0.1f, data.SpawnInterval);
			Assert.AreEqual(Color.Red, data.Color.Start);
			Assert.AreEqual(Color.Green, data.Color.End);
		}

		[TestCase(BillboardMode.FrontAxis), TestCase(BillboardMode.CameraFacing),
		TestCase(BillboardMode.Ground), TestCase(BillboardMode.UpAxis), Test]
		public void SetDifferentBillBoardModes(BillboardMode mode)
		{
			var emitterData = GetEmitterData(logo);
			emitterData.BillboardMode = mode;
			emitterData.DoParticlesTrackEmitter = true;
			new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void SetDifferentBlendMode()
		{
			var emitterData = GetEmitterData(logo);
			emitterData.ParticleMaterial.DiffuseMap.BlendMode = BlendMode.Additive;
			emitterData.DoParticlesTrackEmitter = true;
			new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void SwitchMaterialsOfParticles()
		{
			var emitterData = GetEmitterData(logo);
			emitterData.ParticleMaterial.DiffuseMap.BlendMode = BlendMode.Additive;
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			AdvanceTimeAndUpdateEntities();
			emitter.particles[0].Material = new Material(Shader.Position3DColorUV, "ParticleSpark");
			emitterData.ParticleMaterial.DiffuseMap.BlendMode = BlendMode.Additive;
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void EmittersFromSameDataAreStillIndependentlyChangeable()
		{
			var emitterData = GetEmitterData(logo);
			var emitterChanging = new ParticleEmitter(emitterData, Vector3D.Zero);
			var emitterStayingSame = new ParticleEmitter(emitterData, Vector3D.UnitX);
			emitterChanging.EmitterData.SpawnInterval += 0.2f;
			emitterChanging.EmitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.One);
			emitterChanging.EmitterData.Color = new RangeGraph<Color>(Color.Green, Color.Yellow);
			Assert.AreNotEqual(emitterStayingSame.EmitterData.SpawnInterval,
				emitterChanging.EmitterData.SpawnInterval);
			Assert.AreNotEqual(emitterStayingSame.EmitterData.Acceleration.Values,
				emitterChanging.EmitterData.Acceleration.Values);
			Assert.AreNotEqual(emitterStayingSame.EmitterData.Color.Values,
				emitterChanging.EmitterData.Color.Values);
		}
	}
}