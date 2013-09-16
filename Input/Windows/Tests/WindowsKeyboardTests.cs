using System;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	public class WindowsKeyboardTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateWindowsKeayboard()
		{
			keyboard = new WindowsKeyboard();
		}

		private WindowsKeyboard keyboard;

		[Test]
		public void UpdateKeyboard()
		{
			var trigger = new KeyTrigger(Key.A, State.Pressing);
			keyboard.Update(new[]{trigger});
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void DisposeSetsUnavailable()
		{
			keyboard.Dispose();
			Assert.IsFalse(keyboard.IsAvailable);
		}
	}
}
