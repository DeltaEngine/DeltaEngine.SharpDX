using DeltaEngine.Core;
using DeltaEngine.Datatypes;
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
			//fixedRandomScope = Randomizer.Use(new FixedRandom());
		}

		private Orientation displayMode;
		//private IDisposable fixedRandomScope;

		[Test]
		public void CreateGameInPortrait()
		{
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(600, 800);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void AffixingBlockAddsToScore()
		{
			var game = new Game(Resolve<Window>(), new JewelBlocksContent(), Resolve<SoundDevice>());
			game.StartGame();
			Assert.AreEqual(0, game.UserInterface.Score);
			AdvanceTimeAndUpdateEntities(10.0f);
			Assert.AreEqual(1, game.UserInterface.Score);
		}

		[Test]
		public void CursorLeftMovesBlockLeft()
		{
			var content = new JewelBlocksContent();
			var game = new Game(Resolve<Window>(), content, Resolve<SoundDevice>());
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			//mockResolver.input.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(6, game.Controller.FallingBlock.Left);
		}

		private void InitializeBlocks(Controller controller, JewelBlocksContent content)
		{
			controller.UpcomingBlock = new Block(displayMode, content, Vector2D.Zero);
			controller.FallingBlock = new Block(displayMode, content, new Vector2D(6, 1));
		}

		[Test]
		public void HoldingCursorLeftEventuallyMovesBlockLeftTwice()
		{
			var content = new JewelBlocksContent();
			var game = new Game(Resolve<Window>(), content, Resolve<SoundDevice>());
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			//mockResolver.input.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			//mockResolver.input.SetKeyboardState(Key.CursorLeft, State.Pressed);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreEqual(6, game.Controller.FallingBlock.Left);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreEqual(6, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void CursorRightMovesBlockRight()
		{
			var content = new JewelBlocksContent();
			var game = new Game(Resolve<Window>(), content, Resolve<SoundDevice>());
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			//mockResolver.input.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(6, game.Controller.FallingBlock.Left);
		}

		//		[Test]
		//		public void HoldingCursorRightEventuallyMovesBlockRightTwice()
		//		{
		//			InitializeBlocks(screenSpace, game.Controller, content);
		//			mockResolver.input.SetKeyboardState(Key.CursorRight, State.Pressing);
		//			mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//			mockResolver.input.SetKeyboardState(Key.CursorRight, State.Pressed);
		//			mockResolver.AdvanceTimeAndExecuteRunners(0.1f);
		//			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
		//			mockResolver.AdvanceTimeAndExecuteRunners(0.1f);
		//			Assert.AreEqual(8, game.Controller.FallingBlock.Left);
		//		}

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
		//		public void CursorDownDropsBlockFast()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
		//					mockResolver.input.SetKeyboardState(Key.CursorDown, State.Pressing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsTrue(game.Controller.IsFallingFast);
		//					mockResolver.input.SetKeyboardState(Key.CursorDown, State.Releasing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
		//				});
		//		}

		//		[Test]
		//		public void LeftHalfClickMovesBlockLeft()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					mockResolver.input.SetMousePosition(new Vector2D(0.35f, 0.0f));
		//					mockResolver.input.SetMouseButtonState(MouseButton.Left, State.Pressing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual(5, game.Controller.FallingBlock.Left);
		//				});
		//		}

		//		[Test]
		//		public void RightHalfClickMovesBlockRight()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					mockResolver.input.SetMousePosition(new Vector2D(0.65f, 0.0f));
		//					mockResolver.input.SetMouseButtonState(MouseButton.Left, State.Pressing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual(7, game.Controller.FallingBlock.Left);
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
		//		public void BottomHalfClickDropsBlockFast()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
		//					mockResolver.input.SetMousePosition(new Vector2D(0.5f, 0.6f));
		//					mockResolver.input.SetMouseButtonState(MouseButton.Left, State.Pressing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsTrue(game.Controller.IsFallingFast);
		//					mockResolver.input.SetMousePosition(new Vector2D(0.5f, 0.6f));
		//					mockResolver.input.SetMouseButtonState(MouseButton.Left, State.Releasing);
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
		//				});
		//		}

		//		[Test]
		//		public void LeftHalfTouchMovesBlockLeft()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					mockResolver.input.SetTouchState(0, State.Pressing, new Vector2D(0.35f, 0.0f));
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual(5, game.Controller.FallingBlock.Left);
		//				});
		//		}

		//		[Test]
		//		public void RightHalfTouchMovesBlockRight()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					mockResolver.input.SetTouchState(0, State.Pressing, new Vector2D(0.65f, 0.0f));
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.AreEqual(7, game.Controller.FallingBlock.Left);
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

		//		[Test]
		//		public void BottomHalfTouchDropsBlockFast()
		//		{
		//			Start(typeof(MockResolver),
		//				(ScreenSpace screenSpace, Game game, JewelBlocksContent content) =>
		//				{
		//					InitializeBlocks(screenSpace, game.Controller, content);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
		//					mockResolver.input.SetTouchState(0, State.Pressing, new Vector2D(0.5f, 0.6f));
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsTrue(game.Controller.IsFallingFast);
		//					mockResolver.input.SetTouchState(0, State.Releasing, new Vector2D(0.5f, 0.6f));
		//					mockResolver.AdvanceTimeAndExecuteRunners(0.01f);
		//					Assert.IsFalse(game.Controller.IsFallingFast);
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