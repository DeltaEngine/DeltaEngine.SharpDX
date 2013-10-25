using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering2D
{
	/// <summary>
	/// Batches Sprites by Material and BlendMode for rendering
	/// </summary>
	public class SpriteBatchRenderer : DrawBehavior
	{
		public SpriteBatchRenderer(Drawing drawing)
		{
			this.drawing = drawing;
		}

		protected readonly Drawing drawing;

		public virtual void Draw(List<DrawableEntity> visibleEntities)
		{
			ResetSpriteBatches();
			foreach (var entity in visibleEntities)
				AddVerticesToSpriteBatch(entity);
			DrawBatches();
		}

		protected void ResetSpriteBatches()
		{
			foreach (var batch in batches)
				batch.Reset();
			batchIndex = 0;
		}

		protected readonly List<SpriteBatch> batches = new List<SpriteBatch>();
		protected int batchIndex;

		protected virtual void AddVerticesToSpriteBatch(DrawableEntity entity)
		{
			var sprite = (Sprite)entity;
			var batch = FindOrCreateSpriteBatch(sprite.Material, sprite.BlendMode);
			batch.AddIndicesAndVertices(sprite);
		}

		protected SpriteBatch FindOrCreateSpriteBatch(Material material, BlendMode blendMode, 
			int numberOfQuadsToAdd = 1)
		{
			for (int index = 0; index < batches.Count; index++)
			{
				var batch = batches[index];
				if (batch.material.Shader != material.Shader ||
					batch.material.DiffuseMap != material.DiffuseMap || batch.blendMode != blendMode ||
					batch.IsBufferFullAndResizeIfPossible(numberOfQuadsToAdd))
					continue;
				if (batchIndex <= index)
					batchIndex = index + 1;
				return batch;
			}
			var newBatch = new SpriteBatch(material, blendMode, numberOfQuadsToAdd);
			batches.Add(newBatch);
			batchIndex = batches.Count;
			return newBatch;
		}

		protected void DrawBatches()
		{
			for (int i = 0; i < batchIndex; i++)
				batches[i].Draw(drawing);
		}
	}
}