using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace Breakout
{
	public class MainMenu : Scene
	{
		public MainMenu()
		{
			BackSound = ContentLoader.Load<Sound>("PaddleBallStart");
			EnterSound = ContentLoader.Load<Sound>("BallCollision");
			SetUpTheme();
			AddGameLogo();
			AddStartButton();
			AddHowToPlay();
			AddOptions();
			AddQuitButton();
		}

		public Sound BackSound { get; private set; }
		public Sound EnterSound { get; private set; }

		private void AddGameLogo()
		{
			Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"),
				Rectangle.FromCenter(0.5f, 0.2f, 0.7f, 0.3f)));
		}

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(startGameTheme,
				new Rectangle(0.3f, 0.3f, 0.4f, 0.15f));
			startButton.Clicked += TryInvokeGameStart;
			Add(startButton);
		}

		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
			if (!EnterSound.IsAnyInstancePlaying)
				EnterSound.Play();
		}

		public event Action InitGame;

		private void AddHowToPlay()
		{
			var howToButton = new InteractiveButton(howToPlayTheme,
				new Rectangle(0.3f, 0.44f, 0.4f, 0.15f));
			howToButton.Clicked += ShowHowToPlaySubMenu;
			Add(howToButton);
		}

		private void ShowHowToPlaySubMenu()
		{
			if (howToPlay == null)
				howToPlay = new HowToPlaySubMenu(this, menuTheme);
			howToPlay.Show();
			if(!EnterSound.IsAnyInstancePlaying)
				EnterSound.Play();
			Hide();
		}

		private HowToPlaySubMenu howToPlay;

		private sealed class HowToPlaySubMenu : Scene
		{
			public HowToPlaySubMenu(MainMenu parent, Theme backTheme)
			{
				this.parent = parent;
				this.backTheme = backTheme;
				Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"),
					Rectangle.FromCenter(0.5f, ScreenSpace.Current.Viewport.Top + 0.2f, 0.7f, 0.3f)));
				SetViewportBackground("Background");
				AddControlDescription();
				AddBackButton();
			}

			private readonly MainMenu parent;
			private readonly Theme backTheme;

			private void AddControlDescription()
			{
				Add(new Sprite(ContentLoader.Load<Material>("HowToMaterial"),
					Rectangle.FromCenter(0.5f, 0.5f, 0.8f, 0.4f)));
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(backTheme,
					new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.2f, 0.4f, 0.15f));
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
					if(!parent.BackSound.IsAnyInstancePlaying)
						parent.BackSound.Play();
				};
				Add(backButton);
			}
		}

		private void AddOptions()
		{
			var optionsButton = new InteractiveButton(optionsTheme,
				new Rectangle(0.3f, 0.58f, 0.4f, 0.15f));
			optionsButton.Clicked += ShowOptionsSubmenu;
			Add(optionsButton);
		}

		private void ShowOptionsSubmenu()
		{
			if (options == null)
				options = new OptionSubmenu(this, menuTheme);
			options.Show();
			if (!EnterSound.IsAnyInstancePlaying)
				EnterSound.Play();
			Hide();
		}

		private OptionSubmenu options;

		private sealed class OptionSubmenu : Scene
		{
			public OptionSubmenu(MainMenu parent, Theme menuTheme)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				SetViewportBackground("Background");
				Add(new Sprite(new Material(Shader.Position2DColorUV, "BreakoutLogo"),
					Rectangle.FromCenter(0.5f, ScreenSpace.Current.Viewport.Top + 0.2f, 0.7f, 0.3f)));
				AddMusicOption();
				AddSoundOption();
				AddBackButton();
			}

			private readonly MainMenu parent;
			private readonly Theme menuTheme;

			private void AddMusicOption()
			{
				var labelTheme = new Theme();
				labelTheme.Label = new Material(Shader.Position2DColorUV, "MusicLabel");
				var label = new Label(labelTheme,
					Rectangle.FromCenter(0.3f, ScreenSpace.Current.Viewport.Top + 0.46f, 0.2f, 0.1f));
				Add(label);

				var musicSlider = new Slider(menuTheme,
					Rectangle.FromCenter(0.6f, ScreenSpace.Current.Viewport.Top + 0.46f, 0.4f, 0.05f))
				{
					MaxValue = 100,
					MinValue = 0,
					Value = (int)(Settings.Current.MusicVolume * 100)
				};
				//ncrunch: no coverage start
				musicSlider.ValueChanged += val =>
				{
					Settings.Current.MusicVolume = val / 100.0f;
					if (!parent.EnterSound.IsAnyInstancePlaying)
						parent.EnterSound.Play(Settings.Current.MusicVolume);
				};
				//ncrunch: no coverage end
				musicSlider.Start<SettingsUpdater>();
				Add(musicSlider);
			}

			private class SettingsUpdater : UpdateBehavior
			{
				public override void Update(IEnumerable<Entity> entities)
				{
					foreach (var slider in entities.OfType<Slider>())
					{
						if(slider.State.DragDone)
							MainMenu.InvokeSettingsChanged();
					}
				}
			}

			private void AddSoundOption()
			{
				var labelTheme = new Theme();
				labelTheme.Label = new Material(Shader.Position2DColorUV, "SoundLabel");
				var label = new Label(labelTheme,
					Rectangle.FromCenter(0.3f, ScreenSpace.Current.Viewport.Top + 0.6f, 0.2f, 0.1f));
				Add(label);

				var soundSlider = new Slider(menuTheme,
					Rectangle.FromCenter(0.6f, ScreenSpace.Current.Viewport.Top + 0.6f, 0.4f, 0.05f))
				{
					MaxValue = 100,
					MinValue = 0,
					Value = (int)(Settings.Current.SoundVolume * 100)
				};
				//ncrunch: no coverage start
				soundSlider.ValueChanged += val =>
				{
					Settings.Current.SoundVolume = val / 100.0f;
					if (!parent.EnterSound.IsAnyInstancePlaying)
						parent.EnterSound.Play(Settings.Current.SoundVolume);
				};
				//ncrunch: no coverage end
				soundSlider.Start<SettingsUpdater>();
				Add(soundSlider);
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(menuTheme,
					new Rectangle(0.3f, ScreenSpace.Current.Bottom - 0.2f, 0.4f, 0.15f));
				backButton.Clicked += () =>
				{
					Hide();
					MainMenu.InvokeSettingsChanged();
					if(!parent.BackSound.IsAnyInstancePlaying)
						parent.BackSound.Play();
					parent.Show();
				};
				Add(backButton);
			}
		}

		private static void InvokeSettingsChanged()
		{
			if (SettingsChanged != null)
				SettingsChanged();
		}

		public static event Action SettingsChanged;

		private void AddQuitButton()
		{
			var quitButton = new InteractiveButton(exitTheme, new Rectangle(0.3f, 0.72f, 0.4f, 0.15f));
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
			SetViewportBackground("Background");
			menuTheme = new Theme();
			menuTheme.Button = new Material(Shader.Position2DColorUV, "BackButtonDefault");
			menuTheme.ButtonMouseover = new Material(Shader.Position2DColorUV, "BackButtonHover");
			menuTheme.ButtonPressed = new Material(Shader.Position2DColorUV, "BackButtonHover");
			menuTheme.Slider = new Material(Shader.Position2DColorUV, "SliderBackground");
			menuTheme.SliderPointer = new Material(Shader.Position2DColorUV, "SliderDefault");
			menuTheme.SliderPointerMouseover = new Material(Shader.Position2DColorUV, "SliderHover");

			startGameTheme = new Theme();
			startGameTheme.Button = new Material(Shader.Position2DColorUV, "StartGameButtonDefault");
			startGameTheme.ButtonMouseover = new Material(Shader.Position2DColorUV,
				"StartGameButtonHover");
			startGameTheme.ButtonPressed = new Material(Shader.Position2DColorUV, "StartGameButtonHover");

			optionsTheme = new Theme();
			optionsTheme.Button = new Material(Shader.Position2DColorUV, "OptionsButtonDefault");
			optionsTheme.ButtonMouseover = new Material(Shader.Position2DColorUV, "OptionsButtonHover");
			optionsTheme.ButtonPressed = new Material(Shader.Position2DColorUV, "OptionsButtonHover");
			optionsTheme.Label = new Material(Shader.Position2DColorUV, "OptionsButtonDefault");

			exitTheme = new Theme();
			exitTheme.Button = new Material(Shader.Position2DColorUV, "ExitButtonDefault");
			exitTheme.ButtonMouseover = new Material(Shader.Position2DColorUV, "ExitButtonHover");
			exitTheme.ButtonPressed = new Material(Shader.Position2DColorUV, "ExitButtonHover");

			howToPlayTheme = new Theme();
			howToPlayTheme.Button = new Material(Shader.Position2DColorUV, "HowToPlayButtonDefault");
			howToPlayTheme.ButtonMouseover = new Material(Shader.Position2DColorUV,
				"HoWToPlayButtonHover");
			howToPlayTheme.ButtonPressed = new Material(Shader.Position2DColorUV, "HowtoPlayButtonHover");
		}

		private Theme menuTheme;
		private Theme startGameTheme;
		private Theme optionsTheme;
		private Theme exitTheme;
		private Theme howToPlayTheme;
	}
}