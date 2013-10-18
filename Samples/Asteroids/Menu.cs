using System;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	internal class Menu : Scene
	{
		public Menu()
		{
			CreateMenuTheme();
			AddStartButton();
			AddHowToPlay();
			AddQuitButton();
			AddHighscores();
			SetQuadraticBackground("AsteroidsMainMenuBackground");
			AddMenuLogo();
		}

		private void CreateMenuTheme()
		{
			menuTheme = new Theme();
			menuTheme.Button = new Theme.Appearance(ContentLoader.Load<Material>("ButtonDefault"));
			menuTheme.ButtonMouseover = new Theme.Appearance(ContentLoader.Load<Material>("ButtonHover"));
			menuTheme.ButtonPressed = new Theme.Appearance(ContentLoader.Load<Material>("ButtonPressed"));
			menuTheme.ButtonDisabled = new Theme.Appearance();

		}

		private void AddMenuLogo()
		{
			var material = new Material(Shader.Position2DColorUV, "AsteroidsMainMenuLogo");
			var gameLogo= new Sprite(material,
				Rectangle.FromCenter(0.5f, ScreenSpace.Current.Top + 0.1f, 0.8f, 0.2f));
			gameLogo.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(gameLogo);
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new Button(menuTheme,
				new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.4f, 0.4f, 0.08f), "Start Game");
			startButton.Clicked += TryInvokeGameStart;
			startButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(startButton);
		}

		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
		}

		public event Action InitGame;

		private void AddHowToPlay()
		{
			var howToButton = new Button(menuTheme,
				new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.3f, 0.4f, 0.08f), "How To Play");
			howToButton.Clicked += ShowHowToPlaySubMenu;
			howToButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(howToButton);
		}

		private void ShowHowToPlaySubMenu()
		{
			if (howToPlay == null)
				howToPlay = new HowToPlaySubMenu(this, menuTheme,
					(int)AsteroidsRenderLayer.UserInterface + 10);
			howToPlay.Show();
			Hide();
		}

		private HowToPlaySubMenu howToPlay;

		private sealed class HowToPlaySubMenu : Scene
		{
			public HowToPlaySubMenu(Scene parent, Theme menuTheme, int renderLayer)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				this.renderLayer = renderLayer;
				SetQuadraticBackground("AsteroidsMainMenuBackground");
				AddControlDescription();
				AddBackButton();
				Hide();
			}

			private readonly Theme menuTheme;
			private readonly int renderLayer;
			private readonly Scene parent;

			private void AddControlDescription()
			{
				var descriptionText = "Pilot's Manual\nDelta Alpha I - Standard Issue Missile Ship\n\n";
				descriptionText += "Accelerate - W, cursor up or left analogue stick forward\n";
				descriptionText += "Turn Left - A, cursor left or left analogue stick left\n";
				descriptionText += "Turn Right - D, cursor right or left analogue stick right\n\n";
				descriptionText += "Fire Missiles - Hold Space or the key A on a controller";
				var howToDisplayText = new FontText(Font.Default, descriptionText, Vector2D.Half);
				howToDisplayText.RenderLayer = renderLayer + 2;
				Add(howToDisplayText);
			}

			private void AddBackButton()
			{
				var backButton = new Button(menuTheme,
					new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.15f, 0.4f, 0.08f), "Back");
				backButton.RenderLayer = renderLayer + 1;
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}
		}

		private void AddHighscores()
		{
			var highscoreButton = new Button(menuTheme,
				new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.2f, 0.4f, 0.08f), "Highscores");
			highscoreButton.Clicked += ShowHighScoresSubMenu;
			highscoreButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(highscoreButton);
		}

		private void ShowHighScoresSubMenu()
		{
			if (highscore == null)
				highscore = new HighscoreSubMenu(this, menuTheme,
					(int)AsteroidsRenderLayer.UserInterface + 10);
			highscore.UpdateScoreText(ScoreboardText);
			highscore.Show();
			Hide();
		}

		private HighscoreSubMenu highscore;

		private sealed class HighscoreSubMenu : Scene
		{
			public HighscoreSubMenu(Scene parent, Theme menuTheme, int renderLayer)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				this.renderLayer = renderLayer;
				SetQuadraticBackground("AsteroidsMainMenuBackground");
				scoreboard = new FontText(Font.Default, "", Vector2D.Half);
				scoreboard.RenderLayer = renderLayer + 2;
				Add(scoreboard);
				AddBackButton();
				Hide();
			}

			private readonly Theme menuTheme;
			private readonly int renderLayer;
			private readonly Scene parent;
			private readonly FontText scoreboard;

			public void UpdateScoreText(string text)
			{
				if(scoreboard != null)
				scoreboard.Text = text;
			}

			private void AddBackButton()
			{
				var backButton = new Button(menuTheme,
					new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.1f, 0.4f, 0.08f), "Back");
				backButton.RenderLayer = renderLayer + 1;
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}
		}

		private void AddQuitButton()
		{
			var quitButton = new Button(menuTheme,
				new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.1f, 0.4f, 0.08f), "Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			quitButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(quitButton);
		}

		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;

		public void UpdateHighscoreDisplay(int[] highscores)
		{
			ScoreboardText = "Highscores";
			for (int i = 0; i < highscores.Length; i++)
				ScoreboardText += "\n" + (i + 1).ToString(CultureInfo.InvariantCulture) + ": " +
					highscores[i];
		}

		public string ScoreboardText { get; private set; }
	}
}