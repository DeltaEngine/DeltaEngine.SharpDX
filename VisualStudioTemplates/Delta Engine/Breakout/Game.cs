using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering.Fonts;
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
		}

		private readonly MainMenu menu;

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
			ball = new BallInLevel(new Paddle(), new Level(score));
			hudInterface = new UI(window, this, device);
			score.GameOver += () => 
			{
				ball.Dispose();
				gameOverMessage = new FontText(Font.Default, "That's it.\nGame Over!", Rectangle.One);
				restartCommand = new Command(InitGame).Add(new KeyTrigger(Key.Space)).Add(new 
					MouseButtonTrigger()).Add(new TouchTapTrigger());
			};
			Score = score;
		}

		private BallInLevel ball;
		private Score score;
		private UI hudInterface;
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