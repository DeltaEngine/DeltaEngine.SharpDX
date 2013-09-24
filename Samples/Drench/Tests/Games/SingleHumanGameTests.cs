using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using Drench.Games;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace Drench.Tests.Games
{
	public class SingleHumanGameTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void SetUp()
		{
			Randomizer.Use(new FixedRandom(new[] { 0.1f, 0.6f, 0.7f, 0.2f }));
			InitializeMouse();
			AdvanceTimeAndUpdateEntities();
			game = new SingleHumanGame(BoardTests.Width, BoardTests.Height);
		}

		private Game game;

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Vector2D.Zero);
		}

		private MockMouse mouse;

		[Test]
		public void NewGameInstructions()
		{
			Assert.AreEqual("Try to complete the grid in the lowest number of turns!",
				game.upperText.Text);
		}

		[Test]
		public void ClickInvalidSquare()
		{
			var firstSquare = new Vector2D(ScreenSpace.Current.Left + Game.Border + 0.01f,
				ScreenSpace.Current.Top + Game.Border + 0.01f);
			ClickMouse(firstSquare);
			Assert.AreEqual("0 turns taken - Invalid Move!", game.upperText.Text);
		}

		private void ClickMouse(Vector2D position)
		{
			SetMouseState(State.Pressing, position);
			SetMouseState(State.Releasing, position);
		}

		private void SetMouseState(State state, Vector2D position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void ClickValidSquare()
		{
			ClickMouse(Vector2D.Half);
			Assert.AreEqual("1 turn taken", game.upperText.Text);
		}

		[Test]
		public void ClickSquaresUntilGameOverShowsGameOver()
		{
			ClickSquaresUntilGameOver();
		}

		private void ClickSquaresUntilGameOver()
		{
			bool hasExited = false;
			game.Exited += () => hasExited = true;
			MakeMovesThatFinishGame();
			Assert.IsFalse(hasExited);
			Assert.AreEqual("Game Over! Finished in 10 turns taken", game.upperText.Text);
		}

		private void MakeMovesThatFinishGame()
		{
			foreach (Vector2D move in WinningMoves)
				ClickMouse(move);
		}

		private static readonly List<Vector2D> WinningMoves = new List<Vector2D>
		{
			new Vector2D(0.2714286f, 0.355f),
			new Vector2D(0.3857143f, 0.355f),
			new Vector2D(0.5f, 0.355f),
			new Vector2D(0.6142857f, 0.355f),
			new Vector2D(0.7285714f, 0.355f),
			new Vector2D(0.8428571f, 0.355f),
			new Vector2D(0.8428571f, 0.4275f),
			new Vector2D(0.8428571f, 0.5f),
			new Vector2D(0.8428571f, 0.5725f),
			new Vector2D(0.8428571f, 0.645f)
		};

		[Test]
		public void ClickingSquareAfterGameOverExits()
		{
			bool hasExited = false;
			game.Exited += () => hasExited = true;
			MakeMovesThatFinishGame();
			ClickMouse(Vector2D.Half);
			Assert.IsTrue(hasExited);
		}
	}
}