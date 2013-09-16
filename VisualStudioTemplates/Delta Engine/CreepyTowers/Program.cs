using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	public class Program : App
	{
		public Program()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}