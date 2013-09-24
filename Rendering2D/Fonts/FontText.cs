using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D.Fonts
{
	/// <summary>
	/// Entity used to render text on screen. Can be aligned horizontally and vertically within
	/// a draw area. Can also contain line breaks.
	/// </summary>
	public class FontText : Entity2D
	{
		public FontText(Font font, string text, Vector2D centerPosition)
			: this(font, text, Rectangle.FromCenter(centerPosition, new Size(0.3f, 0.1f))) {}

		public FontText(Font font, string text, Rectangle drawArea)
			: base(drawArea)
		{
			this.text = text;
			wasFontLoadedOk = font.WasLoadedOk;
			if (wasFontLoadedOk)
				RenderAsFontText(font);
			else
				RenderAsVectorText();
		}

		private string text;
		private readonly bool wasFontLoadedOk;

		private void RenderAsFontText(Font font)
		{
			description = font.Description;
			description.Generate(text, HorizontalAlignment.Center);
			Add(font.Material);
			Add(description.Glyphs);
			Add(description.DrawSize);
			OnDraw<Render>();
		}

		private FontDescription description;

	  private void RenderAsVectorText()
		{
			Add(new VectorText.Data(text));
			Start<VectorText.ProcessText>();
			OnDraw<VectorText.Render>();
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
				else
					Get<VectorText.Data>().Text = value;
			}
		}

		private void UpdateFontTextRendering()
		{
			Remove<GlyphDrawData[]>();
			description.Generate(text, HorizontalAlignment);
			Add(description.Glyphs);
			Set(description.DrawSize);
		}

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return Contains<HorizontalAlignment>()
					? Get<HorizontalAlignment>() : HorizontalAlignment.Center;
			}
			set
			{
				Set(value);
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get { return Contains<VerticalAlignment>() ? Get<VerticalAlignment>() : VerticalAlignment.Center; }
			set
			{
				Set(value);
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
			}
		}

		public class Render : DrawBehavior
		{
			public Render(Drawing drawing)
			{
				this.drawing = drawing;
			}

			private readonly Drawing drawing;

			public void Draw(IEnumerable<DrawableEntity> entities)
			{
				drawFontCount = 0;
				foreach (var entity in entities)
					AddToBatch((FontText)entity);
				for (int i = 0; i < drawFontCount; i++)
					drawing.Add(drawnFontTexts[i].material, drawnFontTexts[i].vertices,
						drawnFontTexts[i].indices, drawnFontTexts[i].verticesCount,
						drawnFontTexts[i].indicesCount);
			}

			private void AddToBatch(FontText text)
			{
				drawArea = text.Get<Rectangle>();
				color = text.Get<Color>();
				position = new Vector2D(GetHorizontalPosition(text), GetVerticalPosition(text));
				material = text.Get<Material>();
				glyphs = text.Get<GlyphDrawData[]>();
				AddVerticesAndIndices();
			}

			private Rectangle drawArea;
			private Color color;
			private Vector2D position;
			private Material material;
			private GlyphDrawData[] glyphs;

			private float GetHorizontalPosition(FontText text)
			{
				var alignment = text.HorizontalAlignment;
				if (alignment == HorizontalAlignment.Left)
					return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft).X;
				var size = text.Get<Size>();
				if (alignment == HorizontalAlignment.Right)
					return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight).X - size.Width;
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).X -
					MathExtensions.Round(size.Width / 2);
			}

			private float GetVerticalPosition(FontText text)
			{
				var alignment = text.VerticalAlignment;
				if (alignment == VerticalAlignment.Top)
					return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft).Y;
				var size = text.Get<Size>();
				if (alignment == VerticalAlignment.Bottom)
					return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft).Y - size.Height;
				return ScreenSpace.Current.ToPixelSpaceRounded(drawArea.Center).Y -
					MathExtensions.Round(size.Height / 2);
			}

			private void AddVerticesAndIndices()
			{
				foreach (GlyphDrawData glyph in glyphs)
					AddVerticesAndIndicesForGlyph(glyph);
			}

			private void AddVerticesAndIndicesForGlyph(GlyphDrawData glyph)
			{
				var fontVertices = GetFontVertices();
				fontVertices.AddIndicesForGlyph();

				fontVertices.AddVertex(new VertexPosition2DColorUV(position + glyph.DrawArea.TopLeft, color,
					glyph.UV.TopLeft));
				fontVertices.AddVertex(new VertexPosition2DColorUV(position + glyph.DrawArea.TopRight,
					color, glyph.UV.TopRight));
				fontVertices.AddVertex(new VertexPosition2DColorUV(position + glyph.DrawArea.BottomRight,
					color, glyph.UV.BottomRight));
				fontVertices.AddVertex(new VertexPosition2DColorUV(position + glyph.DrawArea.BottomLeft,
					color, glyph.UV.BottomLeft));
			}

			private SpriteInfo GetFontVertices()
			{
				for (int i = 0; i < drawFontCount; i++)
					if (drawnFontTexts[i].material.DiffuseMap == material.DiffuseMap &&
						!drawnFontTexts[i].MaxVerticesExceeded())
						return drawnFontTexts[i];

				drawnFontTexts[drawFontCount++] = new SpriteInfo(material);
				return drawnFontTexts[drawFontCount - 1];
			}

			private readonly SpriteInfo[] drawnFontTexts = new SpriteInfo[short.MaxValue];
			private int drawFontCount;

			private class SpriteInfo
			{
				public SpriteInfo(Material material)
				{
					vertices = new VertexPosition2DColorUV[short.MaxValue * 4 / 6];
					indices = new short[short.MaxValue];
					this.material = material;
					verticesCount = 0;
					indicesCount = 0;
				}

				public readonly Material material;
				internal readonly VertexPosition2DColorUV[] vertices;
				internal readonly short[] indices;
				internal int verticesCount, indicesCount;

				public void AddIndicesForGlyph()
				{
					indices[indicesCount++] = (short)verticesCount;
					indices[indicesCount++] = (short)(verticesCount + 1);
					indices[indicesCount++] = (short)(verticesCount + 2);
					indices[indicesCount++] = (short)verticesCount;
					indices[indicesCount++] = (short)(verticesCount + 2);
					indices[indicesCount++] = (short)(verticesCount + 3);
				}

				public void AddVertex(VertexPosition2DColorUV vertex)
				{
					vertices[verticesCount++] = vertex;
				}

				public bool MaxVerticesExceeded()
				{
					return short.MaxValue * 4 / 6 - verticesCount < 4;
				}
			}
		}
	}
}