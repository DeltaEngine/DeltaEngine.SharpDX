using System.Collections.Generic;
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

		private readonly Drawing drawing;

		public void Draw(List<DrawableEntity> visibleEntities)
		{
			ResetSpriteBatches();
			foreach (var entity in visibleEntities)
				AddVerticesToSpriteBatch((Sprite)entity);
			for (int i = 0; i < spriteBatchIndex; i++)
				spriteBatch[i].Draw(drawing);
		}

		private SpriteBatch[] spriteBatch = new SpriteBatch[short.MaxValue];

		private void ResetSpriteBatches()
		{
			for (int i = 0; i < spriteBatchIndex; i++)
				spriteBatch[i].Reset();
			if (!Time.CheckEvery(DeleteSpriteBatchesInterval))
				return;
			spriteBatch = new SpriteBatch[short.MaxValue];
			spriteBatchIndex = 0;
		}

		private int spriteBatchIndex;
		private const int DeleteSpriteBatchesInterval = 10;

		private void AddVerticesToSpriteBatch(Sprite sprite)
		{
			var key = new SpriteBatchKey(sprite);
			var hasColor = (sprite.Material.Shader as ShaderWithFormat).Format.HasColor;
			if (hasColor && WasNotAbleToAddToExistingColorSpriteBatch(key, sprite))
				CreateNewColorSpriteBatch(key, sprite);
			else if (!hasColor && WasNotAbleToAddToExistingSpriteBatch(key, sprite))
				CreateNewSpriteBatch(key, sprite);
		}

		private bool WasNotAbleToAddToExistingColorSpriteBatch(SpriteBatchKey key, Sprite sprite)
		{
			for (int i = 0; i < spriteBatchIndex; i++)
			{
				var vertices = spriteBatch[i];
				if (vertices.key != key || vertices.AreColorBuffersFull())
					continue;
				vertices.AddColorVerticesAndIndices(sprite);
				return false;
			}
			return true;
		}

		private void CreateNewColorSpriteBatch(SpriteBatchKey key, Sprite sprite)
		{
			spriteBatch[spriteBatchIndex] = new SpriteBatch(key);
			spriteBatch[spriteBatchIndex].AddColorVerticesAndIndices(sprite);
			spriteBatchIndex++;
		}

		private bool WasNotAbleToAddToExistingSpriteBatch(SpriteBatchKey key, Sprite sprite)
		{
			for (int i = 0; i < spriteBatchIndex; i++)
			{
				var vertices = spriteBatch[i];
				if (vertices.key != key || vertices.AreBuffersFull())
					continue;
				vertices.AddVerticesAndIndices(sprite);
				return false;
			}
			return true;
		}

		private void CreateNewSpriteBatch(SpriteBatchKey key, Sprite sprite)
		{
			spriteBatch[spriteBatchIndex] = new SpriteBatch(key);
			spriteBatch[spriteBatchIndex].AddVerticesAndIndices(sprite);
			spriteBatchIndex++;
		}
	}
}