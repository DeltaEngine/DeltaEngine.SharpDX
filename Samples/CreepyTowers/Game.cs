using DeltaEngine.Core;
using DeltaEngine.Scenes;

namespace CreepyTowers
{
	/// <summary>
	/// Starts a new Creepy Towers game
	/// </summary>
	public class Game
	{
		public Game(Window window)
		{
			Game.window = window;
			Game.window.Title = "Creepy Towers";
			//Game.window.ViewportPixelSize = new Size(1920, 1080);
			MaxZoomedOutFovSize = 6.0f;
			//CreateCamera = new GameCamera(MaxZoomedOutFovSize);
			//GameMainMenu = new MainMenu();
		}

		public static Window window;
		public float MaxZoomedOutFovSize { get; private set; }
		public static GameCamera CreateCamera { get; private set; }
		public Scene GameMainMenu { get; private set; }

		public static void EndGame()
		{
			window.CloseAfterFrame();
		}
	}
}