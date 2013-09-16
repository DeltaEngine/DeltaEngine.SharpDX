using DeltaEngine.Platforms;

namespace Drench
{
	internal class Program : App
	{
		public Program()
		{
			new DrenchMenu();
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}