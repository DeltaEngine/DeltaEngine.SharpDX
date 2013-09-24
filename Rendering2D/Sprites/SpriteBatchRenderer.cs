using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D.Sprites
{
	public class SpriteBatchRenderer : DrawBehavior
	{
		public SpriteBatchRenderer(Drawing drawing)
		{
			this.drawing = drawing;
		}

		public Drawing drawing;

		public void Draw(IEnumerable<DrawableEntity> entities)
		{
			currentDrawVerticesIndex = 0;
			foreach (var entity in entities.OfType<Sprite>())
				AddVerticesToSortedArray(entity);
			for (int i = 0; i < currentDrawVerticesIndex; i++)
				sortedVerticesToDraw[i].Draw(drawing);
		}

		private void AddVerticesToSortedArray(Sprite entity)
		{
			var material = entity.Material;
			var blendMode = entity.BlendMode;
			var found = false;
			for (int i = 0; i < currentDrawVerticesIndex; i++)
				if (sortedVerticesToDraw[i].material.DiffuseMap == material.DiffuseMap &&
					sortedVerticesToDraw[i].blendMode == blendMode)
					if ((material.Shader as ShaderWithFormat).Format.HasColor &&
						!sortedVerticesToDraw[i].MaxVerticesExceeded(true))
					{
						sortedVerticesToDraw[i].AddVerticesToUvColorArray(entity);
						found = true;
					}
					else if (!sortedVerticesToDraw[i].MaxVerticesExceeded(false))
					{
						sortedVerticesToDraw[i].AddVerticesAndIndicesToUvArray(entity);
						found = true;
					}
			if (!found)
			{
				sortedVerticesToDraw[currentDrawVerticesIndex ++] = new VerticesToRenderUv(entity);
				if ((material.Shader as ShaderWithFormat).Format.HasColor)
				sortedVerticesToDraw[currentDrawVerticesIndex -1].AddVerticesToUvColorArray(entity);
				else
					sortedVerticesToDraw[currentDrawVerticesIndex -1].AddVerticesAndIndicesToUvArray(entity);
			}

		}

		private readonly VerticesToRenderUv[] sortedVerticesToDraw =
			new VerticesToRenderUv[short.MaxValue];
		private int currentDrawVerticesIndex;

		private class VerticesToRenderUv
		{
			public VerticesToRenderUv(Sprite entity)
			{
				material = entity.Material;
				blendMode = entity.BlendMode;
				verticesUvBatchIndex = 0;
				indicesUvIndex = 0;
				verticesUvColorBatchIndex = 0;
				indicesUvColorIndex = 0;
			}

			public readonly Material material;
			public readonly BlendMode blendMode;

			public bool MaxVerticesExceeded(bool hasColor)
			{
				return hasColor
					? short.MaxValue * 4 / 6 - verticesUvColorBatchIndex < 4
					: short.MaxValue * 4 / 6 - verticesUvBatchIndex < 4;
			}

			public void AddVerticesAndIndicesToUvArray(Sprite entity)
			{
				indicesUvBatch[indicesUvIndex ++] = (short)verticesUvBatchIndex;
				indicesUvBatch[indicesUvIndex ++] = (short)(verticesUvBatchIndex + 1);
				indicesUvBatch[indicesUvIndex ++] = (short)(verticesUvBatchIndex + 2);
				indicesUvBatch[indicesUvIndex ++] = (short)verticesUvBatchIndex;
				indicesUvBatch[indicesUvIndex ++] = (short)(verticesUvBatchIndex + 2);
				indicesUvBatch[indicesUvIndex ++] = (short)(verticesUvBatchIndex + 3);
				AddUvVertices(entity);
			}

			private readonly VertexPosition2DUV[] verticesUvBatch =
				new VertexPosition2DUV[short.MaxValue * 4 / 6];
			private readonly short[] indicesUvBatch = new short[short.MaxValue];
			private int verticesUvBatchIndex;
			private int indicesUvIndex;

			private void AddUvVertices(Sprite entity)
			{
				var drawArea = entity.Get<Rectangle>();
				var rotation = entity.Get<float>();
				var uv = entity.Coordinates.UV;
				if (rotation != 0)
				{
					var rotationCenter = entity.RotationCenter;
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter,
								rotation)), uv.TopLeft);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter,
								rotation)), uv.TopRight);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(
								rotationCenter, rotation)), uv.BottomRight);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(
								rotationCenter, rotation)), uv.BottomLeft);
				}
				else
				{
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft),
							uv.TopLeft);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight),
							uv.TopRight);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight),
							uv.BottomRight);
					verticesUvBatch[verticesUvBatchIndex++] =
						new VertexPosition2DUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft),
							uv.BottomLeft);
				}
			}

			public void AddVerticesToUvColorArray(Sprite entity)
			{
				indicesUvColorBatch[indicesUvColorIndex++] = (short)verticesUvColorBatchIndex;
				indicesUvColorBatch[indicesUvColorIndex++] = (short)(verticesUvColorBatchIndex + 1);
				indicesUvColorBatch[indicesUvColorIndex++] = (short)(verticesUvColorBatchIndex + 2);
				indicesUvColorBatch[indicesUvColorIndex++] = (short)verticesUvColorBatchIndex;
				indicesUvColorBatch[indicesUvColorIndex++] = (short)(verticesUvColorBatchIndex + 2);
				indicesUvColorBatch[indicesUvColorIndex++] = (short)(verticesUvColorBatchIndex + 3);
				AddUvColorVertices(entity);
			}

			private readonly VertexPosition2DColorUV[] verticesUvColorBatch =
				new VertexPosition2DColorUV[short.MaxValue];
			private readonly short[] indicesUvColorBatch = new short[short.MaxValue];
			private int verticesUvColorBatchIndex;
			private int indicesUvColorIndex;

			private void AddUvColorVertices(Sprite entity)
			{
				var drawArea = entity.Get<Rectangle>();
				var rotation = entity.Get<float>();
				var uv = entity.Coordinates.UV;

				if (rotation != 0)
				{
					var rotationCenter = entity.RotationCenter;
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter,
								rotation)), entity.Color, uv.TopLeft);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter,
								rotation)), entity.Color, uv.TopRight);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(
								rotationCenter, rotation)), entity.Color, uv.BottomRight);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(
								rotationCenter, rotation)), entity.Color, uv.BottomLeft);
				}
				else
				{
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft),
							entity.Color, uv.TopLeft);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight),
							entity.Color, uv.TopRight);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight), entity.Color,
							uv.BottomRight);
					verticesUvColorBatch[verticesUvColorBatchIndex++] =
						new VertexPosition2DColorUV(
							ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft), entity.Color,
							uv.BottomLeft);
				}
			}

			private static readonly VertexPosition2DColorUV[] VertexUvColor =
				new VertexPosition2DColorUV[4];

			public void Draw(Drawing draw)
			{
				if (verticesUvBatchIndex != 0)
					draw.Add(material, verticesUvBatch, indicesUvBatch, verticesUvBatchIndex, indicesUvIndex);
				if (verticesUvColorBatchIndex != 0)
					draw.Add(material, verticesUvColorBatch, indicesUvColorBatch, verticesUvColorBatchIndex,
						indicesUvColorIndex);
			}
		}
	}
}