using System;
using DeltaEngine.Platforms;

namespace DeltaEngine.Input.Tests
{
	internal static class Program
	{
		public static void Main()
		{
			//RunCodeVisually(new KeyboardTests(), tests =>
			//{
			//	tests.SetUp();
			//	tests.HandleInputVisually();
			//});
			//RunCodeVisually(new MouseDragTriggerTests(), tests => tests.DragMouseToCreateRectangles());
			//RunCodeVisually(new MouseDragTriggerTests(),
			//	tests => tests.DragMouseHorizontalToCreateRectangles());
			//RunCodeVisually(new MouseDragTriggerTests(),
			//	tests => tests.DragMouseVerticalToCreateRectangles());
			//RunCodeVisually(new MouseFlickTriggerTests(), tests => tests.ShowRedCircleOnFlick());
			RunCodeVisually(new GamePadTests(),
				tests => tests.PressingGamePadButtonShowsCircle());
		}

		private static void RunCodeVisually<T>(T tester, Action<T> runCode)
			where T : TestWithMocksOrVisually
		{
			tester.InitializeResolver();
			runCode(tester);
			tester.RunTestAndDisposeResolverWhenDone();
		}
	}
}