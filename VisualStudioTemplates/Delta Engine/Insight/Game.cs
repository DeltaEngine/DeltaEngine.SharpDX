using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace $safeprojectname$
{
	public class Game : Entity
	{
		public Game(Window window)
		{
			InitializeWindow(window);
			//new MenuWithScenes();
			new MenuWithouScenes();
		}

		private void InitializeWindow(Window appWindow)
		{
			window = appWindow;
			window.BackgroundColor = Color.White;
			appWindow.Title = "$safeprojectname$";
			appWindow.ViewportPixelSize = new Size(720, 1280);
		}

		public Window window;
	}
}