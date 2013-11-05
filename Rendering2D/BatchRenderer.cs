using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering2D
{
	/// <summary>
	/// Batches by Material and BlendMode for rendering
	/// </summary>
	public class BatchRenderer
	{
		public BatchRenderer(Drawing drawing)
		{
			this.drawing = drawing;
		}

		private readonly Drawing drawing;

		protected readonly List<Batch> batches = new List<Batch>();
		protected int batchIndex;

		public Batch FindOrCreateBatch(Material material, BlendMode blendMode, 
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
			var newBatch = new Batch(material, blendMode, numberOfQuadsToAdd);
			batches.Add(newBatch);
			batchIndex = batches.Count;
			return newBatch;
		}

		public void FlushDrawBuffer(Batch batch)
		{
			DrawAndResetBatch(batch);
			drawing.FlushDrawBuffer(batch.material, batch.blendMode);
		}

		private void DrawAndResetBatch(Batch batch)
		{
			batch.Draw(drawing);
			batch.Reset();
		}

		public void DrawAndResetBatches()
		{
			for (int i = 0; i < batchIndex; i++)
				DrawAndResetBatch(batches[i]);
			batchIndex = 0;
		}
	}
}