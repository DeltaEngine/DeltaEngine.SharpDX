using CreepyTowers.Tests.Simple2D;
using DeltaEngine.Content;
using DeltaEngine.Content.Disk;

namespace CreepyTowers.Tests
{
	public class Program
	{
		public static void Main()
		{
			ContentLoader.Use<DiskContentLoader>();
			var test = new Tower2DTests();
			//var test = new IntroSceneTests();
			test.InitializeResolver();
			test.CreateGrid();
			//test.CreateAllTowerTypes();
			//test.ShowIntro();
			test.RunTestAndDisposeResolverWhenDone();
		}
	}
}