namespace DeltaEngine.Input.Tests
{
	internal static class Program
	{
		public static void Main()
		{
			var keyboardTests = new KeyboardTests();
			keyboardTests.InitializeResolver();
			keyboardTests.SetUp();
			//keyboardTests.PressKeyToShowCircle();
			keyboardTests.HandleInputVisually();
			keyboardTests.RunTestAndDisposeResolverWhenDone();
		}
	}
}