using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.ScreenSpaces;

namespace Breakout
{
	/// <summary>
	/// Renders the background, ball, level and score; Also handles starting new levels
	/// </summary>
	public class Game : Scene
	{
		public Game(Window window)
		{
			screenSpace = new Camera2DScreenSpace(window);
			this.window = window;
			menu = new MainMenu();
			menu.InitGame += InitGame;
			menu.QuitGame += window.CloseAfterFrame;
			window.ViewportPixelSize = new Size(900, 900);
			soundTrack = ContentLoader.Load<Music>("BreakoutMusic");
			soundTrack.Loop = true;
			soundTrack.Play();
			MainMenu.SettingsChanged += UpdateMusicVolume;
			screenSpace.Zoom = 1 / window.ViewportPixelSize.AspectRatio;
			window.ViewportSizeChanged += SizeChanged;
			SetViewportBackground("Background");
		}

		private readonly MainMenu menu;
		private readonly Music soundTrack;
		private readonly Camera2DScreenSpace screenSpace;

		//ncrunch: no coverage start
		private void SizeChanged(Size size)
		{
			screenSpace.Zoom = (size.AspectRatio > 1) ? 1 / size.AspectRatio : size.AspectRatio;
		}
		//ncrunch: no coverage end

		private void UpdateMusicVolume()
		{
			soundTrack.Stop();
			soundTrack.Play();
		}

		private void InitGame()
		{
			if (menu != null)
				menu.Hide();
			if (restartCommand != null && restartCommand.IsActive)
				restartCommand.IsActive = false; //ncrunch: no coverage
			if (gameOverMessage != null)
				gameOverMessage.IsActive = false; //ncrunch: no coverage
			score = new Score();
			currentLevel = new Level(score);
			ball = new BallInLevel(new Paddle(), currentLevel);
			new UI(window, this);
			//ncrunch: no coverage start
			score.GameOver += () =>
			{
				RemoveOldObjects();
				gameOverMessage = new FontText(Font.Default, "That's it.\nGame Over!", Rectangle.One);
				restartCommand =
					new Command(InitGame).Add(new KeyTrigger(Key.Space)).Add(new MouseButtonTrigger()).Add(
						new TouchTapTrigger());
			};
			//ncrunch: no coverage end
			Score = score;
		}

		private Level currentLevel;

		//ncrunch: no coverage start
		private void RemoveOldObjects()
		{
			ball.Dispose();
			currentLevel.Dispose();
		}
		//ncrunch: no coverage end

		private BallInLevel ball;
		private Score score;
		private readonly Window window;
		private Command restartCommand;
		private FontText gameOverMessage;

		public Score Score { get; private set; }
	}
}