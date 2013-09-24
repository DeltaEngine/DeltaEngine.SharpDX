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

		public void Draw(IEnumerable<DrawableEntity> entities)
		{
			cameraUp = device.CameraInvertedViewMatrix.Up;
			cameraUp.Normalize();
			sortedList.Clear();
			unsortedList.Clear();
			foreach (var emitter in entities.OfType<Particle3DEmitter>())
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
			public readonly Particle3D Particle;
			public readonly Material Material;

			public ParticleToRender(Particle3D particle, Material material)
			{
				Particle = particle;
				Material = material;
			}
		}

		private void AddActiveParticlesToTheRenderLists(Particle3DEmitter emitter)
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

		private static IEnumerable<Particle3D> TryGetInterpolatedArray(Particle3DEmitter emitter)
		{
			Particle3D[] particles;
			try
			{
				particles = emitter.GetInterpolatedArray<Particle3D>();
			}
			catch (DrawableEntity.ArrayWithLerpElementsForInterpolationWasNotFound)
			{
				particles = null;
			}
			return particles;
		}

		private void AddParticleToTheSortedList(Particle3D particle, Material material)
		{
			Vector3D particlePositionInCameraSpace = device.CameraViewMatrix * particle.Position;
			if (!sortedList.ContainsKey(particlePositionInCameraSpace.Z))
				sortedList.Add(particlePositionInCameraSpace.Z, new ParticleToRender(particle, material));
		}

		private void AddParticleToTheUnsortedList(Particle3D particle, Material material)
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
				if (material != lastMaterial)
					RenderParticlesInVertexArray(lastMaterial);
				lastMaterial = material;
				AddVerticesToTheVertexArray(item.Value.Particle);
			}
			RenderParticlesInVertexArray(lastMaterial);
		}

		private void AddVerticesToTheVertexArray(Particle3D particle)
		{
			Vector3D look = device.CameraInvertedViewMatrix.Translation - particle.Position;
			look.Normalize();
			Vector3D right = Vector3D.Cross(cameraUp, look);
			Vector3D up = Vector3D.Cross(look, right);
			transform = Matrix.Identity;
			transform.Right = right;
			transform.Up = look;
			transform.Forward = up;
			transform.Translation = particle.Position;
			for (int i = 0; i < 4; i++)
			{
				vertices[currentVertex].Position = transform * particle.Vertices[i].Position;
				vertices[currentVertex].Color = particle.Vertices[i].Color;
				vertices[currentVertex].UV = particle.Vertices[i].UV;
				currentVertex++;
			}			
		}

		private Matrix transform;
		private readonly VertexPosition3DColorUV[] vertices =
			new VertexPosition3DColorUV[BufferMaxParticles * 4];

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
				if (material != lastMaterial)
					RenderParticlesInVertexArray(lastMaterial);
				lastMaterial = material;
				AddVerticesToTheVertexArray(item.Particle);
			}
			RenderParticlesInVertexArray(lastMaterial);
		}
	}
}