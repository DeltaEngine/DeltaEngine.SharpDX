using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	/// <summary>
	/// Just starts the Game class. For more complex examples see the other sample games.
	/// </summary>
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