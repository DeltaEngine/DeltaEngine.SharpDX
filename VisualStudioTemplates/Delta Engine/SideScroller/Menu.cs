using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class Menu : Scene
	{
		public Menu()
		{
			CreateMenuTheme();
			AddStartButton();
			AddHowToPlay();
			AddQuitButton();
		}

		private void CreateMenuTheme()
		{
			SetQuadraticBackground("SidescrollerMainMenuBackground");
			menuTheme = new Theme();
			menuTheme.Button = new Theme.Appearance(new Material(Shader.Position2DUV, 
				"SidescrollerButtonDefault"));
			menuTheme.ButtonDisabled = new Theme.Appearance();
			menuTheme.ButtonMouseover = new Theme.Appearance(new Material(Shader.Position2DUV, 
				"SidescrollerButtonHover"));
			menuTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DUV, 
				"SidescrollerButtonPressed"));
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.3f, 0.4f, 0.1f), 
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
			var howToButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.45f, 0.4f, 0.1f), 
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
				SetQuadraticBackground("SidescrollerMainMenuBackground");
				AddControlDescription();
				AddBackButton();
				Hide();
			}

			private readonly Scene parent;
			private readonly Theme menuTheme;

			private void AddControlDescription()
			{
				const string DescriptionText = "SideScroller - Manual\n\n" + "Fly Up - W or cursor up\n" 
					+ "Fly Down - S or cursor down\n" + "Fire - Space";
				var howToDisplayText = new FontText(Font.Default, DescriptionText, Vector2D.Half);
				Add(howToDisplayText);
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 
					ScreenSpace.Current.Bottom - 0.15f, 0.4f, 0.1f), "Back");
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
			var quitButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.6f, 0.4f, 0.1f), 
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