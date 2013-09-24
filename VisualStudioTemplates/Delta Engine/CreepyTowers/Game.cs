using $safeprojectname$.GUI;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;

namespace $safeprojectname$
{
	public class Game
	{
		public Game(Window window, Device device)
		{
			Game.window = window;
			Game.device = device;
			Game.window.Title = "Creepy Towers";
			var viewPortSize = new Size(1920, 1080);
			Game.window.ViewportPixelSize = viewPortSize;
			MaxZoomedOutFovSize = 6.0f;
			CameraAndGrid = new CreateCameraAndGrid(MaxZoomedOutFovSize);
		}

		public static Window window;
		public static Device device;

		public float MaxZoomedOutFovSize
		{
			get;
			private set;
		}

		public static CreateCameraAndGrid CameraAndGrid
		{
			get;
			private set;
		}

		public MainMenu GameMainMenu
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