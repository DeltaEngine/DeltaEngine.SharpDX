using CreepyTowers.Content;
using CreepyTowers.Creeps;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.GameLogic;
using DeltaEngine.Rendering3D;
using DeltaEngine.Rendering3D.Particles;

namespace CreepyTowers.Towers
{
	/// <summary>
	/// Tower, stats and behavior varying depending on its type.
	/// </summary>
	public sealed class Tower : DrawableEntity, Actor
	{
		public Tower(TowerType type, Vector3D position)
		{
			Add(data = ContentLoader.Load<TowerPropertiesXml>("TowerProperties").Get(type));
			Position = position;
			Scale = Vector3D.One;
			SetDefaultValues();
			Add(ContentLoader.Load<ModelData>(data.Name));
			OnDraw<ModelRenderer>();
		}

		//TODO: add 2D constructor
		private readonly TowerData data;
		public Vector3D Position { get; set; }
		public Quaternion Orientation { get; set; }
		public Vector3D Scale { get; set; }

		private void SetDefaultValues()
		{
			RenderLayer = (int)CreepyTowersRenderLayer.Towers;
			TimeOfLastAttack = Time.Total;
		}

		public float TimeOfLastAttack { get; private set; }

		public void FireAtCreep(Creep creep)
		{
			if (creep == null || !creep.IsActive)
				return;
			if ((Time.Total - TimeOfLastAttack) < 1 / Get<TowerData>().AttackFrequency)
				return;
			FireParticle(creep);
			TimeOfLastAttack = Time.Total;
			var properties = Get<TowerData>();
			creep.ReceiveAttack(properties.Type, properties.AttackDamage);
		}

		private void FireParticle(Creep creep)
		{
			var direction = creep.Position - Position;
			var particleEmitterData = CreateParticleEmitterData();
			particleEmitterData.StartVelocity.Start = Vector3D.Normalize(direction);
			emitter = new Particle3DPointEmitter(particleEmitterData, Position);
			var angle = MathExtensions.Atan2(direction.Y, direction.X);
			particleEmitterData.StartRotation =
				new RangeGraph<ValueRange>(new ValueRange(-angle, -angle), new ValueRange(-angle, -angle));
			emitter.SpawnAndDispose();
			if (emitter.particles[0].Position == creep.Position)
				emitter.particles[0].IsActive = false;
		}

		private Particle3DPointEmitter emitter;

		private static ParticleEmitterData CreateParticleEmitterData()
		{
			const int MaxParticles = 64;
			const float LifeTime = 2.0f;
			var data = new ParticleEmitterData
			{
				MaximumNumberOfParticles = MaxParticles,
				SpawnInterval = 0.0f,
				LifeTime = LifeTime,
				Size = new RangeGraph<Size>(Size.Half, Size.Half),
				Color = new RangeGraph<Color>(Color.White, Color.White),
				Acceleration = new RangeGraph<Vector3D>(Vector3D.Zero, Vector3D.Zero),
				StartVelocity =
					new RangeGraph<Vector3D>(new Vector3D(-0.5f, -0.5f, 0.1f), new Vector3D(0.5f, 0.5f, 0.1f)),
				ParticleMaterial =
					new Material(Shader.Position3DColorUV, Effects.EffectsWaterRangedTrans.ToString()),
				BillboardMode = BillboardMode.Ground
			};
			data.StartVelocity.End = Vector3D.Zero;
			return data;
		}

		public void Dispose()
		{
			IsActive = false;
		}
	}
}