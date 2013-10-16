using System.Collections.Generic;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering2D
{
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
			if (hasColor && WasNotAbleToAddToExistingColorSpriteBatch(sprite, key))
				CreateNewColorSpriteBatch(sprite);
			else if (!hasColor && WasNotAbleToAddToExistingSpriteBatch(sprite, key))
				CreateNewSpriteBatch(sprite);
		}

		private bool WasNotAbleToAddToExistingColorSpriteBatch(Sprite sprite, SpriteBatchKey key)
		{
			for (int i = 0; i < spriteBatchIndex; i++)
			{
				var vertices = spriteBatch[i];
				if (vertices.key != key || vertices.AreColorBuffersFull())
					continue;
				vertices.AddVerticesToUVColorArray(sprite);
				return false;
			}
			return true;
		}

		private void CreateNewColorSpriteBatch(Sprite sprite)
		{
			spriteBatch[spriteBatchIndex] = new SpriteBatch(sprite);
			spriteBatch[spriteBatchIndex].AddVerticesToUVColorArray(sprite);
			spriteBatchIndex++;
		}

		private bool WasNotAbleToAddToExistingSpriteBatch(Sprite sprite, SpriteBatchKey key)
		{
			for (int i = 0; i < spriteBatchIndex; i++)
			{
				var vertices = spriteBatch[i];
				if (vertices.key != key || vertices.AreBuffersFull())
					continue;
				vertices.AddVerticesAndIndicesToUVArray(sprite);
				return false;
			}
			return true;
		}

		private void CreateNewSpriteBatch(Sprite sprite)
		{
			spriteBatch[spriteBatchIndex] = new SpriteBatch(sprite);
			spriteBatch[spriteBatchIndex].AddVerticesAndIndicesToUVArray(sprite);
			spriteBatchIndex++;
		}
	}
}