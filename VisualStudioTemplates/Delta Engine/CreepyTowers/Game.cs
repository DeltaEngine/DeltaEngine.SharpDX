using DeltaEngine.Core;
using DeltaEngine.Scenes;

namespace $safeprojectname$
{
	public class Game
	{
		public Game(Window window)
		{
			Game.window = window;
			Game.window.Title = "Creepy Towers";
			MaxZoomedOutFovSize = 6.0f;
		}

		public static Window window;

		public float MaxZoomedOutFovSize
		{
			get;
			private set;
		}

		public static GameCamera CreateCamera
		{
			get;
			private set;
		}

		public Scene GameMainMenu
		{
			get;
			private set;
		}

		public static void EndGame()
		{
			window.CloseAfterFrame();
		}
	}
}