using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D.Shapes
{
	public class GradientFilledRect : Entity2D
	{
		public GradientFilledRect(Rectangle drawArea, Color startColor, Color finalColor)
			: base(drawArea)
		{
			Color = startColor;
			FinalColor = finalColor;
			UpdateCorners();
			OnDraw<GradientRectRenderer>();
		}

		public readonly List<Vector2D> Points = new List<Vector2D>();
		public Color FinalColor { get; private set; }

		private void UpdateCorners()
		{
			Points.Clear();
			Points.AddRange(DrawArea.GetRotatedRectangleCorners(RotationCenter, Rotation));
		}

		protected override void NextUpdateStarted()
		{
			base.NextUpdateStarted();
			if (DidFootprintChange)
				UpdateCorners();
		}

		public class GradientRectRenderer : DrawBehavior
		{
			public GradientRectRenderer(Drawing draw)
			{
				this.draw = draw;
				material = new Material(Shader.Position2DColor, "");
			}

			private readonly Drawing draw;
			private readonly Material material;

			private void AddToBatch(GradientFilledRect entity)
			{
				var startColor = entity.Color;
				var finalColor = entity.FinalColor;
				vertices[0] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(entity.Points[0]), startColor);
				vertices[1] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(entity.Points[1]), finalColor);
				vertices[2] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(entity.Points[2]), finalColor);
				vertices[3] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(entity.Points[3]), startColor);
				draw.Add(material, vertices, Indices);
			}

			private readonly VertexPosition2DColor[] vertices = new VertexPosition2DColor[4];
			private static readonly short[] Indices = new short[] { 0, 1, 2, 0, 2, 3 };

			public void Draw(List<DrawableEntity> visibleEntities)
			{
				foreach (var entity in visibleEntities.OfType<GradientFilledRect>())
					AddToBatch(entity);
			}
		}
	}
}