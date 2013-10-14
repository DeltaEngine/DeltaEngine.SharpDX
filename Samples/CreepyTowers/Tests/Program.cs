using CreepyTowers.Tests.Simple2D;

namespace CreepyTowers.Tests
{
	public class Program
	{
		public static void Main()
	{
			//var test = new Tower2DTests();
		  //var test = new MainMenuTests();
			var test = new GameTests();
			test.InitializeResolver();
			//test.CreateMainMenu();
			test.CheckGameWithPathFinding();
			//test.CreateAllTowerTypes();
			test.RunTestAndDisposeResolverWhenDone();
		}
	}
}