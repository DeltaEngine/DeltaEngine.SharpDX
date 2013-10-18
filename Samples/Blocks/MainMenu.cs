using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace Blocks
{
	internal class MainMenu : Scene
	{
		public MainMenu()
		{
			CreateMenuTheme();
			AddStartButton();
			AddHowToPlay();
			AddQuitButton();
		}

		private void CreateMenuTheme()
		{
			SetQuadraticBackground("BlocksMainMenuBackground");
			menuTheme = new Theme();
			menuTheme.Button =
				new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonDefault"));
			menuTheme.ButtonMouseover =
				new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonHover"));
			menuTheme.ButtonPressed =
				new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonPressed"));
			menuTheme.ButtonDisabled = new Theme.Appearance();
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.4f, 0.4f, 0.15f),
				"Start Game");
			startButton.Clicked += TryInvokeGameStart;
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
			var howToButton = new Button(menuTheme, new Rectangle(0.3f, 0.6f, 0.4f, 0.15f),
				"How To Play");
			howToButton.Clicked += ShowHowToPlaySubMenu;
			Add(howToButton);
		}

		private void ShowHowToPlaySubMenu()
		{
			if (howToPlay == null)
				howToPlay = new HowToPlaySubMenu(this, menuTheme);
			howToPlay.Show();
			Hide();
		}

		private HowToPlaySubMenu howToPlay;

		private sealed class HowToPlaySubMenu : Scene
		{
			public HowToPlaySubMenu(Scene parent, Theme menuTheme)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				SetQuadraticBackground("BlocksMainMenuBackground");
				AddControlDescription();
				AddBackButton();
				Hide();
			}

			private readonly Theme menuTheme;
			private readonly Scene parent;

			private void AddControlDescription()
			{
				const string DescriptionText =
					"Fruit Blocks - Manual\n\n" +
					"Move Block Left - Cursor left or click left next to the playing field\n" +
					"Move Block Right - Cursor right or click right next to the playing field\n" +
					"Move Block Down - Cursor down or click below the Fruit Block\n" +
					"Turn Block - Cursor up or click above the Fuit Block\n";
				Add(new FontText(Font.Default, DescriptionText, Vector2D.Half));
			}

			private void AddBackButton()
			{
				var backButton = new Button(menuTheme,
					new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.15f, 0.4f, 0.08f), "Back");
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
			var quitButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.8f, 0.4f, 0.15f),
				"Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			Add(quitButton);
		}

		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;
	}
}