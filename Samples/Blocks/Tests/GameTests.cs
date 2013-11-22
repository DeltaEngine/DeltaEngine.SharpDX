using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Platforms.Mocks;
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
			mockResolver = new MockResolver();
			mockKeyboard = mockResolver.Resolve<MockKeyboard>();
			mockMouse = mockResolver.Resolve<MockMouse>();
			mockTouch = mockResolver.Resolve<MockTouch>();
			content = new JewelBlocksContent();
			game = new Game(Resolve<Window>());
		}

		private Orientation displayMode;
		private JewelBlocksContent content;
		private Game game;
		private MockResolver mockResolver;
		private MockKeyboard mockKeyboard;
		private MockMouse mockMouse;
		private MockTouch mockTouch;
		//private IDisposable fixedRandomScope1;

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
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			if (resolver.GetType() != typeof(MockResolver))
				return; //ncrunch: no coverage
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
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			if (resolver.GetType() != typeof(MockResolver))
				return; //ncrunch: no coverage
			Assert.AreEqual(5, game.Controller.FallingBlock.Left);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(4, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void CursorRightMovesBlockRight()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			if (resolver.GetType() != typeof(MockResolver))
				return; //ncrunch: no coverage
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void HoldingCursorRightEventuallyMovesBlockRightTwice()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			if (resolver.GetType() != typeof(MockResolver))
				return; //ncrunch: no coverage
			Assert.AreEqual(7, game.Controller.FallingBlock.Left);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(8, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void CursorDownDropsBlockFast()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockKeyboard.SetKeyboardState(Key.CursorDown, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(game.Controller.IsFallingFast);
		}

		[Test]
		public void LeftHalfClickMovesBlockLeft()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockMouse.SetPosition(new Vector2D(0.35f, 0.0f));
			mockMouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.05f);
			Assert.AreEqual(IndexOfSpawnColumn, game.Controller.FallingBlock.Left);
		}

		private const int IndexOfSpawnColumn = 6;

		[Test]
		public void RightHalfClickMovesBlockRight()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockMouse.SetPosition(new Vector2D(0.65f, 0.0f));
			mockMouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(IndexOfSpawnColumn, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void BottomHalfClickDropsBlockFast()
		{
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
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.35f, 0.0f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(IndexOfSpawnColumn, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void RightHalfTouchMovesBlockRight()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.65f, 0.0f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(IndexOfSpawnColumn, game.Controller.FallingBlock.Left);
		}

		[Test]
		public void BottomHalfTouchDropsBlockFast()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			mockTouch.SetTouchState(0, State.Pressing, new Vector2D(0.5f, 0.6f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(game.Controller.IsFallingFast);
		}

		[Test]
		public void MoveBlockLeftIsNotPossible()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			game.Controller.FallingBlock = null;
			mockKeyboard.SetKeyboardState(Key.CursorLeft, State.Pressing);
		}

		[Test]
		public void MoveBlockRightIsNotPossible()
		{
			game.StartGame();
			InitializeBlocks(game.Controller, content);
			game.Controller.FallingBlock = null;
			mockKeyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
		}
	}
}