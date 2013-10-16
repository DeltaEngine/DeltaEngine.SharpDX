using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D
{
	internal class SpriteBatch
	{
		public SpriteBatch(Sprite sprite)
		{
			key = new SpriteBatchKey(sprite);
		}

		public readonly SpriteBatchKey key;

		public void Reset()
		{
			verticesUVBatchIndex = 0;
			indicesUVIndex = 0;
			verticesUVColorBatchIndex = 0;
			indicesUVColorIndex = 0;
		}

		private int verticesUVBatchIndex;
		private int indicesUVIndex;
		private int verticesUVColorBatchIndex;
		private int indicesUVColorIndex;

		public bool AreBuffersFull()
		{
			return short.MaxValue - indicesUVIndex < IndicesPerSprite;
		}

		private const int IndicesPerSprite = 6;

		public bool AreColorBuffersFull()
		{
			return short.MaxValue - indicesUVColorIndex < IndicesPerSprite;
		}

		public void AddVerticesAndIndicesToUVArray(Sprite sprite)
		{
			indicesUVBatch[indicesUVIndex ++] = (short)verticesUVBatchIndex;
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 1);
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 2);
			indicesUVBatch[indicesUVIndex ++] = (short)verticesUVBatchIndex;
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 2);
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 3);
			AddUVVertices(sprite);
		}

		private readonly short[] indicesUVBatch = new short[short.MaxValue];

		private void AddUVVertices(Sprite sprite)
		{
			if (HasSomethingToRender(sprite))
				if (rotation == 0)
					AddUVVerticesNotRotated();
				else
					AddUVVerticesRotated(sprite.RotationCenter);
		}

		private bool HasSomethingToRender(Sprite sprite)
		{
			var results = sprite.Get<UVCalculator.Results>();
			drawArea = results.DrawArea;
			uv = results.AtlasUV;
			screen = ScreenSpace.Current;
			rotation = sprite.Get<float>();
			return results.HasSomethingToRender;
		}

		private Rectangle drawArea;
		private Rectangle uv;
		private ScreenSpace screen;
		private float rotation;

		private void AddUVVerticesNotRotated()
		{
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopLeft), uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopRight), uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomRight), uv.BottomRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomLeft), uv.BottomLeft);
		}

		private readonly VertexPosition2DUV[] verticesUVBatch =
			new VertexPosition2DUV[short.MaxValue * VerticesPerSprite / IndicesPerSprite];

		private const int VerticesPerSprite = 4;

		private void AddUVVerticesRotated(Vector2D rotationCenter)
		{
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)),
					uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					uv.BottomRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					uv.BottomLeft);
		}

		public void AddVerticesToUVColorArray(Sprite sprite)
		{
			indicesUVColorBatch[indicesUVColorIndex++] = (short)verticesUVColorBatchIndex;
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 1);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 2);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)verticesUVColorBatchIndex;
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 2);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 3);
			AddUVColorVertices(sprite);
		}

		private readonly short[] indicesUVColorBatch = new short[short.MaxValue];

		private void AddUVColorVertices(Sprite sprite)
		{
			if (HasSomethingToRender(sprite))
				if (rotation == 0)
					AddUVColorVerticesNotRotated(sprite.Color);
				else
					AddUVColorVerticesRotated(sprite.Color, sprite.RotationCenter);
		}

		private void AddUVColorVerticesNotRotated(Color color)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft),
					color, uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight),
					color, uv.BottomRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft),
					color, uv.BottomLeft);
		}

		private readonly VertexPosition2DColorUV[] verticesUVColorBatch =
			new VertexPosition2DColorUV[short.MaxValue * VerticesPerSprite / IndicesPerSprite];

		private void AddUVColorVerticesRotated(Color color, Vector2D rotationCenter)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)), color,
					uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					color, uv.BottomRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					color, uv.BottomLeft);
		}

		public void Draw(Drawing drawing)
		{
			if (verticesUVBatchIndex > 0)
				drawing.Add(key.Material, key.BlendMode, verticesUVBatch, indicesUVBatch,
					verticesUVBatchIndex, indicesUVIndex);
			if (verticesUVColorBatchIndex > 0)
				drawing.Add(key.Material, key.BlendMode, verticesUVColorBatch, indicesUVColorBatch,
					verticesUVColorBatchIndex, indicesUVColorIndex);
		}
	}
}