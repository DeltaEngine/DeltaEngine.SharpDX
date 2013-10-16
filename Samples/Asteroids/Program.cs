using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace Asteroids
{
	internal class Program : App
	{
		//ncrunch: no coverage start
		//Program does not really do anything and running from a test fixture causes conflicts
		public Program()
		{
			new Game(Resolve<Window>());
		}

		public static void Main()
		{
			new Program().Run();
		}
		//ncrunch: no coverage end
	}
}