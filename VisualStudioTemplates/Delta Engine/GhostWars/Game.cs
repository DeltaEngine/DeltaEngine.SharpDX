using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	public class Game : App
	{
		public Game()
		{
			var window = Resolve<Window>();
			new MainMenu(window);
		}

		static void Main()
		{
			new Game().Run();
		}
	}
}