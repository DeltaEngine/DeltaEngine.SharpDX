using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$
{
	public class MenuWithScenes
	{
		public MenuWithScenes()
		{
			AddTopSegment();
			AddUserStatsButton();
			AddTopLocationButton();
		}

		private void AddTopSegment()
		{
			TopSegment = new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppMainBg"),
				Rectangle.FromCenter(new Vector2D(0.5f, 0.355f), new Size(0.565f, 0.70f)));
		}

		public Sprite TopSegment { get; set; }

		public void AddUserStatsButton()
		{
			var drawArea = Rectangle.FromCenter(new Vector2D(0.362f, 0.85f), new Size(0.260f, 0.255f));
			UserStatButton = new Button(CreateButtonTheme("$safeprojectname$AppButtonUserStats"), drawArea);
			UserStatsButtonPressed =
				new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppButtonUserStatsPressed"),
					drawArea);
			UserStatsButtonPressed.IsVisible = false;
			AddUserStatButtonAction(drawArea);
		}

		public Sprite UserStatsButtonPressed { get; private set; }

		public Entity2D UserStatButton { get; set; }

		private static Theme CreateButtonTheme(string defaultState)
		{
			return new Theme
			{
				Button = new Material(Shader.Position2DColorUV, defaultState),
				ButtonPressed = new Material(Shader.Position2DColorUV, defaultState),
				ButtonMouseover = new Material(Shader.Position2DColorUV, defaultState),
				ButtonDisabled = new Material(Shader.Position2DColorUV, defaultState)
			};
		}

		private void AddUserStatButtonAction(Rectangle drawArea)
		{
			((Button)UserStatButton).Clicked += () =>
			{
				TopSegment.Material = new Material(Shader.Position2DColorUV, "$safeprojectname$AppSubUserStatsBg");
				UserStatButton.IsVisible = false;
				UserStatsButtonPressed.IsVisible = true;
				TopLocationButton.IsVisible = true;
				TopLocationButtonPressed.IsVisible = false;
			};
		}

		public void AddTopLocationButton()
		{
			var drawArea = Rectangle.FromCenter(new Vector2D(0.638f, 0.85f), new Size(0.260f, 0.255f));
			TopLocationButton = new Button(CreateButtonTheme("$safeprojectname$AppButtonTopLocation"), drawArea);
			TopLocationButtonPressed =
				new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppButtonTopLocationsPressed"),
					drawArea);
			TopLocationButtonPressed.IsVisible = false;
			AddTopLocationButtonAction();
		}

		public Sprite TopLocationButtonPressed { get; private set; }

		public Entity2D TopLocationButton { get; set; }

		private void AddTopLocationButtonAction()
		{
			((Button)TopLocationButton).Clicked += () =>
			{
				TopSegment.Material = new Material(Shader.Position2DColorUV, "$safeprojectname$AppSubTopLocationBG");
				UserStatButton.IsVisible = true;
				UserStatsButtonPressed.IsVisible = false;
				TopLocationButton.IsVisible = false;
				TopLocationButtonPressed.IsVisible = true;
			};
		}
	}
}