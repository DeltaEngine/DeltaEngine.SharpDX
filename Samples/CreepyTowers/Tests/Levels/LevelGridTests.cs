using System;
using CreepyTowers.Levels;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	public class LevelGridTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
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
		public void GridPositionCanBeClicked()
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
		public void WhenCLickedPositionIsInGrid()
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

		[Test]
		public void DisposingGridRemovesGrid()
		{
			grid.DrawGrid();
			grid.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Line3D>().Count);
		}

		[Test]
		public void HidingGridAfterDrawingHidesGridLines()
		{
			grid.DrawGrid();
			grid.ToggleVisibility(false);
			var gridLines = EntitiesRunner.Current.GetEntitiesOfType<Line3D>();
			foreach (Line3D gridLine in gridLines)
				Assert.IsFalse(gridLine.IsVisible);
		}

		[Test]
		public void ShowingGridAfterDrawingShowsGridLines()
		{
			grid.DrawGrid();
			grid.ToggleVisibility(true);
			var gridLines = EntitiesRunner.Current.GetEntitiesOfType<Line3D>();
			foreach (Line3D gridLine in gridLines)
				Assert.IsTrue(gridLine.IsVisible);
		}
	}
}