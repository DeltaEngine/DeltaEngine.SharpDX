using DeltaEngine.Core;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;

namespace ShadowShot
{
	internal class Program : App
	{
		public Program()
		{

			new Game(Resolve<Window>(),Resolve<ScreenSpace>());
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}
