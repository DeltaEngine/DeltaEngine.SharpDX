using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Rendering3D.Particles;

namespace DeltaEngine.Rendering2D.Particles
{
	internal class Particle2DRenderer : DrawBehavior
	{
		public Particle2DRenderer(Drawing drawing)
		{
			this.drawing = drawing;
		}

		private readonly Drawing drawing;

		public void Draw(List<DrawableEntity> visibleEntities)
		{
			foreach (ParticleEmitter entity in visibleEntities)
				if (entity.NumberOfActiveParticles > 0)
					FillParticlesDrawContexts(entity);
			foreach (var verticesToRender in verticesPerTexture)
				if (verticesToRender.CurrentIndex > 0)
					RenderBatch(verticesToRender);
		}

		private readonly List<VerticesToRender> verticesPerTexture = new List<VerticesToRender>();

		private class VerticesToRender
		{
			public VerticesToRender(Image texture, Material material)
			{
				Texture = texture;
				this.material = material;
				FillIndices(Data.Length);
			}

			public readonly Image Texture;
			public readonly Material material;

			private void FillIndices(int verticesCount)
			{
				for (int quad = 0; quad < verticesCount / 4; quad++)
				{
					Indices[quad * 6 + 0] = (short)(quad * 4 + 0);
					Indices[quad * 6 + 1] = (short)(quad * 4 + 1);
					Indices[quad * 6 + 2] = (short)(quad * 4 + 2);
					Indices[quad * 6 + 3] = (short)(quad * 4 + 0);
					Indices[quad * 6 + 4] = (short)(quad * 4 + 2);
					Indices[quad * 6 + 5] = (short)(quad * 4 + 3);
				}
			}

			public readonly VertexPosition2DColorUV[] Data = new VertexPosition2DColorUV[8 * 1024];
			public readonly short[] Indices = new short[12 * 1024];
			public int CurrentIndex;

			public void AddParticlesOfEmitter(Particle interpolatedParticle)
			{
				Data[CurrentIndex] = interpolatedParticle.GetTopLeftVertex();
				Data[CurrentIndex + 1] = interpolatedParticle.GetTopRightVertex();
				Data[CurrentIndex + 2] = interpolatedParticle.GetBottomRightVertex();
				Data[CurrentIndex + 3] = interpolatedParticle.GetBottomLeftVertex();
				CurrentIndex += 4;
			}
		}

		private void FillParticlesDrawContexts(ParticleEmitter emitter)
		{
			foreach (var particle in emitter.GetInterpolatedArray<Particle>())
			{
				if (!particle.IsActive)
					continue;
				CreateVerticesToRenderIfNewImage(emitter, particle);
				foreach (var verticesToRender in verticesPerTexture)
					if (verticesToRender.Texture == particle.Material.DiffuseMap)
						verticesToRender.AddParticlesOfEmitter(particle);
			}
		}

		private void CreateVerticesToRenderIfNewImage(ParticleEmitter emitter, Particle particle)
		{
			if (verticesPerTexture.All(vpt => vpt.Texture != particle.Material.DiffuseMap))
				verticesPerTexture.Add(new VerticesToRender(particle.Material.DiffuseMap,
					emitter.EmitterData.ParticleMaterial));
		}

		private void RenderBatch(VerticesToRender vertices)
		{
			var newMaterial = new Material(vertices.material.Shader, vertices.Texture);
			drawing.Add(newMaterial, vertices.Data, vertices.Indices, vertices.CurrentIndex,
				vertices.CurrentIndex * 6 / 4);
			vertices.CurrentIndex = 0;
		}
	}
}