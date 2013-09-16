using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Rendering.Particles
{
	/// <summary>
	/// Particle data used by an Emitter to represent a single particle. Not a visual representation!
	/// </summary>
	public struct Particle3D : Lerp<Particle3D>, Particle
	{
		public Vector Position { get; set; }
		public float Rotation { get; set; }
		public Vector CurrentMovement { get; set; }
		public float ElapsedTime { get; set; }
		public Vector Acceleration { get; set; }
		public Size Size { get; set; }
		public Color Color { get; set; }
		public bool IsActive { get; set; }
		public int CurrentFrame { get; set; }
		public Rectangle CurrentUV { get; set; }
		public Material Material { get; set; }
		public VertexPosition3DColorUV[] Vertices { get; set; }

		public Particle3D Lerp(Particle3D other, float interpolation)
		{
			return new Particle3D
			{
				Position = Position.Lerp(other.Position, interpolation),
				Rotation = Rotation.Lerp(other.Rotation, interpolation),
				CurrentMovement = CurrentMovement.Lerp(other.CurrentMovement, interpolation),
				ElapsedTime = ElapsedTime.Lerp(other.ElapsedTime, interpolation),
				Size = Size.Lerp(other.Size, interpolation),
				Color = Color.Lerp(other.Color, interpolation),
				IsActive = IsActive && other.IsActive && ElapsedTime < other.ElapsedTime,
				CurrentUV = CurrentUV.Lerp(other.CurrentUV, interpolation),
				Material =
					other.Material != null
						? new Material(other.Material.Shader, other.Material.DiffuseMap) : null,
				Vertices = GetVertices(Size, Color)
			};
		}

		public static VertexPosition3DColorUV[] GetVertices(Size size, Color color)
		{
			float halfWidth = size.Width * 0.5f;
			float halfHeight = size.Height * 0.5f;
			return new[]
			{
				new VertexPosition3DColorUV(new Vector(-halfWidth, 0, halfHeight), color, Point.Zero),
				new VertexPosition3DColorUV(new Vector(halfWidth, 0, halfHeight), color, Point.UnitX),
				new VertexPosition3DColorUV(new Vector(halfWidth, 0, -halfHeight), color, Point.One),
				new VertexPosition3DColorUV(new Vector(-halfWidth, 0, -halfHeight), color, Point.UnitY)
			};
		}

		public void SetStartVelocityRandomizedFromRange(Vector startVelocity,
			Vector startVelocityVariance)
		{
			var velocityMin = startVelocity - startVelocityVariance;
			var velocityMax = startVelocity + startVelocityVariance;
			CurrentMovement = new Vector(velocityMin.X.Lerp(velocityMax.X, Randomizer.Current.Get()),
				velocityMin.Y.Lerp(velocityMax.Y, Randomizer.Current.Get()),
				velocityMin.Z.Lerp(velocityMax.Z, Randomizer.Current.Get()));
		}

		public bool UpdateIfStillActive(ParticleEmitterData data)
		{
			ElapsedTime += Time.Delta;
			if (ElapsedTime > data.LifeTime)
				return IsActive = false;
			CurrentMovement += Acceleration * Time.Delta;
			Position += CurrentMovement * Time.Delta;
			Rotation +=
				data.RotationSpeed.GetInterpolatedValue(ElapsedTime / data.LifeTime).GetRandomValue() *
					Time.Delta;
			return true;
		}
	}
}