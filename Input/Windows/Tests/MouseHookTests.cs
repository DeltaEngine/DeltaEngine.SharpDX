using System;
using DeltaEngine.Platforms.Mocks;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	public class MouseHookTests
	{
		/*[Test]
		public void WindowsMouseHandleProcMessage()
		{
			IntPtr lParamHandle = GenerateMouseHookData(new[] { 0, 0, 0, 0, 0, 240 });
			WindowsMouse windowsMouse = GetMouse();
			Assert.AreEqual(0, windowsMouse.ScrollWheelValue);
			windowsMouse.Dispose();
			windowsMouse.hook.HandleProcMessage((IntPtr)MouseHook.WMMousewheel, lParamHandle, 0);
			windowsMouse.Run();
			Assert.GreaterOrEqual(windowsMouse.ScrollWheelValue, 240);
		}

		[Test]
		public void WindowsMouseHandleProcMessageButton()
		{
			IntPtr lParamHandle = GenerateMouseHookData(new[] { 50, 3, 0, 0, 0, 0x0201 });
			WindowsMouse windowsMouse = GetMouse();
			windowsMouse.Dispose();
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.Run();
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0202, lParamHandle, 0);
			windowsMouse.Run();
			Assert.AreEqual(State.Releasing, windowsMouse.LeftButton);
		}

		[Test]
		public void RunWithPressAndReleasesBetweenTicks()
		{
			IntPtr lParamHandle = GenerateMouseHookData(new[] { 50, 3, 0, 0, 0, 0x0201 });
			WindowsMouse windowsMouse = GetMouse();
			windowsMouse.Dispose();
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0202, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0201, lParamHandle, 0);
			windowsMouse.hook.HandleProcMessage((IntPtr)0x0202, lParamHandle, 0);
			windowsMouse.Run();

			// No matter what happens in one frame, we need to go to Pressing first.
			Assert.AreEqual(State.Pressing, windowsMouse.LeftButton);
			windowsMouse.Run();
			Assert.AreEqual(State.Releasing, windowsMouse.LeftButton);
			windowsMouse.Run();
			Assert.AreEqual(State.Released, windowsMouse.LeftButton);
		}

		private static unsafe IntPtr GenerateMouseHookData(int[] lParamData)
		{
			IntPtr lParamHandle;
			fixed (int* ptr = &lParamData[0])
				lParamHandle = (IntPtr)ptr;

			return lParamHandle;
		}

		private static WindowsMouse GetMouse()
		{
			var resolver = new MockResolver();
			var window = resolver.Window;
			var screen = new QuadraticScreenSpace(window);
			var positionTranslater = new CursorPositionTranslater(window, screen);
			return new WindowsMouse(positionTranslater);
		}*/

		[Test]
		public void TestIsPressed()
		{
			Assert.True(MouseHook.IsPressed(0x0201));
			Assert.False(MouseHook.IsPressed(0));
		}

		[Test]
		public void TestIsReleased()
		{
			Assert.True(MouseHook.IsReleased(0x00A2));
			Assert.False(MouseHook.IsReleased(0));
		}

		[Test]
		public void TestGetMessageButton()
		{
			Assert.AreEqual(MouseButton.Left, MouseHook.GetMessageButton(0x00A2, 0));
			Assert.AreEqual(MouseButton.Right, MouseHook.GetMessageButton(0x0205, 0));
			Assert.AreEqual(MouseButton.Middle, MouseHook.GetMessageButton(0x0209, 0));
			Assert.AreEqual(MouseButton.X1, MouseHook.GetMessageButton(0x020B, 65536));
			Assert.AreEqual(MouseButton.X2, MouseHook.GetMessageButton(0x020B, 0));
		}
	}
}