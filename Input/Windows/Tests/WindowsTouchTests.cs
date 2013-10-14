using DeltaEngine.Commands;
using DeltaEngine.Core;
using NUnit.Framework;
using DeltaEngine.Platforms;

namespace DeltaEngine.Input.Windows.Tests
{
	class WindowsTouchTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateWindowsTouchExplicitly()
		{
			Resolve<Touch>().Dispose();
			touch = new WindowsTouch(Resolve<Window>());
		}

		private WindowsTouch touch;

		[Test]
		public void UpdateTouch()
		{
			var dragTrigger = new TouchDragTrigger();
			var positionTrigger = new TouchPositionTrigger();
			touch.Update(new Trigger[]{dragTrigger, positionTrigger});
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(touch.GetPosition(0), positionTrigger.Position);
		}

		[TearDown]
		public void DisposeTouch()
		{
			touch.Dispose();
		}

		[Test]
		public void CheckNativeTouchInput()
		{
			var native = new NativeTouchInput(0, 1, 0, 0);
			Assert.AreEqual(0, native.Flags);
			Assert.AreEqual(1, native.Id);
			Assert.AreEqual(0, native.X);
			Assert.AreEqual(0, native.Y);
		}
	}
}
