using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;

namespace Blocks
{
	/// <summary>
	/// Renders the static elements (background, grid, score) in portrait plus keeps track of and 
	/// renders the player score.
	/// </summary>
	public class UserInterfacePortrait : Scene
	{
		public UserInterfacePortrait(BlocksContent content)
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
			var material = new Material(shader, image, image.PixelSize);
			SetViewportBackground(material);
		}

		private const int Background = (int)RenderLayer.Background;

		private void AddGrid()
		{
			var image = content.Load<Image>("Grid");
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUV);
			var material = new Material(shader, image, image.PixelSize);
			grid = new Sprite(material, GetGridDrawArea()) { RenderLayer = Background };
			Add(grid);
		}

		private Sprite grid;

		private static Rectangle GetGridDrawArea()
		{
			var left = Brick.OffsetPortrait.X + GridRenderLeftOffset;
			var top = Brick.OffsetPortrait.Y - Brick.ZoomPortrait + GridRenderTopOffset;
			return new Rectangle(left, top, Width, Height);
		}

		private const float GridRenderLeftOffset = -0.009f;
		private const float GridRenderTopOffset = -0.009f;
		private const float GridRenderWidthOffset = 0.019f;
		private const float Width = Grid.Width * Brick.ZoomPortrait + GridRenderWidthOffset;
		private const float Height = (Grid.Height + 1) * Brick.ZoomPortrait + GridRenderHeightOffset;
		private const float GridRenderHeightOffset = 0.018f;

		private void AddScoreWindow()
		{
			var image = content.Load<Image>("ScoreWindow");
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUV);
			var material = new Material(shader, image, image.PixelSize);
			scoreWindow = new Sprite(material, GetScoreWindowDrawArea(material.DiffuseMap.PixelSize));
			scoreWindow.RenderLayer = Background;
			Add(scoreWindow);
		}

		private Sprite scoreWindow;

		private static Rectangle GetScoreWindowDrawArea(Size size)
		{
			var left = Brick.OffsetPortrait.X + GridRenderLeftOffset;
			var top = Brick.OffsetPortrait.Y - Brick.ZoomPortrait + ScoreRenderTopOffset;
			var height = Width / size.AspectRatio;
			return new Rectangle(left, top, Width, height);
		}

		private void AddScore()
		{
			Text = new FontText(ContentLoader.Load<Font>("Verdana12"), "", scoreWindow.DrawArea)
			{
				RenderLayer = (int)RenderLayer.Foreground + 6
			};
		}

		internal FontText Text { get; private set; }

		private const float ScoreRenderTopOffset = -0.135f;
	}
}