//ncrunch: no coverage start
namespace DeltaEngine.Content.Xml.Tests
{
	public class Program
	{
		public static void Main()
		{
			var tests = new InputCommandsTests();
			tests.InitializeResolver();
			tests.TestInputCommands();
			//tests.RunTestAndDisposeResolverWhenDone();
			System.Threading.Thread.Sleep(1000);
		}
	}
}