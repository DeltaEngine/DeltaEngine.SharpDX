using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	internal class LevelTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			new Manager(7.0f);
		}

		[Test]
		public void CameraTest()
		{
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<OrthoCamera>().Count);
		}

		[Test]
		public void ShowChildrensRoomLevelWithGrid()
		{
			new Level(Names.LevelsChildrensRoom);
		}

		[Test]
		public void ShowBathroomLevelWithGrid()
		{
			new Level(Names.LevelsBathroom);
		}

		[Test]
		public void ShowLivingRoomLevelWithGrid()
		{
			new Level(Names.LevelsLivingRoom);
		}

		[Test]
		public void GameGridTests()
		{
			new Level(Names.LevelsLivingRoom);
			var grid = new LevelGrid(30, 0.5f);
			Assert.AreEqual(30, grid.GridSize);
			Assert.AreEqual(0.5, grid.GridScale);
		}

		[Test]
		public void GridMatrixCreation()
		{
			new Level(Names.LevelsLivingRoom);
			const int GridSize = 10;
			new LevelGrid(GridSize, 0.5f);
			var gridProp = new GridProperties[GridSize,GridSize];
			Assert.AreEqual(100, gridProp.Length);
			Assert.AreEqual("DeltaEngine.Datatypes.Vector3D", gridProp[0, 0].MidPoint.GetType().ToString());
			Assert.AreEqual("System.Boolean", gridProp[0, 0].IsOccupied.GetType().ToString());
			Assert.AreEqual("System.Boolean", gridProp[0, 0].IsActive.GetType().ToString());
		}

		[Test]
		public void GridMatrixImplementationTests()
		{
			new Level(Names.LevelsLivingRoom);
			const int GridSize = 10;
			var grid = new LevelGrid(GridSize, 0.5f);

			for (int i = 0; i < GridSize; i++)
				for (int j = 0; j < GridSize; j++)
				{
					grid.PropertyMatrix[i, j].MidPoint = new Vector3D(i, j, 0.0f);
					grid.PropertyMatrix[i, j].IsActive = true;
					grid.PropertyMatrix[i, j].IsOccupied = false;
				}

			var sampleMatrixLoc = grid.PropertyMatrix[2, 5];
			Assert.AreEqual(2, sampleMatrixLoc.MidPoint.X);
			Assert.AreEqual(5, sampleMatrixLoc.MidPoint.Y);
			Assert.IsTrue(sampleMatrixLoc.IsActive);
			Assert.IsFalse(sampleMatrixLoc.IsOccupied);

			sampleMatrixLoc.MidPoint = new Vector3D(3.0f, 1.0f, 5.5f);
			sampleMatrixLoc.IsOccupied = true;
			Assert.AreEqual(5.5f, sampleMatrixLoc.MidPoint.Z);
			Assert.IsTrue(sampleMatrixLoc.IsOccupied);
		}

		//[Test]
		//public void GridMatrixPositionTests()
		//{
		//	new Level(Names.LevelsLivingRoom);
		//	const int GridSize = 25;
		//	var grid = new LevelGrid(GridSize, 0.02f);

		//	var sampleMatrixLoc = grid.PropertyMatrix[0, 0];
		//	Assert.LessOrEqual(0.15f, sampleMatrixLoc.TopLeft.X);
		//	Assert.LessOrEqual(0.25f, sampleMatrixLoc.TopLeft.Z);

		//	sampleMatrixLoc = grid.PropertyMatrix[1, 1];
		//	Assert.LessOrEqual(0.13f, sampleMatrixLoc.TopLeft.X);
		//	Assert.LessOrEqual(0.23f, sampleMatrixLoc.TopLeft.Z);

		//	sampleMatrixLoc = grid.PropertyMatrix[3, 5];
		//	Assert.LessOrEqual(0.09f, sampleMatrixLoc.TopLeft.X);
		//	Assert.LessOrEqual(0.15f, sampleMatrixLoc.TopLeft.Z);
		//}

		//[Test, Ignore]
		//public void ChildsRoomXmlWaypointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsChildrensRoom);
		//	Assert.AreEqual(4, gameLevel.Get<Level.GridData>().CreepPathsList.Count);
		//	var waypointObjectList = gameLevel.Get<Level.GridData>().CreepPathsList;
		//	var wayPoint = new Tuple<int, int>(11, 7);
		//	Assert.AreEqual(wayPoint, waypointObjectList[0][0]);
		//	Assert.AreEqual("Start", waypointObjectList[0][0].Type);
		//}

		//[Test, Ignore]
		//public void ChildsRoomXmlInteractablePointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsChildrensRoom);
		//	Assert.AreEqual(53, gameLevel.Get<Level.GridData>().InteractablePoints.Count);
		//	var interactablePointsList = gameLevel.Get<Level.GridData>().InteractablePoints;
		//	var interactablePoint = new Tuple<int, int>(7, 9);
		//	Assert.AreEqual(interactablePoint, interactablePointsList[0]);
		//	Console.WriteLine((string)interactablePointsList[0]);
		//}

		//[Test, Ignore]
		//public void BathRoomXmlWaypointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsBathRoom);
		//	Assert.AreEqual(26, gameLevel.Get<Level.GridData>().WaypointObjects.Count);
		//	var waypointObjectList = gameLevel.Get<Level.GridData>().WaypointObjects;
		//	var wayPoint = new Tuple<int, int>(16, 1);
		//	Assert.AreEqual(wayPoint, waypointObjectList[0].Waypoint);
		//	Assert.AreEqual("Start", waypointObjectList[0].Type);
		//	Console.WriteLine((string)waypointObjectList[0].Waypoint);
		//	Console.WriteLine((string)waypointObjectList[0].Type);
		//}

		//[Test, Ignore]
		//public void BathRoomXmlInteractablePointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsBathRoom);
		//	Assert.AreEqual(130, gameLevel.Get<Level.GridData>().InteractablePoints.Count);
		//	var interactablePointsList = gameLevel.Get<Level.GridData>().InteractablePoints;
		//	var interactablePoint = new Tuple<int, int>(7, 13);
		//	Assert.AreEqual(interactablePoint, interactablePointsList[0]);
		//	Console.WriteLine((string)interactablePointsList[0]);
		//}

		//[Test, Ignore]
		//public void LivingRoomXmlWaypointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsLivingRoom);
		//	Assert.AreEqual(25, gameLevel.Get<Level.GridData>().WaypointObjects.Count);
		//	var waypointObjectList = gameLevel.Get<Level.GridData>().WaypointObjects;
		//	var wayPoint = new Tuple<int, int>(15, 2);
		//	Assert.AreEqual(wayPoint, waypointObjectList[0].Waypoint);
		//	Assert.AreEqual("Start", waypointObjectList[0].Type);
		//	Console.WriteLine((string)waypointObjectList[0].Waypoint);
		//	Console.WriteLine((string)waypointObjectList[0].Type);
		//}

		//[Test, Ignore]
		//public void LivingRoomXmlInteractablePointsTest()
		//{
		//	var gameLevel = new Level(Names.LevelsLivingRoom);
		//	Assert.AreEqual(181, gameLevel.Get<Level.GridData>().InteractablePoints.Count);
		//	var interactablePointsList = gameLevel.Get<Level.GridData>().InteractablePoints;
		//	var interactablePoint = new Tuple<int, int>(1, 14);
		//	Assert.AreEqual(interactablePoint, interactablePointsList[0]);
		//	Console.WriteLine((string)interactablePointsList[0]);
		//}
	}
}