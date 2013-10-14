using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Particles;
using NUnit.Framework;

namespace DeltaEngine.GameLogic.Tests
{
	internal class ParticleSystemTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateParticleSystem()
		{
			particleSystem = new ParticleSystem();
		}

		private ParticleSystem particleSystem;

		[Test, CloseAfterFirstFrame]
		public void NewSystemHasListInitialized()
		{
			Assert.IsNotNull(particleSystem.AttachedEmitters);
		}

		[Test, CloseAfterFirstFrame]
		public void AttachingEmitterSetsPositionAndRotation()
		{
			particleSystem.Position = Vector3D.UnitY;
			particleSystem.Rotation = Quaternion.FromAxisAngle(Vector3D.UnitZ, 45);
			var emitter = CreateAndAttachEmitter(Vector3D.UnitX);
			Assert.AreEqual(particleSystem.Position, emitter.Position);
			Assert.AreEqual(particleSystem.Rotation, emitter.Rotation);
		}

		private ParticleEmitter CreateAndAttachEmitter(Vector3D emitterPosition)
		{
			var textureData = new ImageCreationData(new Size(32.0f));
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position3DColorUV),
				ContentLoader.Create<Image>(textureData));
			var emitterData = new ParticleEmitterData { ParticleMaterial = material };
			var emitter = new ParticleEmitter(emitterData, emitterPosition);
			particleSystem.AttachEmitter(emitter);
			return emitter;
		}

		[Test, CloseAfterFirstFrame]
		public void DisposeEmitterDeactivates()
		{
			var emitterAlpha = CreateAndAttachEmitter(Vector3D.Zero);
			var emitterBeta = CreateAndAttachEmitter(Vector3D.UnitY);
			particleSystem.DisposeEmitter(1);
			particleSystem.DisposeEmitter(emitterAlpha);
			particleSystem.DisposeEmitter(emitterAlpha);
			Assert.IsFalse(emitterAlpha.IsActive);
			Assert.IsFalse(emitterBeta.IsActive);
			Assert.AreEqual(0, particleSystem.AttachedEmitters.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void RemoveEmitterJustDeattaches()
		{
			var emitterAlpha = CreateAndAttachEmitter(Vector3D.Zero);
			var emitterBeta = CreateAndAttachEmitter(Vector3D.UnitY);
			particleSystem.RemoveEmitter(0);
			particleSystem.RemoveEmitter(emitterBeta);
			Assert.AreEqual(0,particleSystem.AttachedEmitters.Count);
			Assert.IsTrue(emitterAlpha.IsActive);
			Assert.IsTrue(emitterBeta.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void DisposingSystemDisposesAllEmitters()
		{
			var emitterAlpha = CreateAndAttachEmitter(Vector3D.Zero);
			var emitterBeta = CreateAndAttachEmitter(Vector3D.UnitY);
			particleSystem.DisposeSystem();
			Assert.IsFalse(emitterAlpha.IsActive);
			Assert.IsFalse(emitterBeta.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void SettingPositionAndRotationOfSystemAlsoSetsForEmitters()
		{
			particleSystem.Position = Vector3D.One;
			var emitter = CreateAndAttachEmitter(Vector3D.Zero);
			particleSystem.Position = Vector3D.UnitY;
			particleSystem.Rotation = Quaternion.FromAxisAngle(Vector3D.UnitZ, 50);
			Assert.AreEqual(particleSystem.Position, emitter.Position);
			Assert.AreEqual(particleSystem.Rotation, emitter.Rotation);
		}
	}
}