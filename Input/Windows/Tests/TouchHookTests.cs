using System;
using DeltaEngine.Platforms.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	public class TouchHookTests
	{
		[SetUp]
		public void CreateHook()
		{
			resolver = new MockResolver();
			var window = resolver.Window;
			hook = new TouchHook(window);
		}

		private MockResolver resolver;
		private TouchHook hook;

		[TearDown]
		public void DisposeHook()
		{
			hook.Dispose();
			resolver.Dispose();
		}

		[Test]
		public void GetTouchDataFromHandleWithInvalidHandle()
		{
			var nativeTouches = TouchHook.GetTouchDataFromHandle(1, IntPtr.Zero);
			Assert.Null(nativeTouches);
		}

		[Test]
		public void HandleProcMessage()
		{
			//hook.HandleProcMessage((IntPtr)4, IntPtr.Zero, 0);
			Assert.IsEmpty(hook.nativeTouches);
		}
	}
}