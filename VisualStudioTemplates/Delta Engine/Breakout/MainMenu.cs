using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	internal class MainMenu : Scene
	{
		public MainMenu()
		{
			SetUpTheme();
			AddGameLogo();
			AddStartButton();
			AddHowToPlay();
			AddOptions();
			AddQuitButton();
		}

		private void AddGameLogo()
		{
			Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"), 
				Rectangle.FromCenter(0.5f, ScreenSpace.Current.Viewport.Top + 0.2f, 0.7f, 0.3f)));
		}

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(startGameTheme, new Rectangle(0.3f, 
				ScreenSpace.Current.Bottom - 0.6f, 0.4f, 0.15f));
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
			var howToButton = new InteractiveButton(howToPlayTheme, new Rectangle(0.3f, 
				ScreenSpace.Current.Viewport.Bottom - 0.46f, 0.4f, 0.15f));
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
			public HowToPlaySubMenu(Scene parent, Theme backTheme)
			{
				this.parent = parent;
				this.backTheme = backTheme;
				Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"), 
					Rectangle.FromCenter(0.5f, ScreenSpace.Current.Viewport.Top + 0.2f, 0.7f, 0.3f)));
				SetQuadraticBackground("Background");
				AddControlDescription();
				AddBackButton();
				Hide();
			}

			private readonly Scene parent;
			private readonly Theme backTheme;

			private void AddControlDescription()
			{
				string descriptionText = "Breakout - Manual\n\n";
				descriptionText += "Breakout - Manual\n\n";
				descriptionText += "Move Left - Cursor left or drag left holding the mouse button\n";
				descriptionText += "Move Right - Cursor right or drag right holding the mouse button\n";
				descriptionText += "Start the ball - Space or left mouse button\n";
				var howToDisplayText = new FontText(Font.Default, descriptionText, Vector2D.Half);
				Add(howToDisplayText);
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(backTheme, new Rectangle(0.3f, 
					ScreenSpace.Current.Bottom - 0.2f, 0.4f, 0.15f));
				backButton.Clicked += () => 
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}
		}
		private void AddOptions()
		{
			var optionsButton = new InteractiveButton(optionsTheme, new Rectangle(0.3f, 
				ScreenSpace.Current.Viewport.Bottom - 0.32f, 0.4f, 0.15f));
			optionsButton.Clicked += ShowOptionsSubmenu;
			Add(optionsButton);
		}

		private void ShowOptionsSubmenu()
		{
			if (options == null)
				options = new OptionSubmenu(this, menuTheme);

			options.Show();
			Hide();
		}

		private OptionSubmenu options;
		private class OptionSubmenu : Scene
		{
			public OptionSubmenu(MainMenu parent, Theme menuTheme)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				SetQuadraticBackground("Background");
				Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"), 
					Rectangle.FromCenter(0.5f, ScreenSpace.Current.Viewport.Top + 0.2f, 0.7f, 0.3f)));
				AddMusicOption();
				AddSoundOption();
				AddBackButton();
				Hide();
			}

			private readonly MainMenu parent;
			private readonly Theme menuTheme;

			private void AddMusicOption()
			{
				var labelTheme = new Theme();
				labelTheme.Label = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
					"MusicLabel"));
				var label = new Label(labelTheme, Rectangle.FromCenter(0.3f, 
					ScreenSpace.Current.Viewport.Top + 0.46f, 0.2f, 0.1f));
				Add(label);
				var musicSlider = new Slider(menuTheme, Rectangle.FromCenter(0.6f, 
					ScreenSpace.Current.Viewport.Top + 0.46f, 0.4f, 0.05f));
				musicSlider.MaxValue = 100;
				musicSlider.MinValue = 0;
				musicSlider.Value = (int)(Settings.Current.MusicVolume * 100);
				musicSlider.ValueChanged += val => 
				{
					Settings.Current.MusicVolume = val / 100.0f;
				};
				Add(musicSlider);
			}

			private void AddSoundOption()
			{
				var labelTheme = new Theme();
				labelTheme.Label = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
					"SoundLabel"));
				var label = new Label(labelTheme, Rectangle.FromCenter(0.3f, 
					ScreenSpace.Current.Viewport.Top + 0.6f, 0.2f, 0.1f));
				Add(label);
				var soundSlider = new Slider(menuTheme, Rectangle.FromCenter(0.6f, 
					ScreenSpace.Current.Viewport.Top + 0.6f, 0.4f, 0.05f));
				soundSlider.MaxValue = 100;
				soundSlider.MinValue = 0;
				soundSlider.Value = (int)(Settings.Current.SoundVolume * 100);
				soundSlider.ValueChanged += val => 
				{
					Settings.Current.SoundVolume = val / 100.0f;
				};
				Add(soundSlider);
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 
					ScreenSpace.Current.Bottom - 0.2f, 0.4f, 0.15f));
				backButton.Clicked += () => 
				{
					Hide();
					parent.InvokeSettingsChanged();
					parent.Show();
				};
				Add(backButton);
			}
		}
		private void InvokeSettingsChanged()
		{
			if (SettingsChanged != null)
				SettingsChanged();
		}

		public event Action SettingsChanged;

		private void AddQuitButton()
		{
			var quitButton = new InteractiveButton(exitTheme, new Rectangle(0.3f, 
				ScreenSpace.Current.Viewport.Bottom - 0.18f, 0.4f, 0.15f));
			quitButton.Clicked += TryInvokeQuit;
			Add(quitButton);
		}

		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;

		private void SetUpTheme()
		{
			SetQuadraticBackground("Background");
			menuTheme = new Theme();
			menuTheme.Button = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"BackButtonDefault"));
			menuTheme.ButtonMouseover = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"BackButtonHover"));
			menuTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"BackButtonHover"));
			menuTheme.Slider = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"SliderBackground"));
			menuTheme.SliderPointer = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"SliderDefault"));
			menuTheme.SliderPointerMouseover = new Theme.Appearance(new 
				Material(Shader.Position2DColorUV, "SliderHover"));
			startGameTheme = new Theme();
			startGameTheme.Button = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"StartGameButtonDefault"));
			startGameTheme.ButtonMouseover = new Theme.Appearance(new 
				Material(Shader.Position2DColorUV, "StartGameButtonHover"));
			startGameTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"StartGameButtonHover"));
			optionsTheme = new Theme();
			optionsTheme.Button = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"OptionsButtonDefault"));
			optionsTheme.ButtonMouseover = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"OptionsButtonHover"));
			optionsTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"OptionsButtonHover"));
			optionsTheme.Label = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"OptionsButtonDefault"));
			exitTheme = new Theme();
			exitTheme.Button = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"ExitButtonDefault"));
			exitTheme.ButtonMouseover = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"ExitButtonHover"));
			exitTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"ExitButtonHover"));
			howToPlayTheme = new Theme();
			howToPlayTheme.Button = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"HowToPlayButtonDefault"));
			howToPlayTheme.ButtonMouseover = new Theme.Appearance(new 
				Material(Shader.Position2DColorUV, "HoWToPlayButtonHover"));
			howToPlayTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DColorUV, 
				"HowtoPlayButtonHover"));
		}

		private Theme menuTheme;
		private Theme startGameTheme;
		private Theme optionsTheme;
		private Theme exitTheme;
		private Theme howToPlayTheme;
		private Theme backTheme;
	}
}