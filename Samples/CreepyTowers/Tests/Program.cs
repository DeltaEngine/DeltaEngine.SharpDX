using CreepyTowers.Tests.Simple2D;
using DeltaEngine.Content;
using DeltaEngine.Content.Disk;

namespace CreepyTowers.Tests
{
	public class Program
	{
		public static void Main()
		{
			//ContentLoader.Use<DiskContentLoader>();
			//var test = new Tower2DTests();
		  var test = new MainMenuTests();
			test.InitializeResolver();
      test.CreateMainMenu();
			//ContentLoader.DisposeIfInitialized();
			//test.CreateGrid();
			//test.CreateAllTowerTypes();
			//test.ShowIntro();
			test.RunTestAndDisposeResolverWhenDone();
			
			//var test = new TestWithBasic2DDisplaySystem();
			//test.InitializeResolver();
			//test.CreateGrid();
			//test.TestPerformanceAStar();
			//test.RunTestAndDisposeResolverWhenDone();
		}
	}
}