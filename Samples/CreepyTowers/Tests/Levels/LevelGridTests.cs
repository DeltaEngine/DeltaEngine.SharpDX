using System;
using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	public class LevelGridTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			grid = new LevelGrid(10, 0.5f);
		}

		private LevelGrid grid;

		[Test]
		public void DisplayGrid()
		{
			grid.DrawGrid();
			Assert.AreEqual(10, grid.GridSize);
			Assert.AreEqual(0.5, grid.GridScale);
		}

		[Test]
		public void WhenClickedPositionInGridIsInteractable()
		{
			var clickedPos = grid.PropertyMatrix[1, 2].MidPoint;
			var gridPos = grid.ComputeGridCoordinates(grid, clickedPos,
				new ChangeableList<Tuple<int, int>>
				{
					new Tuple<int, int>(1, 2),
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(1, 4)
				});

			Assert.IsTrue(grid.IsClickInGrid);
			Assert.AreEqual(clickedPos, gridPos);
		}

		[Test]
		public void WHenCLickedPositionInGridIs()
		{
			var clickedPos = grid.PropertyMatrix[3, 4].MidPoint;
			var gridPos = grid.ComputeGridCoordinates(grid, clickedPos,
				new ChangeableList<Tuple<int, int>>
				{
					new Tuple<int, int>(1, 2),
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(1, 4)
				});

			Assert.IsFalse(grid.IsClickInGrid);
			Assert.AreNotEqual(clickedPos, gridPos);
		}
	}
}