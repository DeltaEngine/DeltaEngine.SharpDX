using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Networking;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using Drench.Games;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace Drench.Tests.Games
{
	public class TwoHumanNetworkGameTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void SetUp()
		{
			Randomizer.Use(new FixedRandom(new[] { 0.1f, 0.6f, 0.7f, 0.2f }));
			InitializeMouse();
			AdvanceTimeAndUpdateEntities();
			game1 = new TwoHumanNetworkGame(Messaging.StartSession(Port), BoardTests.Width,
				BoardTests.Height);
			game2 = new TwoHumanNetworkGame(Messaging.JoinSession("", Port),
				new Board.Data(BoardTests.Width, BoardTests.Height, ((TwoHumanNetworkGame)game1).Colors));
		}

		private const int Port = 16510;
		private Game game1;
		private Game game2;

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Point.Zero);
		}

		private MockMouse mouse;

		[Test]
		public void NewGameInstructions()
		{
			Assert.AreEqual("*** Player 1: 1  (Your turn) ***", game1.upperText.Text);
			Assert.AreEqual("Player 2: 1  ", game1.lowerText.Text);
			Assert.AreEqual("*** Player 1: 1  (Waiting for other player's turn) ***",
				game2.upperText.Text);
			Assert.AreEqual("Player 2: 1  ", game2.lowerText.Text);
		}

		[Test]
		public void ClickInvalidSquare()
		{
			var firstSquare = new Point(ScreenSpace.Current.Left + Game.Border + 0.01f,
				ScreenSpace.Current.Top + Game.Border + 0.01f);
			ClickMouse(firstSquare);
			Assert.AreEqual("*** Player 1: 1 - Invalid Move! ***", game1.upperText.Text);
			Assert.AreEqual("*** Player 1: 1 (Waiting for other player's turn) ***",
				game2.upperText.Text);
		}

		private void ClickMouse(Point position)
		{
			SetMouseState(State.Pressing, position);
			SetMouseState(State.Releasing, position);
		}

		private void SetMouseState(State state, Point position)
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
			ClickMouse(Point.Half);
			Assert.AreEqual(" Player 1: 3 Game Over! Player 1 wins! ", game1.upperText.Text);
			Assert.AreEqual(" Player 1: 3 Game Over! Player 1 wins! ", game2.upperText.Text);
		}
	}
}