using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Multimedia;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace Blocks.Tests
{
	/// <summary>
	/// Unit tests for Game
	/// </summary>
	public class GameTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			displayMode = ScreenSpace.Current.Viewport.Aspect >= 1.0f
				? Orientation.Landscape : Orientation.Portrait;
			content = new JewelBlocksContent();
			game = new Game(Resolve<Window>(), content);
		}

		private Orientation displayMode;
		private JewelBlocksContent content;
		private Game game;
		//private IDisposable fixedRandomScope;

		[Test]
		public void CreateGameInPortrait()
		{
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(600, 800);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void CreateGameInLandscape()
		{
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(800, 600);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void AffixingBlockAddsToScore()
		{
			game.StartGame();
			Assert.AreEqual(0, game.UserInterface.Score);
			AdvanceTimeAndUpdateEntities(10.0f);
			Assert.AreEqual(1, game.UserInterface.Score);
		}

		[Test]
		public void CursorLeftMovesBlockLeft()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(5, game.Controller.FallingBlock.Left);
		}

		private void InitializeBlocks(Controller controller, JewelBlocksContent content)
		{
			controller.UpcomingBlock = new Block(displayMode, content, Vector2D.Zero);
			controller.FallingBlock = new Block(displayMode, content, new Vector2D(6, 1));
		}

		[Test]
		public void HoldingCursorLeftEventuallyMovesBlockLeftTwice()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(5, game.Controller.FallingBlock.Left);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(4, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void CursorRightMovesBlockRight()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void HoldingCursorRightEventuallyMovesBlockRightTwice()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(8, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void CursorDownDropsBlockFast()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorDown, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(game.Controller.IsFallingFast);
		}

		[Test]
		public void LeftHalfClickMovesBlockLeft()
		{
			var mockMouse = Resolve<MockMouse>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockMouse.SetPosition(new Vector2D(0.35f, 0.0f));
			mockMouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(5, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void RightHalfClickMovesBlockRight()
		{
			var mockMouse = Resolve<MockMouse>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockMouse.SetPosition(new Vector2D(0.65f, 0.0f));
			mockMouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void BottomHalfClickDropsBlockFast()
		{
			var mockMouse = Resolve<MockMouse>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockMouse.SetPosition(new Vector2D(0.5f, 0.6f));
			mockMouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(game.Controller.IsFallingFast);
		}

		[Test]
		public void LeftHalfTouchMovesBlockLeft()
		{
			var mockTouch = Resolve<MockTouch>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.35f, 0.0f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(5, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void RightHalfTouchMovesBlockRight()
		{
			var mockTouch = Resolve<MockTouch>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.65f, 0.0f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void BottomHalfTouchDropsBlockFast()
		{
			var mockTouch = Resolve<MockTouch>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.5f, 0.6f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(game.Controller.IsFallingFast);
		}

		[Test]
		public void MoveBlockLeftIsNotPossible()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			game.Controller.FallingBlock = null;
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
		}

		[Test]
		public void MoveBlockRightIsNotPossible()
		{
			var mockKeyboard = Resolve<MockKeyboard>();
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			game.Controller.FallingBlock = null;
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
		}

		//		[Test]
		//		public void CursorUpRotatesBlock()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					using (Randomizer.Use(new FixedRandom()))
		//					{
		//						InitializeBlocks(screenSpace, game.Controller, content);
		//						mockResolver.input.SetKeyboardState(Key.CursorUp, State.Pressing);
		//						Assert.AreEqual("OOOO/..../..../....", game.Controller.FallingBlock.ToString());
		//						mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//						Assert.AreEqual("O.../O.../O.../O...", game.Controller.FallingBlock.ToString());
		//					}
		//				});
		//		}
		//		[Test]
		//		public void TopHalfClickRotatesBlock()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					Assert.AreEqual("OOOO/..../..../....", game.Controller.FallingBlock.ToString());
		//					mockResolver.input.SetMousePosition(new Vector2D(0.5f, 0.4f));
		//					mockResolver.input.SetMouseButtonState(MouseButton.Left, State.Pressing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual("O.../O.../O.../O...", game.Controller.FallingBlock.ToString());
		//				});
		//		}

		//		[Test]
		//		public void TopHalfTouchRotatesBlock()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					Assert.AreEqual("OOOO/..../..../....", game.Controller.FallingBlock.ToString());
		//					mockResolver.input.SetTouchState(0, State.Pressing, new Vector2D(0.5f, 0.4f));
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual("O.../O.../O.../O...", game.Controller.FallingBlock.ToString());
		//				});
		//		}

		//		[TestFixtureTearDown]
		//		public void TearDown()
		//		{
		//			if (fixedRandomScope != null)
		//				fixedRandomScope.Dispose();
		//		}
	}
}