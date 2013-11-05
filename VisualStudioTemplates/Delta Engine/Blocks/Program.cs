using DeltaEngine.Core;
using DeltaEngine.Multimedia;
using DeltaEngine.Platforms;

namespace $safeprojectname$
{
	/// <summary>
	/// Falling blocks can be moved and rotated. Points are scored by filling rows.
	/// </summary>
	public class Program : App
	{
		//ncrunch: no coverage start
		public Program()
		{
			var blocksContent = new Fruit$safeprojectname$Content();
			new Game(Resolve<Window>(), blocksContent);
		}

		public static void Main()
		{
			new Program().Run();
		}
		//ncrunch: no coverage end
	}
}