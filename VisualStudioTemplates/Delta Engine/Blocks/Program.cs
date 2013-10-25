using DeltaEngine.Core;
using DeltaEngine.Multimedia;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	public class Program : App
	{
		public Program()
		{
			var blocksContent = new FruitBlocksContent();
			new Game(Resolve<Window>(), blocksContent);
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}