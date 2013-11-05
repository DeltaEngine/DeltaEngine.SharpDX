using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class MenuWithouScenes
	{
		public MenuWithouScenes()
		{
			AddTopSegment();
			AddUserStatsButton();
			AddTopLocationButton();
			AddInputCommands();
		}

		private void AddTopSegment()
		{
			TopSegment = new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppMainBg"),
				Rectangle.FromCenter(new Vector2D(0.5f, 0.355f), new Size(0.565f, 0.70f)));
		}

		public Sprite TopSegment { get; set; }

		public void AddUserStatsButton()
		{
			userStatsDrawArea = Rectangle.FromCenter(new Vector2D(0.362f, 0.85f),
				new Size(0.260f, 0.255f));
			UserStatButton =
				new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppButtonUserStats"),
					userStatsDrawArea);
		}

		private Rectangle userStatsDrawArea;
		public Sprite UserStatButton { get; set; }

		public void AddTopLocationButton()
		{
			topLocDrawArea = Rectangle.FromCenter(new Vector2D(0.638f, 0.85f), new Size(0.260f, 0.255f));
			TopLocationButton =
				new Sprite(new Material(Shader.Position2DColorUV, "$safeprojectname$AppButtonTopLocation"),
					topLocDrawArea);
		}

		private Rectangle topLocDrawArea;
		public Sprite TopLocationButton { get; set; }

		private void AddInputCommands()
		{
			new Command(Command.Click, ButtonClick);
		}

		private void ButtonClick(Vector2D position)
		{
			if (IsClickInUserStatsButton(position))
				AddUserStatButtonAction();
			else if (IsClickInTopLocationButton(position))
				AddTopLocationButtonAction();
		}

		private bool IsClickInUserStatsButton(Vector2D position)
		{
			return userStatsDrawArea.Contains(position);
		}

		private void AddUserStatButtonAction()
		{
			TopSegment.Material = new Material(Shader.Position2DColorUV, "$safeprojectname$AppSubUserStatsBg");
			UserStatButton.Material = new Material(Shader.Position2DColorUV,
				"$safeprojectname$AppButtonUserStatsPressed");
			TopLocationButton.Material = new Material(Shader.Position2DColorUV,
				"$safeprojectname$AppButtonTopLocation");
		}

		private bool IsClickInTopLocationButton(Vector2D position)
		{
			return topLocDrawArea.Contains(position);
		}

		private void AddTopLocationButtonAction()
		{
			TopSegment.Material = new Material(Shader.Position2DColorUV, "$safeprojectname$AppSubTopLocationBG");
			UserStatButton.Material = new Material(Shader.Position2DColorUV, "$safeprojectname$AppButtonUserStats");
			TopLocationButton.Material = new Material(Shader.Position2DColorUV,
				"$safeprojectname$AppButtonTopLocationsPressed");
		}
	}
}