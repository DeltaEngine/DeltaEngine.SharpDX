using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace SideScroller
{
	internal class Program : App
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