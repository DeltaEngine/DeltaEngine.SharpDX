using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace Blocks.Tests
{
	/// <summary>
	/// Unit tests for Block
	/// </summary>
	public class BlockTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			displayMode = ScreenSpace.Current.Viewport.Aspect >= 1.0f
				? Orientation.Landscape : Orientation.Portrait;
			content = new JewelBlocksContent();
		}

		private Orientation displayMode;
		private JewelBlocksContent content;

		[Test]
		public void ConstructorTopLeft()
		{
			var block = new Block(displayMode, content, new Vector2D(1, 2));
			Assert.AreEqual(1, block.Left);
			Assert.AreEqual(2, block.Top);
		}

		[Test, Ignore]
		public void RotateClockwise()
		{
			{
				var block = new Block(displayMode, content, new Vector2D(8, 1));
				Assert.AreEqual("O.../OOO./..../....", block.ToString());
				block.RotateClockwise();
				Assert.AreEqual("OO../O.../O.../....", block.ToString());
			}
		}

		//private static readonly float[] JBlock = new[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.9f };

		[Test, Ignore]
		public void RotateAntiClockwise()
		{
			//using (NUnit.Framework.Randomizer.Use(new FixedRandom(JBlock)))
			{
				var block = new Block(displayMode, content, new Vector2D(8, 1));
				Assert.AreEqual("O.../OOO./..../....", block.ToString());
				block.RotateAntiClockwise();
				Assert.AreEqual(".O../.O../OO../....", block.ToString());
			}
		}

		[Test]
		public void Left()
		{
			var shape = new Block(displayMode, content, Vector2D.Zero) { Left = 1 };
			Assert.AreEqual(1, shape.Left);
			Assert.AreEqual(1, shape.Bricks[0].TopLeftGridCoord.X);
			Assert.AreEqual(1, shape.Bricks[1].TopLeftGridCoord.X);
			Assert.AreEqual(1, shape.Bricks[2].TopLeftGridCoord.X);
			Assert.AreEqual(1, shape.Bricks[3].TopLeftGridCoord.X);
		}

		[Test]
		public void Top()
		{
			var shape = new Block(displayMode, content, Vector2D.Zero) { Top = 1 };
			Assert.AreEqual(1, shape.Top);
			Assert.AreEqual(1, shape.Bricks[0].TopLeftGridCoord.Y);
			Assert.AreEqual(1, shape.Bricks[1].TopLeftGridCoord.Y);
			Assert.AreEqual(1, shape.Bricks[2].TopLeftGridCoord.Y);
			Assert.AreEqual(1, shape.Bricks[3].TopLeftGridCoord.Y);
		}

		[Test, Ignore]
		public void RunMovesTheBlock()
		{
			var block = new Block(displayMode, content, Vector2D.Zero);
			AdvanceTimeAndUpdateEntities(0.0167f);
			block.UpdateBrickDrawAreas(2.0f);
			Assert.AreEqual(0.0333f, block.Top, 0.001f);
		}

		[Test]
		public void CheckIBlockAppearsATenthOfTheTime()
		{
			int count = 0;
			for (int i = 0; i < 1000; i++)
			{
				var block = new Block(displayMode, content, Vector2D.Zero);
				if (block.ToString() == "OOOO/..../..../...." || block.ToString() == "O.../O.../O.../O...")
					count++;
			}

			Assert.AreEqual(100, count, 50);
		}

		[Test]
		public void RenderJBlock()
		{
			//using (NUnit.Framework.Randomizer.Use(new FixedRandom(JBlock)))
			{
				var block = new Block(displayMode, content, Vector2D.Zero);
				block.UpdateBrickDrawAreas(0.0f);
			}
		}
	}
}