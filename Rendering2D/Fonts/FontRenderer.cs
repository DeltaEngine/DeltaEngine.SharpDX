using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D.Fonts
{
	internal class FontRenderer : SpriteBatchRenderer
	{
		public FontRenderer(Drawing drawing)
			: base(drawing) {}

		protected override void AddVerticesToSpriteBatch(DrawableEntity entity)
		{
			var text = (FontText)entity;
			glyphs = text.Get<GlyphDrawData[]>();
			if (text.cachedMaterial == null && text.Get<Material>() != null)
				text.cachedMaterial = text.Get<Material>();
			var batch = FindOrCreateSpriteBatch(text.cachedMaterial, BlendMode.Normal, glyphs.Length);
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
			return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).X - (size.Width / 2).Round();
		}

		private float GetVerticalPosition(FontText text)
		{
			var alignment = text.VerticalAlignment;
			if (alignment == VerticalAlignment.Top)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft).Y;
			if (alignment == VerticalAlignment.Bottom)
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft).Y - size.Height;
			return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).Y - (size.Height / 2).Round();
		}

		private void AddIndicesAndVerticesForGlyph(SpriteBatch batch, GlyphDrawData glyph)
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