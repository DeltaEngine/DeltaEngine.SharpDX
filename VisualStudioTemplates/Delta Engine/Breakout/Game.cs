using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class Game
	{
		public Game(Window window, SoundDevice soundDevice)
		{
			new RelativeScreenSpace(window);
			this.window = window;
			device = soundDevice;
			menu = new MainMenu();
			menu.InitGame += InitGame;
			menu.QuitGame += window.CloseAfterFrame;
			window.ViewportPixelSize = new Size(900, 900);
			soundTrack = ContentLoader.Load<Music>("BreakoutMusic");
			soundTrack.Loop = true;
			soundTrack.Play();
			menu.SettingsChanged += UpdateMusicVolume;
		}

		private readonly MainMenu menu;
		private readonly Music soundTrack;

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

			new Background().RenderLayer = 0;
			score = new Score();
			currentLevel = new Level(score);
			ball = new BallInLevel(new Paddle(), currentLevel);
			new UI(window, this, device);
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
		private readonly SoundDevice device;
		private Command restartCommand;
		private FontText gameOverMessage;

		public Score Score
		{
			get;
			private set;
		}
	}
}