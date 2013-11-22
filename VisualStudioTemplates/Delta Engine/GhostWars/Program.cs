using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	public class Program : App
	{
		public Program()
		{
			var window = Resolve<Window>();
			new MainMenu(window);
		}

		static void Main()
		{
			new Program().Run();
		}
	}
}