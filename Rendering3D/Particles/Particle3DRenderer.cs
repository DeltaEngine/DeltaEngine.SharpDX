using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Rendering3D.Particles
{
	internal class Particle3DRenderer : DrawBehavior
	{
		public Particle3DRenderer(Device device, Drawing drawing)
		{
			this.device = device;
			this.drawing = drawing;
			currentVertex = 0;
			int currentIndex = 0;
			for (int particle = 0; particle < ParticleEmitter.MaxParticles; particle++)
			{
				for (int particleIndex = 0; particleIndex < 6; particleIndex++)
					indices[currentIndex++] = (short)(refIndices[particleIndex] + currentVertex);
				currentVertex += 4;
			}
			currentVertex = 0;
		}

		private readonly Device device;
		private readonly Drawing drawing;
		private int currentVertex;
		private const int BufferMaxParticles = ParticleEmitter.MaxParticles * 4;
		private readonly short[] indices = new short[BufferMaxParticles * 6];
		private readonly short[] refIndices = new short[] { 0, 1, 2, 0, 2, 3 };

		public void Draw(List<DrawableEntity> visibleEntities)
		{
			cameraUp = device.CameraInvertedViewMatrix.Up;
			cameraUp.Normalize();
			sortedList.Clear();
			unsortedList.Clear();
			foreach (var emitter in visibleEntities.OfType<ParticleEmitter>())
			{
				AddActiveParticlesToTheRenderLists(emitter);
			}
			DrawAllParticles();
		}

		private Vector3D cameraUp;
		private readonly SortedList<float, ParticleToRender> sortedList =
			new SortedList<float, ParticleToRender>();
		private readonly List<ParticleToRender> unsortedList = new List<ParticleToRender>();

		private struct ParticleToRender
		{
			public readonly Particle Particle;
			public readonly Material Material;

			public ParticleToRender(Particle particle, Material material)
			{
				Particle = particle;
				Material = material;
			}
		}

		private void AddActiveParticlesToTheRenderLists(ParticleEmitter emitter)
		{
			var interpolatedParticleArray = TryGetInterpolatedArray(emitter);
			if(interpolatedParticleArray == null)
				return;
			foreach (var particle in interpolatedParticleArray)
			{
				if (!particle.IsActive)
					continue;
				var blendMode = emitter.EmitterData.ParticleMaterial.DiffuseMap.BlendMode;
				if (blendMode == BlendMode.Normal)
					AddParticleToTheSortedList(particle, emitter.EmitterData.ParticleMaterial);
				else
					AddParticleToTheUnsortedList(particle, emitter.EmitterData.ParticleMaterial);
			}
		}

		private static IEnumerable<Particle> TryGetInterpolatedArray(ParticleEmitter emitter)
		{
			Particle[] particles;
			try
			{
				particles = emitter.GetInterpolatedArray<Particle>();
			}
			catch (DrawableEntity.ArrayWithLerpElementsForInterpolationWasNotFound)
			{
				particles = null;
			}
			return particles;
		}

		private void AddParticleToTheSortedList(Particle particle, Material material)
		{
			Vector3D particlePositionInCameraSpace = device.CameraViewMatrix * particle.Position;
			if (!sortedList.ContainsKey(particlePositionInCameraSpace.Z))
				sortedList.Add(particlePositionInCameraSpace.Z, new ParticleToRender(particle, material));
		}

		private void AddParticleToTheUnsortedList(Particle particle, Material material)
		{
			unsortedList.Add(new ParticleToRender(particle, material));
		}

		private void DrawAllParticles()
		{
			DrawParticlesInSortedList();
			DrawParticlesInUnsortedList();
		}

		private void DrawParticlesInSortedList()
		{
			if (sortedList.Count == 0)
				return;
			Material lastMaterial = sortedList.First().Value.Material;
			foreach (var item in sortedList)
			{
				var material = item.Value.Material;
				lastMaterial = material;
				AddVerticesToTheVertexArray(item.Value.Particle);
			}
			RenderParticlesInVertexArray(lastMaterial);
		}

		private void AddVerticesToTheVertexArray(Particle particle)
		{
			UpdateTransformMatrix(particle);
			var particleVertices = particle.GetVertices(particle.Size, particle.Color);
			for (int i = 0; i < 4; i++)
			{
				vertices[currentVertex].Position = transform * particleVertices[i].Position;
				vertices[currentVertex].Color = particleVertices[i].Color;
				vertices[currentVertex].UV = particleVertices[i].UV;
				currentVertex++;
			}
		}

		private Matrix transform;
		private readonly VertexPosition3DColorUV[] vertices =
			new VertexPosition3DColorUV[BufferMaxParticles * 4];

		private void UpdateTransformMatrix(Particle particle)
		{
			var inverseView = device.CameraInvertedViewMatrix;
			Vector3D look = inverseView.Translation - particle.Position;
			look = CalculateCameraUpAndLook(particle, look, inverseView);
			look.Normalize();
			Vector3D right = Vector3D.Cross(cameraUp, look);
			Vector3D up = Vector3D.Cross(look, right);
			transform = Matrix.Identity;
			transform.Right = right;
			transform.Up = look;
			transform.Forward = up;
			transform *= Matrix.CreateRotationZYX(transform.Forward.X, transform.Forward.Y,
				transform.Forward.Z);
			transform.Translation = particle.Position;
		}

		private Vector3D CalculateCameraUpAndLook(Particle particle, Vector3D look,
			Matrix inverseView)
		{
			if ((particle.BillboardMode & BillboardMode.FrontAxis) != 0)
			{
				cameraUp = Vector3D.UnitY;
				look.Y = 0;
			}
			else if ((particle.BillboardMode & BillboardMode.UpAxis) != 0)
			{
				cameraUp = Vector3D.UnitZ;
				look.Z = 0;
			}
			else if ((particle.BillboardMode & BillboardMode.Ground) != 0)
			{
				cameraUp = -Vector3D.UnitY;
				look = Vector3D.UnitZ;
			}
			else
			{
				cameraUp = inverseView.Up;
				cameraUp.Normalize();
			}
			return look;
		}

		private void RenderParticlesInVertexArray(Material material)
		{
			drawing.Add(material, vertices, indices, currentVertex, currentVertex * 6 / 4);
			currentVertex = 0;
		}

		private void DrawParticlesInUnsortedList()
		{
			if (unsortedList.Count == 0)
				return;
			Material lastMaterial = unsortedList.First().Material;
			foreach (var item in unsortedList)
			{
				var material = item.Material;
				lastMaterial = material;
				AddVerticesToTheVertexArray(item.Particle);
			}
			RenderParticlesInVertexArray(lastMaterial);
		}
	}
}