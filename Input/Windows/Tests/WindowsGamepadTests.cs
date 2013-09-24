using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	class WindowsGamepadTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateWindowsGamepad()
		{
			Resolve<GamePad>().Dispose();
			gamePad = new WindowsGamePad();
		}

		private WindowsGamePad gamePad;

		[Test]
		public void UpdateGamePad()
		{
			var buttonTrigger = new GamePadButtonTrigger(GamePadButton.A);
			var joyStickTrigger = new GamePadAnalogTrigger(GamePadAnalog.LeftTrigger);
			gamePad.Update(new Trigger[]{buttonTrigger, joyStickTrigger});

		}

		[Test]
		public void GetIdleStates()
		{
			Assert.AreEqual(State.Released, gamePad.GetButtonState(GamePadButton.A));
			Assert.AreEqual(Vector2D.Zero, gamePad.GetLeftThumbStick());
			Assert.AreEqual(Vector2D.Zero, gamePad.GetRightThumbStick());
			Assert.AreEqual(0.0f, gamePad.GetLeftTrigger());
			Assert.AreEqual(0.0f, gamePad.GetRightTrigger());
		}
	}
}
