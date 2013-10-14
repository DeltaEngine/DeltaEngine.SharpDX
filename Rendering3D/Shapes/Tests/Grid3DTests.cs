using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Shapes3D.Tests
{
	public class Grid3DTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetupCameraFortyFiveDegreesView()
		{
			Camera.Use<LookAtCamera>(new Vector3D(0.0f, -5.0f, 5.0f));
		}

		[Test]
		public void RenderQuadraticGridWithSizeOfOne()
		{
			var grid = CreateQuadraticGrid(1);
			AssertQuadraticGrid(1, grid);
		}

		private static Grid3D CreateQuadraticGrid(int dimension)
		{
			return new Grid3D(dimension);
		}

		private static void AssertQuadraticGrid(int expectedDimension, Grid3D grid)
		{
			Assert.AreEqual(new Vector2D(expectedDimension, expectedDimension), grid.Dimension);
		}

		[Test]
		public void RenderQuadraticGridWithSizeOfTen()
		{
			var grid = CreateQuadraticGrid(10);
			AssertQuadraticGrid(10, grid);
		}
	}
}