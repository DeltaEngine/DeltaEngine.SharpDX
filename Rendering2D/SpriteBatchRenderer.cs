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
			spriteBatchIndex = 0;
			foreach (var entity in visibleEntities)
				AddVerticesToSpriteBatch((Sprite)entity);
			for (int i = 0; i < spriteBatchIndex; i++)
				spriteBatch[i].Draw(drawing);
		}

		private int spriteBatchIndex;
		private readonly SpriteBatch[] spriteBatch = new SpriteBatch[short.MaxValue];

		private void AddVerticesToSpriteBatch(Sprite sprite)
		{
			var key = new SpriteBatchKey(sprite);
			var hasColor = (sprite.Material.Shader as ShaderWithFormat).Format.HasColor;
			if (WasNotAbleToAddToExistingSpriteBatch(sprite, key, hasColor))
				CreateNewSpriteBatch(sprite, hasColor);
		}

		private bool WasNotAbleToAddToExistingSpriteBatch(Sprite sprite, SpriteBatchKey key,
			bool hasColor)
		{
			for (int i = 0; i < spriteBatchIndex; i++)
			{
				var vertices = spriteBatch[i];
				if (vertices.key == key)
					if (hasColor && !vertices.MaxVerticesExceeded(true))
					{
						vertices.AddVerticesToUVColorArray(sprite);
						return false;
					}
					else if (!vertices.MaxVerticesExceeded(false))
					{
						vertices.AddVerticesAndIndicesToUVArray(sprite);
						return false;
					}
			}
			return true;
		}

		private void CreateNewSpriteBatch(Sprite sprite, bool hasColor)
		{
			spriteBatch[spriteBatchIndex] = new SpriteBatch(sprite);
			if (hasColor)
				spriteBatch[spriteBatchIndex].AddVerticesToUVColorArray(sprite);
			else
				spriteBatch[spriteBatchIndex].AddVerticesAndIndicesToUVArray(sprite);
			spriteBatchIndex++;
		}
	}
}