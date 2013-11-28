using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D.Fonts
{
	internal class FontRenderer : DrawBehavior
	{
		public FontRenderer(BatchRenderer renderer)
		{
			this.renderer = renderer;
		}

		private readonly BatchRenderer renderer;

		public void Draw(List<DrawableEntity> visibleEntities)
		{
			foreach (var entity in visibleEntities)
				AddVerticesToBatch((FontText)entity);
		}

		private void AddVerticesToBatch(FontText text)
		{
			glyphs = text.Get<GlyphDrawData[]>();
			var batch = renderer.FindOrCreateBatch(text.CachedMaterial, BlendMode.Normal, glyphs.Length);
			drawArea = text.Get<Rectangle>();
			color = text.Get<Color>();
			size = text.Get<Size>();
			position = new Vector2D(GetHorizontalPosition(text), GetVerticalPosition(text));
			foreach (GlyphDrawData glyph in glyphs)
				AddIndicesAndVerticesForGlyph(batch, glyph);
		}

		private Rectangle drawArea;
		private Color color;
		private Vector2D position;
		private GlyphDrawData[] glyphs;
		private Size size;

		private float GetHorizontalPosition(FontText text)
		{
			var alignment = text.HorizontalAlignment;
			if (alignment == HorizontalAlignment.Left)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft).X;
			if (alignment == HorizontalAlignment.Right)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight).X - size.Width;
			// ReSharper disable once PossibleLossOfFraction
			return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).X - (int)size.Width / 2;
		}

		private float GetVerticalPosition(FontText text)
		{
			var alignment = text.VerticalAlignment;
			if (alignment == VerticalAlignment.Top)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft).Y;
			if (alignment == VerticalAlignment.Bottom)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft).Y - (int)size.Height;
			// ReSharper disable once PossibleLossOfFraction
			return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).Y - (int)size.Height / 2;
		}

		private void AddIndicesAndVerticesForGlyph(Batch batch, GlyphDrawData glyph)
		{
			batch.AddIndices();
			batch.verticesColorUV[batch.verticesIndex++] = new VertexPosition2DColorUV(
				position + glyph.DrawArea.TopLeft, color, glyph.UV.TopLeft);
			batch.verticesColorUV[batch.verticesIndex++] = new VertexPosition2DColorUV(
				position + glyph.DrawArea.TopRight, color, glyph.UV.TopRight);
			batch.verticesColorUV[batch.verticesIndex++] = new VertexPosition2DColorUV(
				position + glyph.DrawArea.BottomRight, color, glyph.UV.BottomRight);
			batch.verticesColorUV[batch.verticesIndex++] = new VertexPosition2DColorUV(
				position + glyph.DrawArea.BottomLeft, color, glyph.UV.BottomLeft);
		}
	}
}