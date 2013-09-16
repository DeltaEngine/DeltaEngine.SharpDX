using DeltaEngine.Platforms.Mocks;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	public class MouseDeviceCounterTests
	{
		[Test]
		public void GetNumberOfAvailableMice()
		{
			Assert.DoesNotThrow(delegate
			{
				var counter = new MouseDeviceCounter();
				Assert.Greater(counter.GetNumberOfAvailableMice(), 0);
			});
		}

		[Test]
		public void DeviceCounterInMouseProperty()
		{
			var windowsMouse = GetMouse();
			Assert.IsTrue(windowsMouse.IsAvailable);
		}

		private static WindowsMouse GetMouse()
		{
			var resolver = new MockResolver();
			var window = resolver.Window;
			var positionTranslater = new CursorPositionTranslater(window);
			return new WindowsMouse(positionTranslater);
		}
	}
}