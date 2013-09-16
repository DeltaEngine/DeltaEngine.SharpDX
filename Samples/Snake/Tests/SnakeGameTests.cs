using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Snake.Tests
{
	public class SnakeGameTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Init()
		{
			gridSize = 25;
			blockSize = 1.0f / gridSize;
			moveSpeed = 0.15f;
		}

		private float blockSize;
		private int gridSize;
		private float moveSpeed;

		[Test]
		public void StartGame()
		{
			new Game(Resolve<Window>());
		}

		[Test]
		public void RespawnChunkIfCollidingWithSnake()
		{
			var game = new Game(Resolve<Window>());
			game.StartGame();
			game.Chunk.DrawArea = game.Snake.Get<Body>().BodyParts[0].DrawArea;
			Assert.IsTrue(game.Chunk.IsCollidingWithSnake(game.Snake.Get<Body>().BodyParts));
			game.RespawnChunk();
			Assert.IsFalse(game.Chunk.IsCollidingWithSnake(game.Snake.Get<Body>().BodyParts));
		}

		[Test]
		public void SnakeEatsChunk()
		{
			var game = new Game(Resolve<Window>());
			game.StartGame();
			var snakeHead = game.Snake.Get<Body>().BodyParts[0].DrawArea;
			var direction = game.Snake.Get<Body>().Direction;
			var originalLength = game.Snake.Get<Body>().BodyParts.Count;
			game.Chunk.DrawArea =
				new Rectangle(new Point(snakeHead.Left + direction.X, snakeHead.Top + direction.Y),
					new Size(blockSize));
			game.MoveUp();
			AdvanceTimeAndUpdateEntities(moveSpeed);
			Assert.AreEqual(originalLength + 1, game.Snake.Get<Body>().BodyParts.Count);
		}

		[Test]
		public void DisplayGameOver()
		{
			var game = new Game(Resolve<Window>());
			game.StartGame();
			game.Reset();
		}
	}
}