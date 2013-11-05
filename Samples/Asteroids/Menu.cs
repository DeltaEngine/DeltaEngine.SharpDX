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
			SetViewportBackground("AsteroidsMainMenuBackground");
			AddMenuLogo();
		}

		private void CreateMenuTheme()
		{
			menuTheme = new Theme
			{
				Button = ContentLoader.Load<Material>("ButtonDefault"),
				ButtonMouseover = ContentLoader.Load<Material>("ButtonHover"),
				ButtonPressed = ContentLoader.Load<Material>("ButtonPressed")
			};
		}

		private void AddMenuLogo()
		{
			var material = new Material(Shader.Position2DColorUV, "AsteroidsMainMenuLogo");
			var gameLogo= new Sprite(material,
				Rectangle.FromCenter(0.5f, ScreenSpace.Current.Top + 0.2f, 0.8f, 0.2f));
			gameLogo.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(gameLogo);
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new Button(menuTheme,
				new Rectangle(0.25f, ScreenSpace.Current.Top + 0.34f, 0.5f, 0.12f), "Start Game");
			startButton.Clicked += TryInvokeGameStart;
			startButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(startButton);
		}

		//ncrunch: no coverage start
		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
		}
		//ncrunch: no coverage end

		public event Action InitGame;

		private void AddHowToPlay()
		{
			var howToButton = new Button(menuTheme,
				new Rectangle(0.25f, ScreenSpace.Current.Top + 0.46f, 0.5f, 0.12f), "How To Play");
			howToButton.Clicked += ShowHowToPlaySubMenu;
			howToButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(howToButton);
		}

		//ncrunch: no coverage start
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
				SetViewportBackground("AsteroidsMainMenuBackground");
				AddControlDescription();
				AddBackButton();
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
					new Rectangle(0.25f, ScreenSpace.Current.Bottom - 0.2f, 0.5f, 0.12f), "Back");
				backButton.RenderLayer = renderLayer + 1;
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}
		}
		//ncrunch: no coverage end

		private void AddHighscores()
		{
			var highscoreButton = new Button(menuTheme,
				new Rectangle(0.25f, ScreenSpace.Current.Top + 0.58f, 0.5f, 0.12f), "Highscores");
			highscoreButton.Clicked += ShowHighScoresSubMenu;
			highscoreButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(highscoreButton);
		}

		//ncrunch: no coverage start
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
				SetViewportBackground("AsteroidsMainMenuBackground");
				scoreboard = new FontText(Font.Default, "", Vector2D.Half);
				scoreboard.RenderLayer = renderLayer + 2;
				Add(scoreboard);
				AddBackButton();
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
					new Rectangle(0.25f, ScreenSpace.Current.Bottom - 0.2f, 0.5f, 0.12f), "Back");
				backButton.RenderLayer = renderLayer + 1;
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}
		}
		//ncrunch: no coverage end

		private void AddQuitButton()
		{
			var quitButton = new Button(menuTheme,
				new Rectangle(0.25f, ScreenSpace.Current.Top + 0.7f, 0.5f, 0.12f), "Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			quitButton.RenderLayer = (int)AsteroidsRenderLayer.UserInterface;
			Add(quitButton);
		}

		//ncrunch: no coverage start
		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}
		//ncrunch: no coverage end


		public event Action QuitGame;

		public void UpdateHighscoreDisplay(int[] highscores)
		{
			ScoreboardText = "Highscores\n";
			for (int i = 0; i < highscores.Length; i++)
				ScoreboardText += "\n" + (i + 1).ToString(CultureInfo.InvariantCulture) + ": " +
					highscores[i];
		}

		public string ScoreboardText { get; private set; }
	}
}