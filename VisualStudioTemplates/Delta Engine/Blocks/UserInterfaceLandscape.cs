using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;

namespace $safeprojectname$
{
	public class UserInterfaceLandscape : Scene
	{
		public UserInterfaceLandscape(BlocksContent content)
		{
			this.content = content;
			AddBackground();
			AddGrid();
			AddScoreWindow();
			AddScore();
		}

		private readonly BlocksContent content;

		private void AddBackground()
		{
			var image = content.Load<Image>("Background");
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUV);
			var material = new Material(shader, image);
			Add(new Sprite(material, Rectangle.One) {
				RenderLayer = Background
			});
		}

		private const int Background = (int)RenderLayer.Background;

		private void AddGrid()
		{
			var image = content.Load<Image>("Grid");
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUV);
			var material = new Material(shader, image);
			grid = new Sprite(material, GetGridDrawArea()) {
				RenderLayer = Background
			};
			Add(grid);
		}

		private Sprite grid;

		private static Rectangle GetGridDrawArea()
		{
			var left = Brick.OffsetLandscape.X + GridRenderLeftOffset;
			var top = Brick.OffsetLandscape.Y - Brick.ZoomLandscape + GridRenderTopOffset;
			const float Width = Grid.Width * Brick.ZoomLandscape + GridRenderWidthOffset;
			const float Height = (Grid.Height + 1) * Brick.ZoomLandscape + GridRenderHeightOffset;
			return new Rectangle(left, top, Width, Height);
		}

		private const float GridRenderLeftOffset = -0.009f;
		private const float GridRenderTopOffset = -0.009f;
		private const float GridRenderWidthOffset = 0.019f;
		private const float GridRenderHeightOffset = 0.018f;

		private void AddScoreWindow()
		{
			var image = content.Load<Image>("ScoreWindow");
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUV);
			var material = new Material(shader, image);
			scoreWindow = new Sprite(material, GetScoreWindowDrawArea(material.DiffuseMap.PixelSize));
			scoreWindow.RenderLayer = Background;
			Add(scoreWindow);
		}

		private Sprite scoreWindow;

		private static Rectangle GetScoreWindowDrawArea(Size size)
		{
			var left = Brick.OffsetLandscape.X + GridRenderLeftOffset;
			var top = Brick.OffsetLandscape.Y - Brick.ZoomLandscape + ScoreRenderTopOffset;
			const float Width = Grid.Width * Brick.ZoomLandscape + GridRenderWidthOffset;
			var height = Width / size.AspectRatio;
			return new Rectangle(left, top, Width, height);
		}

		private const float ScoreRenderTopOffset = -0.135f;

		private void AddScore()
		{
			Text = new FontText(ContentLoader.Load<Font>("Verdana12"), "", scoreWindow.DrawArea) {
				RenderLayer = (int)RenderLayer.Foreground
			};
		}

		internal FontText Text
		{
			get;
			private set;
		}

		public void ResizeInterface()
		{
			grid.DrawArea = GetGridDrawArea();
			scoreWindow.DrawArea = GetScoreWindowDrawArea(scoreWindow.Material.DiffuseMap.PixelSize);
		}
	}
}