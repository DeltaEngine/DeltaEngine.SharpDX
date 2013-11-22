using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class StatisticsApp
	{
		public StatisticsApp(Window window)
		{
			window.BackgroundColor = Color.White;
			window.Title = "Insight";
			CreateAppMenu();
		}

		public void CreateAppMenu()
		{
			topSegment = new Sprite(MainBg, Rectangle.Zero);
			userStats = new Sprite(UserStatsMaterial, Rectangle.Zero);
			topLocation = new Sprite(TopLocationMaterial, Rectangle.Zero);
			PositionControls();
			new Command(Command.Click, ButtonClick);
			ScreenSpace.Current.ViewportSizeChanged += PositionControls;
		}

		private Sprite topSegment;
		private Sprite userStats;
		private Sprite topLocation;

		private static readonly Material MainBg = new Material(Shader.Position2DColorUV, "MainBg");
		private static readonly Material UserStatsMaterial = new Material(Shader.Position2DColorUV,
			"ButtonUserStats");
		private static readonly Material TopLocationMaterial = new Material(
			Shader.Position2DColorUV, "ButtonTopLocation");

		private void PositionControls()
		{
			var height = ScreenSpace.Current.Viewport.Height;
			var width = ScreenSpace.Current.Viewport.Width;
			var topBottomBorder = height * 0.02f;
			var rightLeftBorder = width * 0.025f;
			var top = ScreenSpace.Current.Viewport.Top;
		
			var drawArea = new Rectangle(0.5f - height * 0.2825f + rightLeftBorder,
				top + topBottomBorder, height * 0.53675f, height * 0.665f);
			topSegment.LastDrawArea = topSegment.DrawArea = drawArea;
			userStats.LastDrawArea =
				userStats.DrawArea =
					new Rectangle(drawArea.Left, drawArea.Bottom + topBottomBorder, height * 0.260f,
						height * 0.255f);
			topLocation.LastDrawArea =
				topLocation.DrawArea =
					new Rectangle(drawArea.Left + height * 0.260f + 0.9f * topBottomBorder,
						drawArea.Bottom + topBottomBorder, height * 0.260f, height * 0.255f);
		}

		private void ButtonClick(Vector2D position)
		{
			if (userStats.DrawArea.Contains(position))
				ClickUserStatButton();
			else if (topLocation.DrawArea.Contains(position))
				ClickTopLocationButton();
		}

		private void ClickUserStatButton()
		{
			topSegment.Material = TopSegmentUserStatsMaterial;
			userStats.Material = UserStatButtonPressedMaterial;
			topLocation.Material = TopLocationMaterial;
		}

		private static readonly Material TopSegmentUserStatsMaterial =
			new Material(Shader.Position2DColorUV, "UserStatsBg");

		private static readonly Material UserStatButtonPressedMaterial =
			new Material(Shader.Position2DColorUV, "ButtonUserStatsPressed");

		private void ClickTopLocationButton()
		{
			topSegment.Material = TopSegmentTopLocationMaterial;
			userStats.Material = UserStatsMaterial;
			topLocation.Material = TopLocationButtonPressedMaterial;
		}

		private static readonly Material TopSegmentTopLocationMaterial =
			new Material(Shader.Position2DColorUV, "TopLocationBg");
		private static readonly Material TopLocationButtonPressedMaterial =
			new Material(Shader.Position2DColorUV, "ButtonTopLocationsPressed");
	}
}