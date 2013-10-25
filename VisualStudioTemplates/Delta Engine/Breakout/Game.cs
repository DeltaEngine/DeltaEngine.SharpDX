using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
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

		private void SizeChanged(Size size)
		{
			screenSpace.Zoom = (size.AspectRatio > 1) ? 1 / size.AspectRatio : size.AspectRatio;
		}

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
				restartCommand.IsActive = false;

			if (gameOverMessage != null)
				gameOverMessage.IsActive = false;

			score = new Score();
			currentLevel = new Level(score);
			ball = new BallInLevel(new Paddle(), currentLevel);
			new UI(window, this);
			score.GameOver += () => 
			{
				RemoveOldObjects();
				gameOverMessage = new FontText(Font.Default, "That's it.\nGame Over!", Rectangle.One);
				restartCommand = new Command(InitGame).Add(new KeyTrigger(Key.Space)).Add(new 
					MouseButtonTrigger()).Add(new TouchTapTrigger());
			};
			Score = score;
		}

		private Level currentLevel;

		private void RemoveOldObjects()
		{
			ball.Dispose();
			currentLevel.Dispose();
		}

		private BallInLevel ball;
		private Score score;
		private readonly Window window;
		private Command restartCommand;
		private FontText gameOverMessage;

		public Score Score
		{
			get;
			private set;
		}
	}
}