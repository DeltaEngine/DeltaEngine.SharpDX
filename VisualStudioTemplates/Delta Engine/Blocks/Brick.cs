using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Sprites;

namespace $safeprojectname$
{
	public class Brick : Sprite
	{
		public Brick(Material material, Point offset, Orientation displayMode) : base(material, 
			Rectangle.Zero)
		{
			Offset = offset;
			RenderLayer = (int)Blocks.RenderLayer.Grid;
			this.displayMode = displayMode;
		}

		public Point Offset;
		private readonly Orientation displayMode;

		public void UpdateDrawArea()
		{
			Point newPoint;
			if (displayMode == Orientation.Landscape)
			{
				newPoint = OffsetLandscape + (Position - Point.UnitY) * ZoomLandscape;
				DrawArea = NewDrawArea(newPoint, SizeLandscape);
			} else
			{
				newPoint = OffsetPortrait + (Position - Point.UnitY) * ZoomPortrait;
				DrawArea = new Rectangle(newPoint, SizePortrait);
			}
		}

		private static Rectangle NewDrawArea(Point point, Size size)
		{
			return new Rectangle(point, size);
		}

		public Point Position
		{
			get
			{
				return TopLeftGridCoord + Offset;
			}
		}

		public Point TopLeftGridCoord;
		public static readonly Point OffsetLandscape = new Point(0.38f, 0.385f);
		public static readonly Point OffsetPortrait = new Point(0.38f, 0.385f);
		public const float ZoomLandscape = 0.02f;
		public const float ZoomPortrait = 0.02f;
		private static readonly Size SizeLandscape = new Size(ZoomLandscape);
		private static readonly Size SizePortrait = new Size(ZoomPortrait);
	}
}