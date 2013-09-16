using DeltaEngine.Core;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace ShadowShot.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			game = new Game(Resolve<Window>(),Resolve<ScreenSpace>());
			game.InitializeGame();
		}

		private Game game;

		[Test]
		public void DisplayGameWindow()
		{
			Assert.AreEqual((int)Constants.RenderLayer.Background, game.Background.RenderLayer);
		}

		[Test]
		public void CheckPlayerShipMoveLeft()
		{
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.A, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorLeft, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.A, State.Pressing);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void CheckPlayerShipMoveRight()
		{
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.D, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorRight, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.D, State.Pressing);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void StopPlayerShipMovement()
		{
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.S, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorDown, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.S, State.Pressing);
			AdvanceTimeAndUpdateEntities();
			keyboard.SetKeyboardState(Key.CursorDown, State.Pressing);
			AdvanceTimeAndUpdateEntities();
		}
	}
}