using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Sprites.Tests;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Particles.Tests
{
	public class Particle3DEmitterTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateCamera()
		{
			window = Resolve<Window>();
			window.BackgroundColor = Color.DarkGray;
			camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One;
			logo = new Material(Shader.Position3DColorUv, "DeltaEngineLogo");
			spark = new Material(Shader.Position3DColorUv, "ParticleSpark");
			fire = new Material(Shader.Position3DColorUv, "ParticleFire");
			new PerformanceTests.FpsDisplay();
			RegisterCameraCommands();
		}

		private Window window;
		private LookAtCamera camera;
		private Material logo;
		private Material spark;
		private Material fire;

		public void RegisterCameraCommands()
		{
			new Command(Command.MoveLeft, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				camera.Position -= right * Time.Delta * 4;
			});
			new Command(Command.MoveRight, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				camera.Position += right * Time.Delta * 4;
			});
			new Command(Command.Click, () => { camera.Zoom(0.5f); });
			new Command(Command.RightClick, () => { camera.Zoom(-0.5f); });
			new Command(Command.MoveUp, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				var up = Vector3D.Cross(right, front);
				camera.Position += up * Time.Delta * 4;
			});
			new Command(Command.MoveDown, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				var up = Vector3D.Cross(right, front);
				camera.Position -= up * Time.Delta * 4;
			});
		}

		[Test, CloseAfterFirstFrame]
		public void CreateMultiMaterialEmittersAndAdvanceTime()
		{
			new Particle3DPointEmitter(GetEmitterData(logo), Vector3D.Zero);
			new Particle3DPointEmitter(GetEmitterData(fire), Vector3D.Zero);
			AdvanceTimeAndUpdateEntities(1.0f);
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
			};
		}

		[Test]
		public void PlaneEmitter()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
			emitterData.StartVelocity = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
			new Particle3DBoxEmitter(emitterData,
				new Range<Vector3D>(new Vector3D(-1.0f, -1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f)));
		}

		[Test]
		public void SphericalEmitter()
		{
			new Grid3D(10);
			var emitterData = GetEmitterData(spark, 512);
			emitterData.Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
			emitterData.StartVelocity = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
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
			new Command(() => emitter.SpawnBurst(64)).Add(new KeyTrigger(Key.Space));
		}

		[Test]
		public void SmokeAndWind()
		{
			new Grid3D(10);
			window.BackgroundColor = new Color(40, 64, 20);
			var defaultForce = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero);
			var windForce = new RangeGraph<Vector3D>(new Vector3D(-0.5f, -0.01f, 0.0f),
				new Vector3D(-1.0f, 0.01f, 0.0f));
			var emitterData = GetEmitterData(spark, 256, 2.0f);
			emitterData.Color = new RangeGraph<Color>(Color.White, new Color(Color.DarkGray, 0));
			emitterData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.2f));
			emitterData.Acceleration = defaultForce;
			emitterData.StartVelocity = new RangeGraph<Vector3D>(new Vector3D(0.0f, 0.0f, 0.35f),
				new Vector3D(0.1f, 0.1f, 0.1f));
			var emitter = new Particle3DPointEmitter(emitterData, Vector3D.Zero);
			new Command(() => emitter.SetForce(windForce)).Add(new KeyTrigger(Key.Space));
			new Command(() => emitter.SetForce(defaultForce)).Add(
				new KeyTrigger(Key.Space, State.Releasing));
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
			new Particle3DLineEmitter(emitterData, new RangeGraph<Vector3D>(new Vector3D(-0.1f, 0.0f, 0.0f),
				new Vector3D(0.1f, 0.0f, 0.0f)));
			new Particle3DLineEmitter(emitterData, new RangeGraph<Vector3D>(new Vector3D(0.0f, -0.1f, 0.0f),
				new Vector3D(0.0f, 0.1f, 0.0f)));
		}
	}
}