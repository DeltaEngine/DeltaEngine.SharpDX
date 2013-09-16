using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	internal class Program : App
	{
		public Program()
		{
			for (int num = 0; num < 15; num++)
				new BouncingLogo();
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}