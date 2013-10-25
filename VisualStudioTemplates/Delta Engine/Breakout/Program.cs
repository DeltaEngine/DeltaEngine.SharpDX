using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	public class Program : App
	{
		public Program()
		{
			Resolve<Settings>().UpdatesPerSecond = 60;
			new Game(Resolve<Window>());
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}