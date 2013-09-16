using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.ScreenSpaces;

namespace ShadowShot
{
	public class Game
	{
		public Game(Window window, ScreenSpace screenSpace)
		{
			this.window = window;
			this.screenSpace = screenSpace;
			mainMenu = new Menu();
			mainMenu.QuitGame += window.CloseAfterFrame;
			mainMenu.InitGame += () =>
			{
				mainMenu.Hide();
				InitializeGame();
			};
		}

		private readonly Window window;
		private readonly ScreenSpace screenSpace;
		private Menu mainMenu;

		public void InitializeGame()
		{
			if (restartCommand!=null && restartCommand.IsActive)
				restartCommand.IsActive = false;
			SetupPlayArea();
			SetupShip();
			SetupController();
			new GameInputControls(Ship);
		}

		private void SetupPlayArea()
		{
			window.Title = "ShadowShot Game";
			AddBackground();
		}

		private void AddBackground()
		{
			Background = new Sprite(new Material(Shader.Position2DColorUv,"starfield"), Rectangle.One);
			Background.RenderLayer = (int)Constants.RenderLayer.Background;
		}

		public Sprite Background { get; private set; }

		private void SetupShip()
		{
			Rectangle viewport = ScreenSpace.Current.Viewport;
			Ship = new PlayerShip(new Material(Shader.Position2DColorUv, "player"),
				Rectangle.FromCenter(viewport.Right / 2, viewport.Bottom * 0.93f, 0.04f, 0.04f), viewport);
		}

		public PlayerShip Ship { get; private set; }

		private void SetupController()
		{
			Controller = new GameController(Ship, new Material(Shader.Position2DColorUv, "asteroid"),
				objectSize, screenSpace);
			Controller.ShipCollidedWithAsteroid += RestartGame;
		}

		private readonly Size objectSize = new Size(0.05f);

		public GameController Controller { get; private set; }

		public void RestartGame()
		{
			Controller.Dispose();
			var gameOverMsg = new FontText(Font.Default,
				"Game Over!\nPress Space or tap/click for restart!", Rectangle.One);
			restartCommand = new Command(() =>
			{
				InitializeGame();
				gameOverMsg.IsActive = false;
			}).Add(new KeyTrigger(Key.Space)).Add(new TouchTapTrigger()).Add(new MouseButtonTrigger());
		}

		private Command restartCommand;
	}
}