using System.Collections.Generic;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering2D.Particles
{
	internal class Particle2DRenderer : SpriteBatchRenderer
	{
		public Particle2DRenderer(Drawing drawing)
			: base(drawing) { }

		public override void Draw(List<DrawableEntity> visibleEntities)
		{
			ResetSpriteBatches();
			foreach (ParticleEmitter entity in visibleEntities)
				if (entity.NumberOfActiveParticles > 0)
					AddVerticesToBatch(entity);
			DrawBatches();
		}

		private void AddVerticesToBatch(ParticleEmitter emitter)
		{
			particles = emitter.GetInterpolatedArray<Particle>(emitter.NumberOfActiveParticles);
			var length = particles.Length;
			for (int index = 0; index < length; index++)
				AddIndicesAndVerticesForParticle(index);
		}

		private Particle[] particles;

		private void AddIndicesAndVerticesForParticle(int index)
		{
			var particle = particles[index];
			if (!particle.IsActive)
				return;
			var material = particle.Material;
			var batch = FindOrCreateSpriteBatch(material, material.DiffuseMap.BlendMode);
			batch.AddIndices();
			batch.verticesColorUV[batch.verticesIndex++] = particle.GetTopLeftVertex();
			batch.verticesColorUV[batch.verticesIndex++] = particle.GetTopRightVertex();
			batch.verticesColorUV[batch.verticesIndex++] = particle.GetBottomRightVertex();
			batch.verticesColorUV[batch.verticesIndex++] = particle.GetBottomLeftVertex();
		}
	}
}
