using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	public class HudInterface
	{
		public HudInterface()
		{
			hudFont = ContentLoader.Load<Font>("Tahoma30");
			ScoreDisplay = new FontText(hudFont, "0",
				new Rectangle(ScreenSpace.Current.Viewport.Left, ScreenSpace.Current.Viewport.Top, 0.1f,
					0.05f));
			ScoreDisplay.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			GameOverText = new FontText(ContentLoader.Load<Font>("Verdana12"), "", Rectangle.FromCenter(0.5f, 0.5f, 0.8f, 0.4f));
			GameOverText.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
		}

		private readonly Font hudFont;
		public FontText ScoreDisplay { get; private set; }

		public void SetScoreText(int score)
		{
			ScoreDisplay.Text = score.ToString(CultureInfo.InvariantCulture);
		}

		public void SetGameOverText()
		{
			GameOverText.Text = "Game Over!\n\n[Space] / Controller (A) - Restart\n[Esc] / Controller (B)- Back to Menu";
			GameOverText.IsVisible = true;
		}

		public FontText GameOverText { get; private set; }

		public void SetIngameMode()
		{
			GameOverText.IsVisible = false;
		}

		public void Dispose()
		{
			GameOverText.IsActive = false;
			ScoreDisplay.IsActive = false;
		}
	}
}