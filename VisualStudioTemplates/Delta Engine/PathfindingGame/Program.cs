using DeltaEngine.Platforms;
using Window = DeltaEngine.Core.Window;

namespace $safeprojectname$
{
	public class Program : App
	{
		public Program()
		{
			new Game(Resolve<Window>());
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}