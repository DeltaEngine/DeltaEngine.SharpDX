using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;

namespace Insight
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
			TopSegment = new Sprite(new Material(Shader.Position2DColorUV, "InsightAppMainBg"),
				Rectangle.FromCenter(new Vector2D(0.5f, 0.355f), new Size(0.565f, 0.70f)));
		}

		public Sprite TopSegment { get; set; }

		public void AddUserStatsButton()
		{
			userStatsDrawArea = Rectangle.FromCenter(new Vector2D(0.362f, 0.85f),
				new Size(0.260f, 0.255f));
			UserStatButton =
				new Sprite(new Material(Shader.Position2DColorUV, "InsightAppButtonUserStats"),
					userStatsDrawArea);
		}

		private Rectangle userStatsDrawArea;
		public Sprite UserStatButton { get; set; }

		public void AddTopLocationButton()
		{
			topLocDrawArea = Rectangle.FromCenter(new Vector2D(0.638f, 0.85f), new Size(0.260f, 0.255f));
			TopLocationButton =
				new Sprite(new Material(Shader.Position2DColorUV, "InsightAppButtonTopLocation"),
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
			TopSegment.Material = new Material(Shader.Position2DColorUV, "InsightAppSubUserStatsBg");
			UserStatButton.Material = new Material(Shader.Position2DColorUV,
				"InsightAppButtonUserStatsPressed");
			TopLocationButton.Material = new Material(Shader.Position2DColorUV,
				"InsightAppButtonTopLocation");
		}

		private bool IsClickInTopLocationButton(Vector2D position)
		{
			return topLocDrawArea.Contains(position);
		}

		private void AddTopLocationButtonAction()
		{
			TopSegment.Material = new Material(Shader.Position2DColorUV, "InsightAppSubTopLocationBG");
			UserStatButton.Material = new Material(Shader.Position2DColorUV, "InsightAppButtonUserStats");
			TopLocationButton.Material = new Material(Shader.Position2DColorUV,
				"InsightAppButtonTopLocationsPressed");
		}
	}
}