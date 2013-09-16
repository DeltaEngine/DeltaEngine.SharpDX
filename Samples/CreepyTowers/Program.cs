using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;

namespace CreepyTowers
{
	/// <summary>
	/// CreepyTowers Tower Defense game
	/// </summary>
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