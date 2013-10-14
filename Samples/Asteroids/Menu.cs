using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	class Menu : Scene
	{
		public Menu() 
		{
			CreateMenuTheme();
			AddStartButton();
			AddQuitButton();
			AddHighScores();
		}

		private void CreateMenuTheme()
		{
			menuTheme = new Theme();
			menuTheme.Button = new Theme.Appearance(ContentLoader.Load<Material>("ButtonDefault"));
			menuTheme.ButtonMouseover = new Theme.Appearance(ContentLoader.Load<Material>("ButtonHover"));
			menuTheme.ButtonPressed = new Theme.Appearance(ContentLoader.Load<Material>("ButtonPressed"));
			menuTheme.ButtonDisabled = new Theme.Appearance();
			SetQuadraticBackground("AsteroidsMainMenuBackground");
			AddMenuLogo();
		}

		private void AddMenuLogo()
		{
			var material = new Material(Shader.Position2DColorUV, "AsteroidsMainMenuLogo");
			Add(new Sprite(material, Rectangle.FromCenter(0.5f, ScreenSpace.Current.Top + 0.16f, 0.8f,0.2f)));
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.3f, 0.4f, 0.1f), "Start Game");
			startButton.Clicked += TryInvokeGameStart;
			Add(startButton);
		}

		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
		}

		public event Action InitGame;

		private void AddQuitButton()
		{
			var quitButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.15f, 0.4f, 0.1f), "Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			Add(quitButton);
		}

		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;

		private void AddHighScores()
		{
			scoreBoard = new FontText(ContentLoader.Load<Font>("Verdana12"), " ", new Vector2D(0.8f, 0.6f));
			Add(scoreBoard);
		}

		private FontText scoreBoard;

		public void UpdateHighscoreDisplay(int[] highscores)
		{
			var scoreboardText = "Highscores";
			for (int i = 0; i < highscores.Length; i++)
				scoreboardText += "\n" + (i + 1).ToString() + ": " + highscores[i];
			scoreBoard.Text = scoreboardText;
		}
	}
}
