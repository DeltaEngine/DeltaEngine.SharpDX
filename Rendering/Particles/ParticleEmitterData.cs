using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Sprites;

namespace DeltaEngine.Rendering.Particles
{
	/// <summary>
	/// Data for ParticleEmitter, usually created and saved in the Editor.
	/// </summary>
	public class ParticleEmitterData : ContentData
	{
		protected ParticleEmitterData(string contentName)
			: base(contentName) { }

		public ParticleEmitterData()
			: base("<GeneratedParticleEmitterData>")
		{
			StartVelocity = new RangeGraph<Vector>(Vector.Zero, Vector.Zero);
			Acceleration = new RangeGraph<Vector>(Vector.Zero, Vector.Zero);
			Size = new RangeGraph<Size>(new Size(0.1f), new Size(0.1f));
			Color = new RangeGraph<Color>(Datatypes.Color.White, Datatypes.Color.White);
			StartPosition = new RangeGraph<Vector>(Vector.Zero, Vector.Zero);
			StartRotation = new RangeGraph<ValueRange>();
			RotationSpeed = new RangeGraph<ValueRange>();
			particlesPerSpawn = new RangeGraph<ValueRange>(new ValueRange(1,1), new ValueRange(1,1));
		}

		protected override void DisposeData() { }

		public float SpawnInterval { get; set; }
		public float LifeTime { get; set; }
		public int MaximumNumberOfParticles { get; set; }
		public RangeGraph<Vector> StartVelocity { get; set; }
		public RangeGraph<Vector> Acceleration { get; set; }
		public RangeGraph<Size> Size { get; set; }
		public RangeGraph<ValueRange> StartRotation { get; set; }
		public RangeGraph<ValueRange> RotationSpeed { get; set; }
		public RangeGraph<Color> Color { get; set; }
		public Material ParticleMaterial { get; set; }
		public BillboardMode BillboardMode { get; set; }
		public RangeGraph<Vector> StartPosition { get; set; }
		public RangeGraph<ValueRange> particlesPerSpawn { get; set; }

		protected override void LoadData(Stream fileData)
		{
			var emitterData = (ParticleEmitterData)new BinaryReader(fileData).Create();
			SpawnInterval = emitterData.SpawnInterval;
			LifeTime = emitterData.LifeTime;
			MaximumNumberOfParticles = emitterData.MaximumNumberOfParticles;
			StartVelocity = emitterData.StartVelocity;
			Acceleration = emitterData.Acceleration;
			Size = emitterData.Size;
			StartRotation = emitterData.StartRotation;
			Color = emitterData.Color;
			ParticleMaterial = emitterData.ParticleMaterial;
		}
	}
}